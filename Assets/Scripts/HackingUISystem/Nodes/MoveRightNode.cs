﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveRightNode : RotateNode
{
    public override Type AssociatedType()
    {
        return typeof(MoveRight);
    }
}
