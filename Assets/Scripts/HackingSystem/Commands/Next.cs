using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Next : Command
{
    public virtual void SetNextCommand(Command command)
    {
        nextCommand = command;
    }
}
