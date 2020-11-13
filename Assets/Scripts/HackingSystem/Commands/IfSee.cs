using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfSee : If
{
    [SerializeReference]
    public string tag;

    public IfSee() { }
    public IfSee(string tag)
    {
        this.tag = tag;
    }

    public void SetTag(string tag)
    {
        this.tag = tag;
    }

    public override void Activate()
    {
        parentComputer.sensors.StartCoroutine(parentComputer.sensors.LookForTag(tag, retval => { if (retval) nextCommand = ifTrue; else nextCommand = ifFalse; })); //use of callback to return a value
    }

}