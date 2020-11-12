﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : Next
{
    public string stringToPrint;

    public Print() { }
    public Print(string str)
    {
        stringToPrint = str;
    }

    public override void Activate()
    {
        Debug.Log(stringToPrint);
    }

    public void SetPrintString(string str)
    {
        stringToPrint = str;
    }
}
