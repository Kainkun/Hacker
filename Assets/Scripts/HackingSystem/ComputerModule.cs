using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerModule : MonoBehaviour
{
    [HideInInspector]
    public Computer parentComputer;
    public void SetParentComputer(Computer computer)
    {
        parentComputer = computer;
    }
}
