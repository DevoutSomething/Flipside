using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharecterController : MonoBehaviour
{
    [Header("Speed")]
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float velocityPoweer;
    public float friction;
    public bool facingForward;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCut;
    public float CoyoteTime;
    public float jumpBuffer;
    public float gravityScale;
    public float GravityMultiplier;
    public float jumpBufferTime;
    public bool canJump;
    [Header("Checks")]
    public Transform groundCheckPoint;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;


    [Header("Dash")]
    public float dashSpeed;
    public float slowDownLength = 2;
    [Range(0.0f, 1.0f)]
    public float dashDiagnalMod;

    [Header("Private")]
    [SerializeField] private float lastTimeGrounded;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float moveDirection;
    private Rigidbody2D rb;
    [SerializeField] private bool isJumping;
    private bool isDashing;
    private bool canDash;
    private float directionX;
    private float directionY;
    private CameraController cameraController;

    [Header("set to zero")]
    public float rotationOnJump;

    private BoxCollider2D boxCollider2d;
    private float jumpBufferTemp;

    public TimeManager timeManager;
    public GameObject Camera;
    //[Header("Private Variables")]



    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        setGravityScale(gravityScale);
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        cameraController = Camera.GetComponent<CameraController>();
    }
    private void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        #region Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            canDash = false;
            isDashing = true;
            beginDashSlow();
        }
        if (isDashing && Input.GetKeyUp(KeyCode.LeftShift))
        {
            isDashing = false;
            Dash(directionX, directionY);
            slowDownLength = 2;
        }
        if(slowDownLength >= 0 && Input.GetKey(KeyCode.LeftShift))
        {
            slowDownLength -= Time.unscaledDeltaTime;
        }
        if (slowDownLength <= 0 && isDashing)
        {
            Dash(directionX, directionY);

        }
        #endregion
        if (Input.GetButtonDown("Jump") && lastTimeGrounded < CoyoteTime && !isJumping && jumpBufferTemp <= 0)
        {
            isJumping = true;
            Jump();
            Debug.Log("jumped");
        }
        else if(Input.GetButtonDown("Jump") && !isGrounded && !isJumping)
        {
            jumpBufferTemp = jumpBufferTime;
        }
        if (jumpBufferTemp > 0 && !isJumping)
        {
            jumpBufferTemp -= Time.deltaTime;
            if (isGrounded && lastTimeGrounded < CoyoteTime && !isJumping)
            {
                jumpBufferTemp = 0;
                Jump();
                Debug.Log("Jumped with buffer");
            }
        }
        if (isJumping && Input.GetButtonUp("Jump") && !isDashing)
        {
            onJumpUp();
        }
        if(Input.GetAxis("Horizontal") <= 1 && (Input.GetAxis("Horizontal")) > 0)
        {
            facingForward = true;
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (Input.GetAxis("Horizontal") >= -1 && (Input.GetAxis("Horizontal")) < 0)
        {
            facingForward = false;
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        directionX = Input.GetAxisRaw("Horizontal");
        directionY = Input.GetAxisRaw("Vertical");

    }
    private void FixedUpdate()
    {
       
        #region Speed
        float targetSpeed = moveDirection * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        //? = if statement
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, velocityPoweer) * Mathf.Sign(speedDiff);
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
        

        groundCheck();
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        jumpBufferTemp = 0;
        #region rotate on jump 
        /*
        if (rb.velocity.x > 0)
        {
            rb.AddTorque(rotationOnJump * -1, ForceMode2D.Impulse);
        }
        if (rb.velocity.x < 0)
        {
            rb.AddTorque(rotationOnJump, ForceMode2D.Impulse);
        }
        */
        #endregion
    }
    private void groundCheck()
    {
       RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.05f, groundLayer);
      if (raycastHit.collider != null)
      {
            lastTimeGrounded = 0;
            isGrounded = true;
            canDash = true;
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
    private void beginDashSlow()
    {
        Debug.Log("TimeSlow");
        cameraController.IsDashing();
        timeManager.SlowDownTime();
    }
    private void Dash(float dirx, float diry)
    {
        timeManager.ResetTime();
        cameraController.FinishedDash();
        isDashing = false;
        slowDownLength = 2f;
        #region dashDirection
        //dash in direction facing
        if (dirx == 0 && diry == 0)
        {
            Debug.Log("dash no direction");
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //dash left
        if (dirx < 0 && diry == 0)
        {
            Debug.Log("dash left");
            rb.velocity = new Vector2(0,0);
            rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash right
        if (dirx > 0 && diry == 0)
        {
            Debug.Log("dash right");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash up
        if(dirx == 0 && diry > 0)
        {
            Debug.Log("dash up");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.up * dashSpeed, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash down
        if(dirx == 0 && diry < 0)
        {
            Debug.Log("dash down");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.down * dashSpeed, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash left down
        if (dirx < 0 && diry < 0)
        {
            Debug.Log("dash down left");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.down * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            rb.AddForce(Vector2.left * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash right down
        if (dirx > 0 && diry < 0)
        {
            Debug.Log("dash down right");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.down * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash left up
        if (dirx < 0 && diry > 0)
        {
            Debug.Log("dash up left");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.up * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            rb.AddForce(Vector2.left * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        //dash right up
        if (dirx > 0 && diry > 0)
        {
            Debug.Log("dash up right");
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.up * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * dashSpeed * dashDiagnalMod, ForceMode2D.Impulse);
            setGravityScale(2);
        }
        #endregion
    }

}

