using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : Command
{
    public float distance;
    public MoveBack(float distance = 1)
    {
        this.distance = distance;
    }

    public override bool Activate()
    {
        parentComputer.movement.MoveBackwards(distance);
        return true;
    }
}
