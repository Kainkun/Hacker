using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInput : NodeConnector
{
    public NodeOutput previousNodeInput;

    protected override void Disconnect()
    {
        if (previousNodeInput)
        {
            previousNodeInput.DestroyLine();
            previousNodeInput.nextNodeInput = null;
        }
        previousNodeInput = null;
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if(component.GetType() == typeof(NodeOutput))
        {
            return true;
        }

        return false;
    }

    protected override void SetUpConnection(NodeConnector component)
    {
        previousNodeInput = component.GetComponent<NodeOutput>();
        previousNodeInput.nextNodeInput = this;
    }
}
