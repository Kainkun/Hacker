using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Sensors : ComputerModule
{
    public float visionDistance = 3;
    public float visionAngle = 90;

    public List<GameObject> ObjectsInView()
    {
        List<Collider2D> colliders;

        colliders = new List<Collider2D>();
        colliders.AddRange(Physics2D.OverlapCircleAll(transform.position, visionDistance));
        colliders.Remove(GetComponent<Collider2D>());


        List<GameObject> objectsInView = new List<GameObject>();
        foreach (var collider in colliders)
        {
            if (WithinFOVAngle(collider.transform.position))
                objectsInView.Add(collider.gameObject);
        }

        return objectsInView;
    }

    public bool WithinFOVAngle(Vector2 position)
    {
        Vector2 forward = transform.right;
        Vector2 directionTo = (position - (Vector2)transform.position);
        float angle = kMath.AngleBetween(forward, directionTo);
        //print(angle);
        if (angle <= visionAngle / 2)
            return true;
        else
            return false;
    }

    public void Infront()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = new Color(1f, 0.2f, 0.2f, 0.2f);
        Vector3 startDir = new Vector3(Mathf.Cos(((visionAngle / 2)  + transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin(((visionAngle / 2)  + transform.eulerAngles.z) * Mathf.Deg2Rad), 0);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, startDir, visionAngle, visionDistance);
    }
#endif
}
