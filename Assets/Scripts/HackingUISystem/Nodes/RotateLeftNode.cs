using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateLeftNode : RotateNode
{
    public override Type AssociatedType()
    {
        return typeof(RotateLeft);
    }
}
