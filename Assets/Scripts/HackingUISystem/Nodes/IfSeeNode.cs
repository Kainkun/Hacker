using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class IfSeeNode : IfNode
{
    public override Type AssociatedType()
    {
        return typeof(IfSee);
    }

    public void SetTag(string tag)
    {
        ((IfSee)attachedCommand).SetTag(tag);
    }

    internal void SetInputField()
    {
        GetComponentInChildren<TMP_InputField>().text = ((IfSee)attachedCommand).tag;
    }
}