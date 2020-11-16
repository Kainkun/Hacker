using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeOutput : NodeConnector
{
    public NodeInput nextNodeInput;

    public override bool HasOppositePair()
    {
        if (nextNodeInput != null)
            return true;

        return false;
    }

    public NodeInput GetOppositePair()
    {
        return nextNodeInput;
    }

    public void SetOppositePair(NodeConnector nodeConnector)
    {
        if (nodeConnector == null)
            nextNodeInput = null;
        else
            nextNodeInput = nodeConnector.GetComponent<NodeInput>();
    }

    protected override bool IsOppositeSlot(Component component)
    {
        if (component is NodeInput)
        {
            return true;
        }

        return false;
    }

    public override NodeInput GetInputConnector()
    {
        return nextNodeInput;
    }

    public override List<NodeOutput> GetOutputConnectors()
    {
        return nextNodeInput.GetOutputConnectors();
    }

    public void DisplayConnection()
    {
        if (parentNode.attachedCommand is Next)
        {
            SetOppositePair(((Next)parentNode.attachedCommand).GetNextCommand()?.connectedNode.nodeInput);
        }
        else if (parentNode.attachedCommand is If)
        {
            if (this is NodeOutputTrue)
                SetOppositePair(((If)parentNode.attachedCommand).GetIfTrueCommand()?.connectedNode.nodeInput);
            else
                SetOppositePair(((If)parentNode.attachedCommand).GetIfFalseCommand()?.connectedNode.nodeInput);
        }
        else
        {
            Debug.LogError("Bad");
        }


        if (GetOppositePair() != null)
        {
            GetOppositePair().AddOppositePair(this);
            CreateConnectedLine(this);
            DrawConnectedLines();
        }
    }
}
