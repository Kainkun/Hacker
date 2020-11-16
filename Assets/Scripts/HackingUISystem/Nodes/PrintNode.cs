using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PrintNode : CommandNode
{
    public override Type AssociatedType()
    {
        return typeof(Print);
    }

    public void SetPrintString(string str)
    {
        ((Print)attachedCommand).SetPrintString(str);
    }

    public void SetInputField()
    {
        GetComponentInChildren<TMP_InputField>().text = ((Print)attachedCommand).stringToPrint;
    }
}
