using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;

public abstract class CommandNodeDragger : EventTrigger
{
    public abstract Type AssociatedType();
    public Command attachedCommand;
    private bool dragging;
    NodeConnector[] nodeConnectors;
    private void Awake()
    {
        nodeConnectors = GetComponentsInChildren<NodeConnector>();
        foreach (var nodeConnector in nodeConnectors)
        {
            nodeConnector.parentNode = this;
        }
    }

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            foreach (var nodeConnector in nodeConnectors)
            {
                UILineRenderer lineRenderer = nodeConnector.GetLineRenderer();
                if(lineRenderer)
                    nodeConnector.SetLinePositions();
                //lineRenderer.Points[1] = nodeConnector.transform.position;
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

}
