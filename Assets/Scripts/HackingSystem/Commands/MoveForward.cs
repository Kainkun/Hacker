using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : Command
{
    public float distance;
    public MoveForward(float distance = 1)
    {
        this.distance = distance;
    }

    public override bool Activate()
    {
        parentComputer.movement.MoveForward(distance);
        return true;
    }
}
