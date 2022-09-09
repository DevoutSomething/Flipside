using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharecterController : MonoBehaviour
{
    [Header("Movement Speed")]
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float vellocityPoweer;
    public float friction;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCut;
    public float CoyoteTime;
    public float jumpBuffer;
    public float gravitiyMultiplier;

    [Header("Checks")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public SortingLayer groundLayer;

    private float lastTimeGrounded;
    private bool isGrounded;
    private float moveDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        float targetSpeed = moveDirection * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        //? = if statement
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, vellocityPoweer) * Mathf.Sign(speedDiff);
        rb.AddForce(movement * Vector2.right);
        groundCheck();

    }
    private void Jump()
    {

    }
    private void groundCheck()
    {
        //if (Physics2D)
    }
}

