using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Program
{
    //TODO: make sure parentComputer is a reference and not a new Computer
    public Computer parentComputer;
    public string name;
    public Program(string name, bool noStartNode = false)
    {
        this.name = name;

        if (!noStartNode)
        {
            Start command = (Start)AddCommand(new Start());
            command.connectedNodePosition = new Vector2(20, 15);
        }
    }

    [SerializeReference]
    List<Command> commands = new List<Command>(); //list of commands that the program will be running

    public Command AddCommand(Command command)
    {
        command.parentProgram = this;
        commands.Add(command);
        return command;
    }

    public void RemoveCommand(Command command)
    {
        commands.Remove(command);
    }

    public Command GetCommand(int i)
    {
        if (i < commands.Count)
        {
            return commands[i];
        }
        return null;
    }

    public List<Command> GetCommands()
    {
        return commands;
    }


    public Command CreateCommand(Type type)
    {

        if (type == typeof(Print))
            return AddCommand(new Print());
        else if (type == typeof(Move))
            return AddCommand(new Move());
        else if (type == typeof(MoveForward))
            return AddCommand(new MoveForward());
        else if (type == typeof(MoveBack))
            return AddCommand(new MoveBack());
        else if (type == typeof(MoveLeft))
            return AddCommand(new MoveLeft());
        else if (type == typeof(MoveRight))
            return AddCommand(new MoveRight());
        else if (type == typeof(IfSee))
            return AddCommand(new IfSee());
        else if (type == typeof(Start))
            return AddCommand(new Start());

        Debug.LogError("Command missing");
        return null;
    }


}
