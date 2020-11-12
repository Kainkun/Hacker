using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeInput : NodeConnector
{
    public NodeOutput previousNodeInput;
    public override NodeConnector GetOppositePair()
    {
        return previousNodeInput;
    }

    public override void SetOppositePair(NodeConnector nodeConnector)
    {
        if(nodeConnector == null)
            previousNodeInput = null;
        else
            previousNodeInput = nodeConnector.GetComponent<NodeOutput>();
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if (component is NodeOutput)
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
