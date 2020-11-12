using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Next
{
    public Vector2 directionToMove;

    public Move() { }
    public Move(Vector2 direction)
    {
        directionToMove = direction;
    }

    public override void Activate()
    {
        parentComputer.transform.Translate(directionToMove);
    }

    public void SetDirection(Vector2 direction)
    {
        directionToMove = direction;
    }
}

public class MoveForward : Next
{
    public float distance;
    public MoveForward(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveForward(distance);
    }
}


public class MoveBack : Next
{
    public float distance;
    public MoveBack(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveBackwards(distance);
    }
}

public class MoveLeft : Next
{
    public float distance;
    public MoveLeft(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveLeft(distance);
    }
}
public class MoveRight : Next
{
    public float distance;
    public MoveRight(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveRight(distance);
    }
}
