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
    public float dashTime;
    private Vector2 dashingDir;


    [Header("Private")]
    [SerializeField] private float lastTimeGrounded;
    [SerializeField] public bool isGrounded;
    [SerializeField] private float moveDirection;
    private Rigidbody2D rb;
    [SerializeField] private bool isJumping;
    private bool isDashing;
    private bool isActuallyDashing;
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
    public bool FacingRight;

    //[Header("Private Variables")]


    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        setGravityScale(gravityScale);
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        cameraController = Camera.GetComponent<CameraController>();
        canDash = true;
    }
    private void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection > 0)
        {
            FacingRight = true;
        }
        else if (moveDirection < 0)
        {
            FacingRight = false;
        }
        #region Dashing
        var dashInput = Input.GetButtonDown("Dash");
        var dashInputUp = Input.GetButtonUp("Dash");

        if (dashInput && canDash)
        {
            canDash = false;
            isDashing = true;
            beginDashSlow();

        }
        if (isDashing && dashInputUp || slowDownLength <= 0 && isDashing)
        {
            slowDownLength = 2;
            isDashing = false;
            isActuallyDashing = true;
            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(dashingDir == Vector2.zero)
            {
                Debug.Log("dash no direction");
            }
            timeManager.ResetTime();
            cameraController.FinishedDash();
            isDashing = false;
            slowDownLength = 2f;
            StartCoroutine(StopDashing());
            //add dash stop
        }
        if(slowDownLength >= 0 && Input.GetKey(KeyCode.LeftShift))
        {
            slowDownLength -= Time.unscaledDeltaTime;
        }

        if (isActuallyDashing)
        {
            rb.velocity = dashingDir.normalized * dashSpeed;
            return;
        }
        #endregion

        #region Jump
        if (Input.GetButtonDown("Jump") && lastTimeGrounded < CoyoteTime && !isJumping && jumpBufferTemp <= 0 && !isActuallyDashing)
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
        #endregion
        #region directionFacing
        if (Input.GetAxis("Horizontal") <= 1 && (Input.GetAxis("Horizontal")) > 0)
        {
            facingForward = true;
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (Input.GetAxis("Horizontal") >= -1 && (Input.GetAxis("Horizontal")) < 0)
        {
            facingForward = false;
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        #endregion

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


    private IEnumerator StopDashing()
    {
        yield return new WaitForSecondsRealtime(dashTime);
        isDashing = false;
        isActuallyDashing = false;
        onJumpUp();
    }
}

