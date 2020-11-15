using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HackingUISystem : MonoBehaviour
{
    public static HackingUISystem instance;

    public GameObject trashBin;

    public Computer currentlyEditingComputer;
    public Program currentlyEditingProgram;
    public List<CommandNodeDragger> nodes = new List<CommandNodeDragger>();

    public GameObject PrintNode;
    public GameObject MoveForwardNode;
    public GameObject MoveBackNode;
    public GameObject MoveLeftNode;
    public GameObject MoveRightNode;
    public GameObject IfSeeNode;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        //TEMP
        currentlyEditingComputer = FindObjectOfType<Computer>();
        currentlyEditingProgram = new Program("UIMadeProgram", currentlyEditingComputer);
        currentlyEditingComputer.AddProgram(currentlyEditingProgram);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentlyEditingComputer.RunProgram(0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateNode(typeof(Print));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CreateNode(typeof(MoveForward));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CreateNode(typeof(MoveRight));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CreateNode(typeof(MoveBack));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CreateNode(typeof(MoveLeft));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateNode(typeof(IfSee));
        }

    }

    public void EditComputer(Computer computer)
    {
        currentlyEditingComputer = computer;
    }

    public void EditProgram(Program program)
    {
        currentlyEditingProgram = program;
    }

    public void DisplayProgram(Program prog)
    {
        float yInc = 150;
        float xInc = 300;
        float height = Screen.height;
        float width = Screen.width;
        Vector2 currentPos = new Vector2(xInc / 2, height - yInc / 2);

        foreach (var command in prog.GetCommands())
        {
            // GameObject node = Instantiate(genericCommandNode, transform);
            // Command c = currentlyEditingProgram.CreateCommand(command.GetType());
            // node.transform.position = currentPos;

            // currentPos.y -= yInc;
            // if (currentPos.y < yInc / 2)
            // {
            //     currentPos.y = height - yInc / 2;
            //     currentPos.x += xInc;
            // }
        }

    }

    public void DisplayNode()
    {

    }

    public void DeleteNode()
    {

    }

    public Command CreateNode(Type type)
    {

        Command command = currentlyEditingProgram.CreateCommand(type);

        if (type == typeof(Print))
            nodes.Add(Instantiate(PrintNode, transform).GetComponent<PrintNode>());
        // else if (type == typeof(Move))
        //     nodes.Add(Instantiate(genericCommandNode, transform).GetComponent<MoveNode>());
        else if (type == typeof(MoveForward))
            nodes.Add(Instantiate(MoveForwardNode, transform).GetComponent<MoveForwardNode>());
        else if (type == typeof(MoveBack))
            nodes.Add(Instantiate(MoveBackNode, transform).GetComponent<MoveBackNode>());
        else if (type == typeof(MoveLeft))
            nodes.Add(Instantiate(MoveLeftNode, transform).GetComponent<MoveLeftNode>());
        else if (type == typeof(MoveRight))
            nodes.Add(Instantiate(MoveRightNode, transform).GetComponent<MoveRightNode>());
        else if (type == typeof(IfSee))
            nodes.Add(Instantiate(IfSeeNode, transform).GetComponent<IfSeeNode>());

        nodes[nodes.Count-1].attachedCommand = command;
        command.SetConnectedNode(nodes[nodes.Count - 1]);
        return command;
    }


}
