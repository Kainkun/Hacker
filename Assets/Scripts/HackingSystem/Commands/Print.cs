using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : Command
{
    public string stringToPrint;

    public Print(string str)
    {
        stringToPrint = str;
    }

    public override bool Activate()
    {
        Debug.Log(stringToPrint);
        return true;
    }
}
