using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
