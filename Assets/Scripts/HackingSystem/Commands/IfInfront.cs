using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfInfront : If
{
    [SerializeReference]
    public string tag;

    public IfInfront() { }
    public IfInfront(string tag)
    {
        this.tag = tag;
    }

    public void SetTag(string tag)
    {
        this.tag = tag;
    }

    public override void Activate()
    {
        parentProgram.parentComputer.sensors.StartCoroutine(parentProgram.parentComputer.sensors.LookForTag(tag, retval => { if (retval) nextCommand = ifTrue; else nextCommand = ifFalse; }, true)); //use of callback to return a value
    }
}
