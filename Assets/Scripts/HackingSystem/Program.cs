using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public T CreateCommand<T>(T command) where T : Command
    {
        AddCommand(command);
        return command;
    }

    public void AddCommand(Command command)
    {
        command.parentComputer = parentComputer;
        command.parentProgram = this;
        commands.Add(command);
    }

    public Command GetCommand(int i)
    {
        return commands[i];
    }

}
