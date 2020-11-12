using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrintNode : CommandNodeDragger
{
    public override Type AssociatedType()
    {
        return typeof(Print);
    }
}
