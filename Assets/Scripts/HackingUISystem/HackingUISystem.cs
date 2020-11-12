using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HackingUISystem : MonoBehaviour
{
    public static HackingUISystem instance;

    static Computer currentlyEditingComputer;
    static Program currentlyEditingProgram;
    List<CommandNodeDragger> nodes = new List<CommandNodeDragger>();

    public GameObject genericCommandNode;
    public GameObject PrintNode;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        //TEMP
        currentlyEditingComputer = FindObjectOfType<Computer>();
        //currentlyEditingProgram = currentlyEditingComputer.programs[0];
        currentlyEditingProgram = new Program("UIMadeProgram", currentlyEditingComputer);
        currentlyEditingComputer.programs[0] = currentlyEditingProgram;
        //DisplayProgram(currentlyEditingProgram);
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
            GameObject node = Instantiate(genericCommandNode, transform);
            Command c = currentlyEditingProgram.CreateCommand(command.GetType());
            node.transform.position = currentPos;

            currentPos.y -= yInc;
            if (currentPos.y < yInc / 2)
            {
                currentPos.y = height - yInc / 2;
                currentPos.x += xInc;
            }
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
        else if (type == typeof(Move))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<MoveNode>());
        else if (type == typeof(MoveForward))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<MoveForwardNode>());
        else if (type == typeof(MoveBack))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<MoveBackNode>());
        else if (type == typeof(MoveLeft))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<MoveLeftNode>());
        else if (type == typeof(MoveRight))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<MoveRightNode>());
        else if (type == typeof(IfSee))
            nodes.Add(Instantiate(genericCommandNode, transform).AddComponent<IfSeeNode>());

        nodes[nodes.Count-1].attachedCommand = command;
        return command;
    }


}
