using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharecterController : MonoBehaviour
{
    [Header("Speed")]
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
    public float gravityScale;
    public float GravityMultiplier;
    public float jumpBufferTime;
    [Header("Checks")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;


    [Header("Dash")]
    public float dashSpeed;

    [Header("Private")]
    [SerializeField] private float lastTimeGrounded;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float moveDirection;
    private Rigidbody2D rb;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool hasDashed;

    [Header("set to zero")]
    public float rotationOnJump;

    private BoxCollider2D boxCollider2d;
    private float jumpBufferTemp;
    //[Header("Private Variables")]



    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        setGravityScale(gravityScale);
    }
    private void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && lastTimeGrounded < CoyoteTime && !isJumping && jumpBufferTemp <= 0)
        {
            Jump();
            Debug.Log("jumped");
        }
        /*else if(Input.GetButtonDown("Jump") && !isGrounded && !isJumping)
        {
            jumpBufferTemp = jumpBufferTime;
        }
        if (jumpBufferTemp >= 0)
        {
            jumpBufferTemp -= Time.deltaTime;
            if (isGrounded && lastTimeGrounded < CoyoteTime)
            {
                jumpBufferTemp = 0;
                Jump();
                Debug.Log("Jumped with buffer");
            }
        }*/
        if (isJumping && Input.GetButtonUp("Jump"))
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
            isJumping = false;
        }
        else
        {
            setGravityScale(gravityScale);
        }
        #endregion
        #region Dashing
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        if (!hasDashed && Input.GetMouseButtonDown(0))
        {

        }
        #endregion

        groundCheck();
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        jumpBufferTemp = 0;
        #region rotate on jump 
        if (rb.velocity.x > 0)
        {
            rb.AddTorque(rotationOnJump * -1, ForceMode2D.Impulse);
        }
        if (rb.velocity.x < 0)
        {
            rb.AddTorque(rotationOnJump, ForceMode2D.Impulse);
        }
        #endregion
    }
    private void groundCheck()
    {
       RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.05f, groundLayer);
      if (raycastHit.collider != null)
      {
            Debug.Log(raycastHit.collider);
            lastTimeGrounded = 0;
            isGrounded = true;
            hasDashed = false;
      }
        else if (!isGrounded)
        {
            //Debug.Log(raycastHit.collider);
            lastTimeGrounded += Time.deltaTime;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void onJumpUp()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCut), ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    private void setGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
    private void Dash(float dirx, float diry)
    {
        
    }
}

