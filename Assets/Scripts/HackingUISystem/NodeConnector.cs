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
    protected abstract void Disconnect();
    public abstract NodeConnector GetOppositePair();
    public abstract NodeInput GetInputPair();
    public abstract NodeOutput GetOutputPair();
    public abstract void SetOppositePair(NodeConnector nodeConnector);


    public void Update()
    {
        if (dragging)
        {
            lineTarget = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - (Vector2)gameObject.transform.position;
            line.Points[1] = lineTarget;
            line.SetAllDirty();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        Disconnect();

        if (!line)
        {
            lineObject = new GameObject("Line Object", typeof(UILineRenderer));
            lineObject.transform.SetParent(gameObject.transform);
            lineObject.transform.localPosition = Vector2.zero;
            line = lineObject.GetComponent<UILineRenderer>();
            line.Points = new Vector2[2];
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        NodeConnector connection = GetConnection(eventData);
        if (connection)
        {
            SetUpConnection(connection);
            SetLinePositions();
            //line.Points[1] = connection.transform.position - gameObject.transform.position;
        }
        else
        {
            DestroyLine();
        }

        dragging = false;
    }

    public void SetUpConnection(NodeConnector component)
    {
        
        SetOppositePair(component.GetComponent<NodeConnector>());
        GetOppositePair().SetOppositePair(this);

        print("A");
        if(parentNode.attachedCommand is Next)
        {
            print("B");

            ((Next)GetOutputPair().parentNode.attachedCommand).SetNextCommand(GetInputPair().parentNode.attachedCommand);
        }
    }

    // public void Disconnect()
    // {
    //     if (GetOppositePair())
    //     {
    //         GetOppositePair().DestroyLine();
    //         GetOppositePair().SetOppositePair(null);
    //     }
    //     SetOppositePair(null);
    // }

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
                lineRenderer.Points[0] = Vector2.zero;
                lineRenderer.Points[1] = GetOppositePair().transform.position - transform.position;
            }
            else
            {
                lineRenderer.Points[0] = GetOppositePair().transform.position - transform.position;
                lineRenderer.Points[1] = Vector2.zero;
            }
        }
        else
        {
            if (GetComponent<NodeOutput>())
            {
                lineRenderer.Points[0] = transform.position - GetOppositePair().transform.position;
                lineRenderer.Points[1] = Vector2.zero;
            }
            else
            {
                lineRenderer.Points[0] = Vector2.zero;
                lineRenderer.Points[1] = transform.position - GetOppositePair().transform.position;
            }
        }

        lineRenderer.SetAllDirty();
    }

}
