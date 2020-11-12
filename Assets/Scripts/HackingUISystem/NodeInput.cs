using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInput : NodeConnector
{
    public NodeOutput previousNodeInput;

    protected override bool IsOppositeSlot(Component component)
    {
        if(component.GetType() == typeof(NodeOutput))
        {
            return true;
        }

        return false;
    }
}
