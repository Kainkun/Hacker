using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class CommandNode : EventTrigger
{
    public abstract Type AssociatedType();
    public Command attachedCommand;
    private bool dragging;
    NodeConnector[] nodeConnectors;
    public NodeInput nodeInput;
    public List<NodeOutput> nodeOutputs;
    Image nodeIcon;

    private void Awake()
    {
        nodeIcon = GetComponent<Image>();
        nodeConnectors = GetComponentsInChildren<NodeConnector>();
        foreach (var nodeConnector in nodeConnectors)
        {
            if (nodeConnector is NodeInput)
                nodeInput = (NodeInput)nodeConnector;
            else
                nodeOutputs.Add((NodeOutput)nodeConnector);

            nodeConnector.parentNode = this;
        }
    }

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            attachedCommand.connectedNodePosition = transform.localPosition;
            foreach (var nodeConnector in nodeConnectors)
            {
                UILineRenderer lineRenderer = nodeConnector.GetLineRenderer();
                if (lineRenderer)
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

        RectTransform trashBinRect = HackingUISystem.instance.trashBin.transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(trashBinRect, Input.mousePosition))
            DeleteNodeAndCommand();
    }

    public void ActivateIcon(bool activate = true)
    {
        nodeIcon.color = activate ? Color.green : Color.white;
    }

    public void DisplayConnections()
    {
        foreach (var nodeConnection in nodeOutputs)
        {
            nodeConnection.DisplayConnection();
        }
    }

    public void DeleteNodeAndCommand()
    {
        foreach (var nodeConnector in nodeConnectors)
            nodeConnector.Disconnect();

        HackingUISystem.instance.GetCurrentlyEditingProgram().RemoveCommand(attachedCommand);
        HackingUISystem.instance.nodes.Remove(this);
        Destroy(gameObject);
    }

    public void HideNode()
    {
        foreach (var nodeConnector in nodeConnectors)
            nodeConnector.Hide();

        HackingUISystem.instance.nodes.Remove(this);
        Destroy(gameObject);
    }

}
