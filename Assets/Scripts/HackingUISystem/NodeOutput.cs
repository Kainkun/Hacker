using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOutput : NodeConnector
{
    public NodeInput nextNodeInput;

    protected override void Disconnect()
    {
        if(nextNodeInput)
            nextNodeInput.previousNodeInput = null;
        nextNodeInput = null;
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if(component.GetType() == typeof(NodeInput))
        {
            return true;
        }

        return false;
    }

    protected override void SetUpConnection(NodeConnector component)
    {
        nextNodeInput = component.GetComponent<NodeInput>();
        nextNodeInput.previousNodeInput = this;
    }
}
