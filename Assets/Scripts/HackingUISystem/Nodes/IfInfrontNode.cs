using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class IfInfrontNode : IfNode
{
    public override Type AssociatedType()
    {
        return typeof(IfInfront);
    }

    public void SetTag(string tag)
    {
        ((IfInfront)attachedCommand).SetTag(tag);
    }

    internal void SetInputField()
    {
        GetComponentInChildren<TMP_InputField>().text = ((IfInfront)attachedCommand).tag;
    }
}
