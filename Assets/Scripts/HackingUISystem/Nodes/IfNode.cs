using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IfNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(If);
    }


}
