using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveBackNode : CommandNodeDragger
{
    public override Type AssociatedType()
    {
        return typeof(MoveBack);
    }
}
