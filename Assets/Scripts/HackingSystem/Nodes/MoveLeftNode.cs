using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveLeftNode : CommandNodeDragger
{
    public override Type AssociatedType()
    {
        return typeof(MoveLeft);
    }
}
