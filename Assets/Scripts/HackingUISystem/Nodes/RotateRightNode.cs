using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateRightNode : RotateNode
{
    public override Type AssociatedType()
    {
        return typeof(RotateRight);
    }
}
