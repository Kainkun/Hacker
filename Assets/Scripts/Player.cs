using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    Rigidbody2D rb;
    public float moveSpeed = 1;
    public GameObject HackingUICanvas;
    bool disableInput;
    Vector2 movementInput;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {

        if (!disableInput)
            GetInput();
    }

    public void DisableInput()
    {
        disableInput = true;
        movementInput = Vector2.zero;
    }

    public void EnableInput()
    {
        disableInput = false;
    }

    void GetInput()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!HackingUICanvas.activeSelf)
            {
                var closestComputer = GetClosestComputerInRange();
                if (closestComputer != null)
                {
                    DisableInput();
                    HackingUICanvas.SetActive(true);
                    HackingUISystem.instance.EditComputer(closestComputer);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed);

    }

    Computer GetClosestComputerInRange()
    {
        var computers = FindObjectsOfType<Computer>();
        Computer closestComputer = null;
        float shortest = Mathf.Infinity;
        foreach (var computer in computers)
        {
            var dist = Vector3.Distance(transform.position, computer.transform.position);
            if (dist < 2 && dist < shortest)
            {
                closestComputer = computer;
                shortest = dist;
            }
        }

        return closestComputer;
    }
}
