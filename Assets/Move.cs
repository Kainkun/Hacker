using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Command
{
    public Vector2 directionToMove;

    public Move(Vector2 direction)
    {
        directionToMove = direction;
    }

    public override bool Activate()
    {
        parentComputer.transform.Translate(directionToMove);
        return true;
    }
}
