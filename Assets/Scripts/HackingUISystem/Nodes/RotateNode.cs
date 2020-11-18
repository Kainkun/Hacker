using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(Rotate);
    }
}
