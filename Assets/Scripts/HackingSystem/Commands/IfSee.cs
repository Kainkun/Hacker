using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfSee : Command
{
    public string tag;
    int nextCommandIndex;

    public IfSee(string tag)
    {
        this.tag = tag;
    }

    public override bool Activate()
    {
        nextCommandIndex = 0;

        parentComputer.sensors.StartCoroutine(parentComputer.sensors.LookForTag(tag, retval => { if (retval) nextCommandIndex = 1; })); //use of callback to return a value

        return true;
    }

    public override Command GetNextCommand()
    {
        return nextCommands[nextCommandIndex];
    }


}
