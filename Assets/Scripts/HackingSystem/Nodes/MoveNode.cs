﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveNode : CommandNodeDragger
{
    public override Type AssociatedType()
    {
        return typeof(Move);
    }
}
