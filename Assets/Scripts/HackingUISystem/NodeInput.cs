using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

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

    public override NodeConnector GetOppositePair()
    {
        return previousNodeInput;
    }

    public override void SetOppositePair(NodeConnector nodeConnector)
    {
        previousNodeInput = nodeConnector.GetComponent<NodeOutput>();
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if (component.GetType() == typeof(NodeOutput))
        {
            return true;
        }

        return false;
    }

    public override NodeInput GetInputPair()
    {
        return this;
    }

    public override NodeOutput GetOutputPair()
    {
        return (NodeOutput)GetOppositePair();
    }
}
