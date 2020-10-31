using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    Rigidbody2D rb;
    public float moveSpeed = 1;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    Vector2 input;
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed);

    }
}
