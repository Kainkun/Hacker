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

    public void Turn()
    {

    }

    IEnumerator MoveLerp(Vector2 position) //Completes movement within tick time
    {
        float time = 0;
        Vector2 startPos = transform.position;
        while(time < parentComputer.tickTime)
        {
            transform.position = Vector2.Lerp(startPos, position, time / parentComputer.tickTime);
            time += Time.deltaTime;
            yield return null;
        }

    }
}
