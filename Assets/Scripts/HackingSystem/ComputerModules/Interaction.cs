using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : ComputerModule
{
    public Rigidbody2D holdingRb;

    public void PickupDrop()
    {
        if (!holdingRb)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (transform.right * 0.5f), transform.right, 0.5f);
            foreach (var hit in hits)
            {
                if (hit && hit.collider.GetComponent<Pickupable>())
                {
                    StartCoroutine(holdObject(hit));
                    break;
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * 0.5f), transform.right, 0.5f);
            if (hit.collider == null)
                StartCoroutine(ReleaseObject());
        }

    }

    IEnumerator holdObject(RaycastHit2D hit)
    {
        holdingRb = hit.collider.GetComponent<Rigidbody2D>();
        holdingRb.simulated = false;
        holdingRb.transform.SetParent(parentComputer.transform, true);

        float t = 0;
        Vector3 startPos = holdingRb.transform.position;
        Vector3 endPos = transform.position;
        while (t < parentComputer.tickTime - 0.05f)
        {
            holdingRb.transform.position = Vector2.Lerp(startPos, endPos, t / parentComputer.tickTime);
            holdingRb.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0.5f, 0.5f, 0.5f), t / parentComputer.tickTime);
            t += Time.deltaTime;
            yield return null;
        }
        holdingRb.transform.position = endPos;
        holdingRb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    IEnumerator ReleaseObject()
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.right;
        while (t < parentComputer.tickTime - 0.05f)
        {
            holdingRb.transform.position = Vector2.Lerp(startPos, endPos, t / parentComputer.tickTime);
            holdingRb.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1), t / parentComputer.tickTime);
            t += Time.deltaTime;
            yield return null;
        }
        holdingRb.transform.position = endPos;
        holdingRb.transform.localScale = new Vector3(1, 1, 1);

        holdingRb.transform.parent = null;
        holdingRb.transform.position = kMath.SnapVector2(holdingRb.transform.position);
        holdingRb.simulated = true;
        holdingRb = null;
    }
}
