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

    public IEnumerator LookForTag(string tag, System.Action<bool> callback, bool onlyInfront = false) //look for objects with tag for the duration of the command, uses a callback to return a value
    {
        float time = 0;
        while (time < parentComputer.tickTime)
        {
            if (onlyInfront && LookForObjectWithTag(tag, true))
            {
                callback(true);
                yield break;
            }
            else if (!onlyInfront && LookForObjectWithTag(tag))
            {
                callback(true);
                yield break;
            }
            time += Time.deltaTime;
            yield return null;
        }
        callback(false);
    }

    bool LookForObjectWithTag(string tag, bool onlyInfront = false)
    {
        List<GameObject> objects;
        if (!onlyInfront)
            objects = ObjectsInView();
        else
            objects = ObjectsInfront();


        foreach (var item in objects)
        {
            if (item.tag == tag)
            {
                return true;
            }
        }

        return false;
    }

    List<GameObject> ObjectsInView() //get all objects that are in the sensor's view
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

    List<GameObject> ObjectsInfront() //get all objects that are one unit infront of sensor
    {
        List<Collider2D> colliders;

        colliders = new List<Collider2D>();
        var rays = Physics2D.RaycastAll(transform.position + (transform.right * 0.5f), transform.right, 0.5f);
        foreach (var item in rays)
        {
            colliders.Add(item.collider);
        }
        //colliders.AddRange(Physics2D.OverlapBoxAll(transform.position + transform.right, new Vector2(.9f, .9f), transform.localEulerAngles.z));

        List<GameObject> objects = new List<GameObject>();
        foreach (var collider in colliders)
        {
            objects.Add(collider.gameObject);
        }

        return objects;
    }

    public bool WithinFOVAngle(Vector2 position)
    {
        Vector2 forward = transform.right;
        Vector2 directionTo = (position - (Vector2)transform.position);
        float angle = kMath.AngleBetween(forward, directionTo);
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
        Vector3 startDir = new Vector3(Mathf.Cos(((visionAngle / 2) + transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin(((visionAngle / 2) + transform.eulerAngles.z) * Mathf.Deg2Rad), 0);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, startDir, visionAngle, visionDistance);

        Handles.color = new Color(0, 0, 0, 1f);
        Handles.DrawLine(transform.position + (transform.right * 0.5f), (transform.position + (transform.right * 0.5f)) + transform.right * 0.5f);
    }
#endif
}
