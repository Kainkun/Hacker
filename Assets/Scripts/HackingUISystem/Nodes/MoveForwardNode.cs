using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveForwardNode : MoveDirectionNode
{
    public override Type AssociatedType()
    {
        return typeof(MoveForward);
    }
}
