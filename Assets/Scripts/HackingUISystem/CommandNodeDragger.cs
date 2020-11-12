using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CommandNodeDragger : EventTrigger
{

    private bool dragging;
    NodeConnector[] nodeConnectors;
    private void Awake()
    {
        nodeConnectors = GetComponentsInChildren<NodeConnector>();
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
