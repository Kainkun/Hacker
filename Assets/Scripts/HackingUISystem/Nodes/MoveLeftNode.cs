﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveLeftNode : MoveDirectionNode
{
    public override Type AssociatedType()
    {
        return typeof(MoveLeft);
    }
}
