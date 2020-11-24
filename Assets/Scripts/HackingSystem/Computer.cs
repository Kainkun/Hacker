using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Computer : MonoBehaviour
{
    public float tickTime = 1;
    public bool RunProgramOnStart;
    public string[] preloadedPrograms;
    public List<Program> programs = new List<Program>(); //a computer has a list of programs it can run

    //Modules
    [HideInInspector]
    public Movement movementModule;
    [HideInInspector]
    public Sensors sensorsModule;
    [HideInInspector]
    public Interaction interactionModule;
    public GameObject ps_explosion;
    public Rigidbody2D rb;

    private void Awake()
    {
        AddModules();
        LoadPreloadedPrograms();
        rb = GetComponent<Rigidbody2D>();
    }

    void AddModules()
    {
        movementModule = GetComponent<Movement>();
        sensorsModule = GetComponent<Sensors>();
        interactionModule = GetComponent<Interaction>();

        movementModule?.SetParentComputer(this);
        sensorsModule?.SetParentComputer(this);
        interactionModule?.SetParentComputer(this);
    }

    void LoadPreloadedPrograms()
    {
        foreach (var name in preloadedPrograms)
        {
            AddProgram(TxtToProgram(name));
        }
    }

    private void Start()
    {
        if (RunProgramOnStart && programs.Count > 0)
            RunProgram(0);
        //AddProgram(CreateTestProgramSight());
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     RunProgram(programs[0]);
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     ProgramToTxt(programs[0]); //Export the program as a JSON
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     AddProgram(TxtToProgram("DefaultProgram.txt"));
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (!rb || (rb && rb.isKinematic) || other.gameObject.GetComponent<Computer>())
        {
            Destroy(Instantiate(ps_explosion, transform.position, Quaternion.identity), 0.5f);
            Destroy(gameObject);
        }
    }


    // public Program CreateTestProgram()
    // {
    //     Program program = new Program("Test_Program", this);

    //     //create different commands the program will run
    //     Print print1 = new Print("First Print");
    //     program.AddCommand(print1);
    //     Move move1 = new Move(new Vector2(0, 1));
    //     program.AddCommand(move1);
    //     Print print2 = new Print("Second Print");
    //     program.AddCommand(print2);

    //     //create the flow of commands, first command runs first
    //     print1.SetNextCommand(move1);
    //     move1.SetNextCommand(print2);

    //     //add the programs to the program's list of commands
    //     //TODO: maybe this can be combined with the creattion of a command?

    //     return program;
    // }

    // public Program CreateTestProgramSight()
    // {
    //     Program program = new Program("SightTest_Program", this);

    //     MoveForward move1 = new MoveForward();
    //     program.AddCommand(move1);
    //     MoveBack move2 = new MoveBack();
    //     program.AddCommand(move2);
    //     IfSee ifSee = new IfSee("Player");
    //     program.AddCommand(ifSee);

    //     ifSee.SetIfFalse(ifSee);
    //     ifSee.SetIfTrue(move1);

    //     move1.SetNextCommand(move2);
    //     move2.SetNextCommand(ifSee);

    //     return program;
    // }

    public void CreateProgram(string name = "DefaultProgram")
    {
        var p = new Program(name);
        AddProgram(p);
    }

    public void AddProgram(Program program)
    {
        program.parentComputer = this;
        foreach (var command in program.GetCommands())
            command.parentProgram = program;
        programs.Add(program);
    }

    public void ProgramToTxt(Program program) //Stores the program as a JSON
    {
        string prog = JsonUtility.ToJson(program, true);
        StreamWriter writer = new StreamWriter($"Assets/Resources/Programs/{program.name}.txt");
        writer.WriteLine(prog);
        writer.Close();
        print($"Done saving program: {program.name}.txt");
    }

    public Program TxtToProgram(string fileName)
    {
        StreamReader reader = new StreamReader($"Assets/Resources/Programs/{fileName}.txt");
        string prog = reader.ReadToEnd();
        Program program = JsonUtility.FromJson<Program>(prog);
        program.name = fileName;
        print($"Done loading program: {fileName}");

        return program;
    }



    #region Running Programs
    Coroutine runAttempt;
    Coroutine currentRunningProgram;
    bool runningProgram;
    public void RunProgram(int i)
    {
        if (i < programs.Count)
            RunProgram(programs[i]);
        else
            Debug.LogError("bad Program index");
    }
    public void RunProgram(Program program) //if program is alreay running, stop it gracefully by waiting for it the finish its last command, then run the new program once its fully stopped
    {
        if (currentRunningProgram != null)
            StopCurrentProgram();
        if (runAttempt != null)
            StopCoroutine(runAttempt);
        runAttempt = StartCoroutine(_TryToRunProgram(program));
    }
    public void StopCurrentProgram()//allows the current program to finish its current command
    {
        runningProgram = false;
    }
    IEnumerator _TryToRunProgram(Program program)//wait for the current program to finish before starting the new program
    {
        while (currentRunningProgram != null)
        {
            yield return null;
        }
        runAttempt = null;
        currentRunningProgram = StartCoroutine(_RunProgram(program));
    }
    IEnumerator _RunProgram(Program program)
    {
        runningProgram = true;
        Command currentCommand = program.GetCommand(0);
        while (runningProgram && currentCommand != null)
        {
            currentCommand.connectedNode?.ActivateIcon();
            currentCommand.Activate();

            yield return new WaitForSeconds(tickTime);
            currentCommand.connectedNode?.ActivateIcon(false);

            Command nextCommand = currentCommand.GetNextCommand();
            if (nextCommand == null)
                break;

            currentCommand = nextCommand;
        }

        runningProgram = false;
        currentRunningProgram = null;
        print("program done");
    }
    #endregion

}