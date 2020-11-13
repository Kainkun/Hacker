using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Next
{
    [SerializeReference]
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

public abstract class MoveDirection : Next
{
    [SerializeReference]
    protected float distance;
    public abstract void SetDistance(float dist);

}

public class MoveForward : MoveDirection
{
    
    public MoveForward(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveForward(distance);
    }

    public override void SetDistance(float dist)
    {
        distance = dist;
    }
}


public class MoveBack : MoveDirection
{
    public MoveBack(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveBackwards(distance);
    }

    public override void SetDistance(float dist)
    {
        distance = dist;
    }
}

public class MoveLeft : MoveDirection
{
    public MoveLeft(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveLeft(distance);
    }

    public override void SetDistance(float dist)
    {
        distance = dist;
    }
}
public class MoveRight : MoveDirection
{
    public MoveRight(float distance = 1)
    {
        this.distance = distance;
    }

    public override void Activate()
    {
        parentComputer.movement.MoveRight(distance);
    }

    public override void SetDistance(float dist)
    {
        distance = dist;
    }
}
