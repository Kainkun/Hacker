using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : Command
{
    public float distance;
    public MoveLeft(float distance = 1)
    {
        this.distance = distance;
    }

    public override bool Activate()
    {
        parentComputer.movement.MoveLeft(distance);
        return true;
    }
}
