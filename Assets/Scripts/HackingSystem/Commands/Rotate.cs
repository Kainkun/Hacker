using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rotate : Next
{
    [SerializeReference]
    public float amountToRotate;
}

public class RotateRight : Rotate
{
    public RotateRight(float rotation = 90)
    {
        amountToRotate = rotation;
    }

    public override void Activate()
    {
        parentProgram.parentComputer.movementModule.RotateRight(amountToRotate);
    }
}

public class RotateLeft : Rotate
{
    public RotateLeft(float rotation = 90)
    {
        amountToRotate = rotation;
    }
    public override void Activate()
    {
        parentProgram.parentComputer.movementModule.RotateLeft(amountToRotate);
    }
}