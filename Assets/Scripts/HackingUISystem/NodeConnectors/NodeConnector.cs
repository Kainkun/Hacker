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
    UILineRenderer lineRenderer;

    public CommandNode parentNode;

    protected abstract bool IsOppositeSlot(Component component);
    public abstract NodeConnector GetOppositePair();
    public abstract NodeInput GetInputPair();
    public abstract NodeOutput GetOutputPair();
    public abstract void SetOppositePair(NodeConnector nodeConnector);

    const int startIndex = 0;
    const int endIndex = 3;
    const int startHandle = 1;
    const int endHandle = 2;


    public void Update()
    {
        if (dragging)
        {
            lineTarget = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - (Vector2)gameObject.transform.position;
            lineRenderer.Points[endIndex] = lineTarget;
            var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
            var tempHandleOffset = xDist / 3;
            lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
            lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);

            lineRenderer.SetAllDirty();
        }
    }

    protected void CreateLine()
    {
        lineObject = new GameObject("Line Object", typeof(UILineRenderer));
        lineObject.transform.SetParent(gameObject.transform);
        lineObject.transform.localPosition = Vector2.zero;
        lineRenderer = lineObject.GetComponent<UILineRenderer>();
        lineRenderer.rectTransform.sizeDelta = new Vector2(12,12);
        lineRenderer.BezierMode = UILineRenderer.BezierType.Improved;
        lineRenderer.BezierSegmentsPerCurve = 20;
        lineRenderer.Points = new Vector2[4];
    }

    protected void DrawLine()
    {
        if (GetOppositePair() == null)
            return;

        lineRenderer.Points[endIndex] = GetOppositePair().transform.position - transform.position;
        var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
        var tempHandleOffset = xDist / 3;
        lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
        lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);

        lineRenderer.SetAllDirty();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        if (this is NodeOutput)
        {
            Disconnect();
        }

        if (!lineRenderer)
        {
            CreateLine();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        NodeConnector connection = GetConnection(eventData);

        if (connection)
        {
            if (connection.GetOppositePair() != this)
            {
                if (connection is NodeOutput)
                {
                    connection.Disconnect();
                }

                SetUpConnection(connection);
                SetLinePositions();
            }
            else
            {
                DestroyLine();
            }
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

        Hide();
    }

    public void Hide()
    {
        if (!GetOppositePair())
            return;

        if (GetOppositePair())
        {
            DestroyLine();
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

        if (lineRenderer)
            return lineRenderer;
        else if (oppositePair && oppositePair.lineRenderer)
            return oppositePair.lineRenderer;
        else
            return null;
    }

    public void DestroyLine()
    {
        lineRenderer = null;
        Destroy(lineObject);
    }

    public void SetLinePositions()
    {
        //AAAAAAAA I GOT IT WORKING BUT I GIVE UP
        var lineRenderer = GetLineRenderer();

        if (this.lineRenderer)
        {
            if (GetComponent<NodeOutput>())
            {
                lineRenderer.Points[startIndex] = Vector2.zero;
                lineRenderer.Points[endIndex] = GetOppositePair().transform.position - transform.position;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = xDist / 3;
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
            else
            {
                lineRenderer.Points[startIndex] = GetOppositePair().transform.position - transform.position;
                lineRenderer.Points[endIndex] = Vector2.zero;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = xDist / 3;
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
                var tempHandleOffset = xDist / 3;
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
            else
            {
                lineRenderer.Points[startIndex] = Vector2.zero;
                lineRenderer.Points[endIndex] = transform.position - GetOppositePair().transform.position;

                var xDist = lineRenderer.Points[endIndex].x - lineRenderer.Points[startIndex].x;
                var tempHandleOffset = xDist / 3;
                lineRenderer.Points[startHandle] = lineRenderer.Points[startIndex] + new Vector2(tempHandleOffset, 0);
                lineRenderer.Points[endHandle] = lineRenderer.Points[endIndex] - new Vector2(tempHandleOffset, 0);
            }
        }

        lineRenderer.SetAllDirty();
    }

}
