using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class If : Command
{
    [SerializeReference]
    protected Command ifTrue;
    [SerializeReference]
    protected Command ifFalse;

    public void SetIfTrue(Command command)
    {
        ifTrue = command;
    }

    public void SetIfFalse(Command command)
    {
        ifFalse = command;
    }
}
