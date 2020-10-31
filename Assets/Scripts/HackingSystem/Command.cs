using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class  Command
{
    [SerializeReference]
    public Computer parentComputer;
    [SerializeReference]
    public Program parentProgram;
    //TODO: make sure theyre pointers to commands
    [SerializeReference]
    protected List<Command> nextCommands = new List<Command>(); //list of potential commands to run next. Its the branches on a tree
    public abstract bool Activate();

    //TODO: Getter setters?
    public void AddNextCommand(Command command)
    {
        nextCommands.Add(command);
    }

    public virtual Command GetNextCommand()
    {
        if(nextCommands.Count == 0)
            return null;
        else
            return nextCommands[0];
    }
}
