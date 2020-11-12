using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeOutput : NodeConnector
{
    public NodeInput nextNodeInput;

    protected override void Disconnect()
    {
        if (nextNodeInput)
        {
            nextNodeInput.DestroyLine();
            nextNodeInput.previousNodeInput = null;
        }
        nextNodeInput = null;
    }

    public override NodeConnector GetOppositePair()
    {
        return nextNodeInput;
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
