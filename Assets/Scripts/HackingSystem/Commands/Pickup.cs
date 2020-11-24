using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : Next
{
    public PickupDrop() { }

    public override void Activate()
    {
        parentProgram.parentComputer.interactionModule.PickupDrop();
    }
}

