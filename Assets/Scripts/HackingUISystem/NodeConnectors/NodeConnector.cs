using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public abstract class NodeConnector : EventTrigger
{
    private bool dragging;
    Vector2 lineTarget;

    UILineRenderer lineRenderer;

    public CommandNode parentNode;

    protected abstract bool IsOppositeSlot(Component component);
    public abstract bool HasOppositePair();
    public abstract NodeInput GetInputConnector();
    public abstract List<NodeOutput> GetOutputConnectors();

    const int startIndex = 0;
    const int endIndex = 3;
    const int startHandle = 1;
    const int endHandle = 2;


    public void Update()
    {
        if (dragging)
        {
            DrawLineToMouse();
        }
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        if (this is NodeOutput)
        {
            Disconnect();
        }

        CreateMouseLine();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        DestroyLine();

        NodeConnector connection = GetConnection(eventData);
        if (connection)
        {
            if (this is NodeInput)
            {
                if (((NodeOutput)connection).GetOppositePair() != this)
                {
                    connection.Disconnect();

                    SetUpConnection(connection);
                    CreateConnectedLine((NodeOutput)connection);
                    DrawConnectedLines();
                }
            }
            else// this is NodeOutput
            {
                bool thisNotInList = true;
                foreach (var item in ((NodeInput)connection).GetOppositePairs())
                {
                    if (item == this)
                        thisNotInList = false;
                }
                if (thisNotInList)
                {
                    SetUpConnection(connection);
                    CreateConnectedLine((NodeOutput)this);
                    DrawConnectedLines();
                }
            }

        }
        dragging = false;
    }

    public void SetUpConnection(NodeConnector component)
    {
        NodeOutput newNodeOutput = null;

        //Set up Node connections
        if (this is NodeInput)
        {
            ((NodeInput)this).AddOppositePair(component);
            newNodeOutput = (NodeOutput)component;
            newNodeOutput.SetOppositePair(this);
        }
        else// this is NodeOutput
        {
            newNodeOutput = (NodeOutput)this;
            newNodeOutput.SetOppositePair(component);
            ((NodeInput)component).AddOppositePair(this);
        }

        //Set up Command connections
        if (newNodeOutput.parentNode.attachedCommand is Next)
        {
            ((Next)newNodeOutput.parentNode.attachedCommand).SetNextCommand(GetInputConnector().parentNode.attachedCommand);
        }
        else if (newNodeOutput.parentNode.attachedCommand is If)
        {
            if (newNodeOutput is NodeOutputTrue)
                ((If)newNodeOutput.parentNode.attachedCommand).SetIfTrue(GetInputConnector().parentNode.attachedCommand);
            else if (newNodeOutput is NodeOutputFalse)
                ((If)newNodeOutput.parentNode.attachedCommand).SetIfFalse(GetInputConnector().parentNode.attachedCommand);
            else
                Debug.LogError("Node Output. Needs to be true or false based");
        }
    }


    public void Disconnect()
    {
        if (!HasOppositePair())
            return;


        if (this is NodeInput)
        {
            foreach (var outputConnector in GetOutputConnectors())
            {
                DisconnectSingle(outputConnector);
            }
        }
        else// this is NodeOutput
        {
            DisconnectSingle((NodeOutput)this);
        }


        HideLines();
    }

    void DisconnectSingle(NodeOutput nodeOutput)
    {
        if (nodeOutput.parentNode.attachedCommand is Next)
        {
            ((Next)nodeOutput.parentNode.attachedCommand).SetNextCommand(null);
        }
        else if (nodeOutput.parentNode.attachedCommand is If)
        {
            if (nodeOutput is NodeOutputTrue)
                ((If)nodeOutput.parentNode.attachedCommand).SetIfTrue(null);
            else if (nodeOutput is NodeOutputFalse)
                ((If)nodeOutput.parentNode.attachedCommand).SetIfFalse(null);
            else
                Debug.LogError("Node Output. Needs to be true or false based");
        }
    }

    public void HideLines()
    {
        if (this is NodeInput)
        {
            if (GetOutputConnectors()?.Count > 0)
            {
                foreach (var outputConnector in GetOutputConnectors())
                {
                    outputConnector.DestroyLine();
                    outputConnector.SetOppositePair(null);
                }
            }
            ((NodeInput)this).RemoveAllOppositePairs();
        }
        else// this is NodeOutput
        {
            DestroyLine();
            ((NodeOutput)this).GetOppositePair()?.RemoveOppositePair(this);
            ((NodeOutput)this).SetOppositePair(null);
        }
    }

    NodeConnector GetConnection(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            var nc = item.GetComponent<NodeConnector>();
            if (nc && IsOppositeSlot(nc))
            {
                return nc;
            }
        }
        return null;
    }

    public List<UILineRenderer> GetConnectedLineRenderers()
    {
        var lineRenderers = new List<UILineRenderer>();
        if (this is NodeInput)
        {
            foreach (var nodeOutput in GetOutputConnectors())
            {
                if (nodeOutput.lineRenderer)
                    lineRenderers.Add(nodeOutput.lineRenderer);
            }
        }
        else// this is NodeOutput
        {
            if (lineRenderer)
                lineRenderers.Add(lineRenderer);
        }
        return lineRenderers;
    }

    void CreateMouseLine()
    {
        var lineObject = new GameObject("Mouse Line Object", typeof(UILineRenderer));
        lineObject.transform.SetParent(transform);
        lineObject.transform.localPosition = Vector2.zero;
        lineRenderer = lineObject.GetComponent<UILineRenderer>();

        lineRenderer.rectTransform.sizeDelta = new Vector2(12, 12);
        lineRenderer.BezierMode = UILineRenderer.BezierType.Improved;
        lineRenderer.BezierSegmentsPerCurve = 20;
        lineRenderer.Points = new Vector2[4];
    }

    void DrawLineToMouse()
    {
        lineTarget = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - (Vector2)gameObject.transform.position;
        lineRenderer.Points[endIndex] = lineTarget;
        var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
        var tempHandleOffset = xDist / 3;
        lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
        lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);

        lineRenderer.SetAllDirty();
    }

    protected void CreateConnectedLine(NodeOutput node)
    {
        var lineObject = new GameObject("Connected Line Object", typeof(UILineRenderer));
        lineObject.transform.SetParent(node.transform);
        lineObject.transform.localPosition = Vector2.zero;
        node.lineRenderer = lineObject.GetComponent<UILineRenderer>();

        node.lineRenderer.rectTransform.sizeDelta = new Vector2(12, 12);
        node.lineRenderer.BezierMode = UILineRenderer.BezierType.Improved;
        node.lineRenderer.BezierSegmentsPerCurve = 20;
        node.lineRenderer.Points = new Vector2[4];
    }

    public void DrawConnectedLines()
    {
        foreach (var lineRenderer in GetConnectedLineRenderers())
        {
            lineRenderer.Points[startIndex] = Vector2.zero;
            lineRenderer.Points[endIndex] = GetInputConnector().transform.position - lineRenderer.transform.position;

            var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
            var tempHandleOffset = xDist / 3;
            lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
            lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);

            lineRenderer.SetAllDirty();
        }
    }

    public void DestroyLine()
    {
        if (lineRenderer)
        {
            Destroy(lineRenderer.gameObject);
            lineRenderer = null;
        }
    }

    // public void SetLinePositions()
    // {
    //     //AAAAAAAA I GOT IT WORKING BUT I GIVE UP
    //     var lineRenderer = GetLineRenderer();

    //     if (this.lineRenderer)
    //     {
    //         if (GetComponent<NodeOutput>())
    //         {
    //             lineRenderer.Points[startIndex] = Vector2.zero;
    //             lineRenderer.Points[endIndex] = GetOppositePair().transform.position - transform.position;

    //             var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
    //             var tempHandleOffset = xDist / 3;
    //             lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
    //             lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
    //         }
    //         else
    //         {
    //             lineRenderer.Points[startIndex] = GetOppositePair().transform.position - transform.position;
    //             lineRenderer.Points[endIndex] = Vector2.zero;

    //             var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
    //             var tempHandleOffset = xDist / 3;
    //             lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
    //             lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
    //         }
    //     }
    //     else
    //     {
    //         if (GetComponent<NodeOutput>())
    //         {
    //             lineRenderer.Points[startIndex] = transform.position - GetOppositePair().transform.position;
    //             lineRenderer.Points[endIndex] = Vector2.zero;

    //             var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
    //             var tempHandleOffset = xDist / 3;
    //             lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
    //             lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
    //         }
    //         else
    //         {
    //             lineRenderer.Points[startIndex] = Vector2.zero;
    //             lineRenderer.Points[endIndex] = transform.position - GetOppositePair().transform.position;

    //             var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
    //             var tempHandleOffset = xDist / 3;
    //             lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
    //             lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
    //         }
    //     }

    //     lineRenderer.SetAllDirty();
    // }

}
