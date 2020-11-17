using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(Start);
    }
}
