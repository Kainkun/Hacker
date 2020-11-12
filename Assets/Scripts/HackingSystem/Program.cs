using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Program
{

    //TODO: make sure parentComputer is a reference and not a new Computer
    public Computer parentComputer;
    public string name;
    //TODO: is there a better way to get parent? is there a better way to handle parent?
    public Program(string name, Computer parentComputer)
    {
        this.name = name;
        this.parentComputer = parentComputer;
    }

    [SerializeReference]
    List<Command> commands = new List<Command>(); //list of commands that the program will be running

    public Command AddCommand(Command command)
    {
        command.parentComputer = parentComputer;
        command.parentProgram = this;
        commands.Add(command);
        return command;
    }

    public Command GetCommand(int i)
    {
        return commands[i];
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

        Debug.LogError("Command missing");
        return null;
    }


}
