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

    GameObject lineObject;
    UILineRenderer line;

    public CommandNodeDragger parentNode;

    protected abstract bool IsOppositeSlot(Component component);
    public abstract NodeConnector GetOppositePair();
    public abstract NodeInput GetInputPair();
    public abstract NodeOutput GetOutputPair();
    public abstract void SetOppositePair(NodeConnector nodeConnector);

    const int startIndex = 0;
    const int endIndex = 3;
    const int startHandle = 1;
    const int endHandle = 2;
    const float handleOffset = 100;


    public void Update()
    {
        if (dragging)
        {
            lineTarget = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - (Vector2)gameObject.transform.position;
            line.Points[endIndex] = lineTarget;
            line.Points[startHandle] = line.Points[startIndex] + new Vector2(handleOffset, 0);
            line.Points[endHandle] = line.Points[endIndex] - new Vector2(handleOffset, 0);
            line.SetAllDirty();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        NodeConnector connection = GetConnection(eventData);
        if (connection && connection is NodeInput)
        {
            Disconnect();
        }

        if (!line)
        {
            lineObject = new GameObject("Line Object", typeof(UILineRenderer));
            lineObject.transform.SetParent(gameObject.transform);
            lineObject.transform.localPosition = Vector2.zero;
            line = lineObject.GetComponent<UILineRenderer>();
            line.BezierMode = UILineRenderer.BezierType.Improved;
            line.BezierSegmentsPerCurve = 20;
            line.Points = new Vector2[4];
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        NodeConnector connection = GetConnection(eventData);
        if (connection)
        {
            SetUpConnection(connection);
            SetLinePositions();
            //line.Points[endIndex] = connection.transform.position - gameObject.transform.position;
        }
        else
        {
            DestroyLine();
        }

        dragging = false;
    }

    public void SetUpConnection(NodeConnector component)
    {
        //Set up Node connections
        SetOppositePair(component.GetComponent<NodeConnector>());
        GetOppositePair().SetOppositePair(this);

        //Set up Command connections
        if (GetOutputPair().parentNode.attachedCommand is Next)
        {
            ((Next)GetOutputPair().parentNode.attachedCommand).SetNextCommand(GetInputPair().parentNode.attachedCommand);
        }
        else if (GetOutputPair().parentNode.attachedCommand is If)
        {
            if (GetOutputPair() is NodeOutputTrue)
                ((If)GetOutputPair().parentNode.attachedCommand).SetIfTrue(GetInputPair().parentNode.attachedCommand);
            else if (GetOutputPair() is NodeOutputFalse)
                ((If)GetOutputPair().parentNode.attachedCommand).SetIfFalse(GetInputPair().parentNode.attachedCommand);
            else
                Debug.LogError("Node Output. Needs to be true or false based");
        }
    }

    public void Disconnect()
    {
        if (!GetOppositePair())
            return;

        //Disconnect Commands
        if (GetOutputPair().parentNode.attachedCommand is Next)
        {
            ((Next)GetOutputPair().parentNode.attachedCommand).SetNextCommand(null);
        }
        else if (GetOutputPair().parentNode.attachedCommand is If)
        {
            if (GetOutputPair() is NodeOutputTrue)
                ((If)GetOutputPair().parentNode.attachedCommand).SetIfTrue(null);
            else if (GetOutputPair() is NodeOutputFalse)
                ((If)GetOutputPair().parentNode.attachedCommand).SetIfFalse(null);
            else
                Debug.LogError("Node Output. Needs to be true or false based");
        }

        //Disconnect Nodes
        if (GetOppositePair())
        {
            GetOppositePair().DestroyLine();
            GetOppositePair().SetOppositePair(null);
        }
        SetOppositePair(null);
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

    public UILineRenderer GetLineRenderer()
    {
        var oppositePair = GetOppositePair();

        if (line)
            return line;
        else if (oppositePair && oppositePair.line)
            return oppositePair.line;
        else
            return null;
    }

    public void DestroyLine()
    {
        Destroy(lineObject);
    }

    public void SetLinePositions()
    {
        //AAAAAAAA I GOT IT WORKING BUT I GIVE UP
        var lineRenderer = GetLineRenderer();

        if (line)
        {
            if (GetComponent<NodeOutput>())
            {
                lineRenderer.Points[startIndex] = Vector2.zero;
                lineRenderer.Points[endIndex] = GetOppositePair().transform.position - transform.position;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = Mathf.Max(handleOffset, xDist/3);
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
            else
            {
                lineRenderer.Points[startIndex] = GetOppositePair().transform.position - transform.position;
                lineRenderer.Points[endIndex] = Vector2.zero;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = Mathf.Max(handleOffset, xDist/3);
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
        }
        else
        {
            if (GetComponent<NodeOutput>())
            {
                lineRenderer.Points[startIndex] = transform.position - GetOppositePair().transform.position;
                lineRenderer.Points[endIndex] = Vector2.zero;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = Mathf.Max(handleOffset, xDist/3);
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
            else
            {
                lineRenderer.Points[startIndex] = Vector2.zero;
                lineRenderer.Points[endIndex] = transform.position - GetOppositePair().transform.position;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = Mathf.Max(handleOffset, xDist/3);
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
        }

        lineRenderer.SetAllDirty();
    }

}
