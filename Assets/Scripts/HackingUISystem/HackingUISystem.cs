using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class HackingUISystem : MonoBehaviour
{
    public static HackingUISystem instance;

    public GameObject trashBin;
    public TMP_InputField programNameInputField;

    Computer currentlyEditingComputer;
    Program currentlyEditingProgram;
    public List<CommandNode> nodes = new List<CommandNode>();

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

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     currentlyEditingComputer.RunProgram(0);
        // }
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     CreateNode(typeof(Print));
        // }
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     CreateNode(typeof(MoveForward));
        // }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     CreateNode(typeof(MoveRight));
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     CreateNode(typeof(MoveBack));
        // }
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     CreateNode(typeof(MoveLeft));
        // }
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     CreateNode(typeof(IfSee));
        // }

    }

    public Computer GetCurrentlyEditingComputer()
    {
        return currentlyEditingComputer;
    }

    public Program GetCurrentlyEditingProgram()
    {
        return currentlyEditingProgram;
    }

    public void EditComputer(Computer computer)
    {
        currentlyEditingComputer = computer;
        if (computer.programs.Count == 0)
        {
            EditProgram(new Program("DefaultProgram", currentlyEditingComputer));
            currentlyEditingComputer.AddProgram(currentlyEditingProgram);
        }
        else
        {
            EditProgram(computer.programs[0]);
        }
            DisplayProgram(computer.programs[0]);
    }

    public void EditProgram(Program program)
    {
        currentlyEditingProgram = program;
    }

    public void CloseUI()
    {
        while (nodes.Count > 0)
            nodes[0].HideNode();
        currentlyEditingProgram = null;
        currentlyEditingComputer = null;
        Player.instance.EnableInput();
        gameObject.SetActive(false);
    }

    public void DisplayProgram(Program prog)
    {
        programNameInputField.text = prog.name;
        
        foreach (var command in prog.GetCommands())
            DisplayCommandAsNode(command);
        foreach (var node in nodes)
            node.DisplayConnections();
    }

    public CommandNode CreateNode(GameObject nodePrefab)
    {
        GameObject nodeObject = Instantiate(nodePrefab, transform);
        CommandNode node = nodeObject.GetComponent<CommandNode>();
        Command command = currentlyEditingProgram.CreateCommand(node.AssociatedType());

        nodes.Add(node);
        node.attachedCommand = command;
        command.SetConnectedNode(node);

        return node;
    }

    public CommandNode DisplayCommandAsNode(Command command)
    {
        GameObject nodeObject = Instantiate(GetNodePrefab(command), transform);
        CommandNode node = nodeObject.GetComponent<CommandNode>();

        nodes.Add(node);
        node.attachedCommand = command;
        command.SetConnectedNode(node);

        node.transform.localPosition = command.connectedNodePosition;

        if (node is PrintNode)
            ((PrintNode)node).SetInputField();
        if (node is IfSeeNode)
            ((IfSeeNode)node).SetInputField();

        return node;
    }

    public void CreateNodeButton(GameObject nodePrefab)
    {
        CreateNode(nodePrefab);
    }

    GameObject GetNodePrefab(Command command)
    {
        if (command is Print)
            return PrintNode;
        else if (command is MoveForward)
            return MoveForwardNode;
        else if (command is MoveBack)
            return MoveBackNode;
        else if (command is MoveRight)
            return MoveRightNode;
        else if (command is MoveLeft)
            return MoveLeftNode;
        else if (command is IfSee)
            return IfSeeNode;

        return null;
    }

    // public Command CreateNode(Type type)
    // {

    //     Command command = currentlyEditingProgram.CreateCommand(type);

    //     if (type == typeof(Print))
    //         nodes.Add(Instantiate(PrintNode, transform).GetComponent<PrintNode>());
    //     // else if (type == typeof(Move))
    //     //     nodes.Add(Instantiate(genericCommandNode, transform).GetComponent<MoveNode>());
    //     else if (type == typeof(MoveForward))
    //         nodes.Add(Instantiate(MoveForwardNode, transform).GetComponent<MoveForwardNode>());
    //     else if (type == typeof(MoveBack))
    //         nodes.Add(Instantiate(MoveBackNode, transform).GetComponent<MoveBackNode>());
    //     else if (type == typeof(MoveLeft))
    //         nodes.Add(Instantiate(MoveLeftNode, transform).GetComponent<MoveLeftNode>());
    //     else if (type == typeof(MoveRight))
    //         nodes.Add(Instantiate(MoveRightNode, transform).GetComponent<MoveRightNode>());
    //     else if (type == typeof(IfSee))
    //         nodes.Add(Instantiate(IfSeeNode, transform).GetComponent<IfSeeNode>());

    //     nodes[nodes.Count - 1].attachedCommand = command;
    //     command.SetConnectedNode(nodes[nodes.Count - 1]);
    //     return command;
    // }

    public void RunProgram(int programIndex = 0)
    {
        currentlyEditingComputer.RunProgram(programIndex);
    }

    public void StopCurrentProgram()
    {
        currentlyEditingComputer.StopCurrentProgram();
    }

    public void SetCurrentProgramName(string name)
    {
        currentlyEditingProgram.name = name;
    }

}
