using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Command
{
    [SerializeReference]
    public Computer parentComputer;
    [SerializeReference]
    public Program parentProgram;
    public CommandNodeDragger connectedNode;

    //TODO: make sure theyre pointers to commands
    [SerializeReference]
    protected Command nextCommand; //list of potential commands to run next. Its the branches on a tree
    public abstract void Activate();

    public virtual Command GetNextCommand()
    {
        return nextCommand;
    }

    public void SetConnectedNode(CommandNodeDragger node)
    {
        connectedNode = node;
    }
}
