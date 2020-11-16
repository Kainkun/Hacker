using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveDirectionNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(MoveDirection);
    }

    public void SetDistance(string distance)
    {
        ((MoveDirection)attachedCommand).SetDistance(Int32.Parse(distance));
    }
}
