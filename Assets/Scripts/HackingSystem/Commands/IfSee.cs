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
        List<GameObject> objects = parentComputer.sensors.ObjectsInView();

        foreach (var item in objects)
        {
            if (item.tag == tag)
            {
                nextCommandIndex = 1;
                return true;
            }
        }

        return true;
    }

    public override Command GetNextCommand()
    {
        return nextCommands[nextCommandIndex];
    }
}
