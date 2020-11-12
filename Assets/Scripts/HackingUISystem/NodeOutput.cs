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

    public override void SetOppositePair(NodeConnector nodeConnector)
    {
        nextNodeInput = nodeConnector.GetComponent<NodeInput>();
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if (component.GetType() == typeof(NodeInput))
        {
            return true;
        }

        return false;
    }

    public override NodeInput GetInputPair()
    {
        return (NodeInput)GetOppositePair();
    }

    public override NodeOutput GetOutputPair()
    {
        return this;
    }
}
