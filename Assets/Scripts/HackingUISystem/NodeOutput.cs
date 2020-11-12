using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOutput : NodeConnector
{
    public NodeInput nextNodeInput;

    protected override bool IsOppositeSlot(Component component)
    {
        if(component.GetType() == typeof(NodeInput))
        {
            return true;
        }

        return false;
    }
}
