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

    public Command parentCommand;

    protected abstract bool IsOppositeSlot(Component component);
    protected abstract void SetUpConnection(NodeConnector component);
    protected abstract void Disconnect();


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
            line.Points[1] = connection.transform.position - gameObject.transform.position;
            line.SetAllDirty();
            SetUpConnection(connection);
        }
        else
        {
            Destroy(lineObject);
        }

        dragging = false;
    }

    NodeConnector GetConnection(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            var nc = item.GetComponent<NodeConnector>();
            if (nc && IsOppositeSlot(nc))
            {
                print("YES");
                return nc;
            }
        }
        return null;
    }

}
