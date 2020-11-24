using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickupDropNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(PickupDrop);
    }
}
