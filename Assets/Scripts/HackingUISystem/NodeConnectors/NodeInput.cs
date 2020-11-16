using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeInput : NodeConnector
{
    public List<NodeOutput> previousNodeOutputs;

    public override bool HasOppositePair()
    {
        if (previousNodeOutputs?.Count > 0)
            return true;

        return false;
    }

    public List<NodeOutput> GetOppositePairs()
    {
        return previousNodeOutputs;
    }

    public void AddOppositePair(NodeConnector nodeConnector)
    {
        if (nodeConnector == null)
            return;
        else
            previousNodeOutputs.Add(nodeConnector.GetComponent<NodeOutput>());
    }

    public void RemoveOppositePair(NodeConnector nodeConnector)
    {
        previousNodeOutputs.Remove((NodeOutput)nodeConnector);
    }

    public void RemoveAllOppositePairs()
    {
        previousNodeOutputs = null;
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if (component is NodeOutput)
        {
            return true;
        }

        return false;
    }

    public override NodeInput GetInputConnector()
    {
        return this;
    }

    public override List<NodeOutput> GetOutputConnectors()
    {
        return previousNodeOutputs;
    }

}
