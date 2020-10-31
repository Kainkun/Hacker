using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : Command
{
    public float distance;
    public MoveRight(float distance = 1)
    {
        this.distance = distance;
    }

    public override bool Activate()
    {
        parentComputer.movement.MoveRight(distance);
        return true;
    }
}
