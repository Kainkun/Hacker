using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IfSeeNode : IfNode
{
    public override Type AssociatedType()
    {
        return typeof(IfSee);
    }

    public void SetTag(string tag)
    {
        ((IfSee)attachedCommand).SetTag(tag);
    }
}