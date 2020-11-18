using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : ComputerModule
{
    public void Move(Vector2 direction)
    {
        MoveLerp((Vector2)transform.position + direction);
    }

    public void MoveForward(float distance = 1) => StartCoroutine(MoveLerp(transform.position + (transform.right * distance)));
    public void MoveBackwards(float distance = 1) => StartCoroutine(MoveLerp(transform.position + (-transform.right * distance)));
    public void MoveRight(float distance = 1) => StartCoroutine(MoveLerp(transform.position + (-transform.up * distance)));
    public void MoveLeft(float distance = 1) => StartCoroutine(MoveLerp(transform.position + (transform.up * distance)));
    public void RotateRight(float rotation = 90) => StartCoroutine(RotateLerp(rotation));
    public void RotateLeft(float rotation = 90) => StartCoroutine(RotateLerp(-rotation));

    public void Turn()
    {

    }

    IEnumerator MoveLerp(Vector2 position) //Completes movement within tick time
    {
        float time = 0;
        Vector2 startPos = transform.position;
        while (time < parentComputer.tickTime)
        {
            transform.position = Vector2.Lerp(startPos, position, time / parentComputer.tickTime);
            time += Time.deltaTime;
            yield return null;
        }
        //snap
        transform.position = kMath.SnapVector2(transform.position);

    }

    IEnumerator RotateLerp(float rotation) //Completes movement within tick time
    {
        float time = 0;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation;
        Vector3 endEuler = endRotation.eulerAngles;
        endEuler.z -= rotation;
        endRotation = Quaternion.Euler(endEuler);


        while (time < parentComputer.tickTime)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, endRotation, time / parentComputer.tickTime);
            time += Time.deltaTime;
            yield return null;
        }
        //snap
        Vector3 angle = transform.localEulerAngles;
        angle.z = Mathf.Round(angle.z / 90) * 90;
        transform.localEulerAngles = angle;

    }
}
