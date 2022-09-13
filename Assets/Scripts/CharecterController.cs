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
    public float baseGravitiyMultiplier;
    public float GravityMultiplier;

    [Header("Checks")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    
    private float lastTimeGrounded;
    private bool isGrounded;
    private float moveDirection;
    private Rigidbody2D rb;
    private float lastJumpTime;
    private bool isJumping;
    private bool jumpReleased;
    private float gravityScale;
    //[Header("Private Variables")]
    

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        setGravityScale(gravityScale);
    }
    private void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && lastTimeGrounded < CoyoteTime && !isJumping)
        {
            Jump();
        }
        if(isJumping && Input.GetButtonUp("Jump"))
        {
            onJumpUp();
        }
    }
    private void FixedUpdate()
    {
        #region Speed
        float targetSpeed = moveDirection * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        //? = if statement
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, vellocityPoweer) * Mathf.Sign(speedDiff);
        rb.AddForce(movement * Vector2.right);
        #endregion
        #region Gravity
        if (rb.velocity.y < 0)
        {
            setGravityScale(gravityScale * GravityMultiplier);
        }
        else
        {
            setGravityScale(gravityScale);
        }
        #endregion
        groundCheck();
        lastJumpTime += Time.deltaTime;
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastTimeGrounded = 0;
        lastJumpTime = 0;
        isJumping = true;
        jumpReleased = false;
    }
    private void groundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && !isJumping)
        {
            lastTimeGrounded = 0;
            isGrounded = true;
        }
        else if (!isGrounded)
        {
            lastTimeGrounded += Time.deltaTime;
        }
    }

    private void onJumpUp()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCut), ForceMode2D.Impulse);
            isJumping = false;
            lastJumpTime = 0;
        }
    }

    private void setGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
}

