using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IfSeeNode : CommandNodeDragger
{
    public override Type AssociatedType()
    {
        return typeof(IfSee);
    }
}