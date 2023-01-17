using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharecterController : MonoBehaviour
{
    private GameObject gameManager;
    public bool colideWithTilemap;
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
    public bool hasDashedAir;
    public float dashStartTime;
    private bool Side1;

    [Header("Private")]
    [SerializeField] private float lastTimeGrounded;
    [SerializeField] public bool isGrounded;
    [SerializeField] private float moveDirection;
    private bool canResetDash;
    public bool canResetJump;
    private Rigidbody2D rb;
    [SerializeField] private bool isJumping;
    private bool isDashing;
    private bool isActuallyDashing;
    public bool canDash;
    private CameraController cameraController;

    [Header("set to zero")]
    public float rotationOnJump;

    private BoxCollider2D boxCollider2d;
    private float jumpBufferTemp;

    public TimeManager timeManager;
    public GameObject Camera;
    public bool FacingRight;
    public Animator playerAnim;

    [Header("attack interface")]
    private meleeAttackManager MeleeAttackManager;
    //[Header("Private Variables")]

    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        setGravityScale(gravityScale);
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        MeleeAttackManager = gameObject.GetComponent<meleeAttackManager>();
        cameraController = Camera.GetComponent<CameraController>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();
        slowDownLength = 2f;
        hasDashedAir = false;
        canDash = true;
        canResetJump = true;
        gameManager = GameObject.Find("GameManager");
    }
    private void Update()
    {
        if (gameManager.GetComponent<GameManager>().isFlipped == false)
        {
            Side1 = true;
        }
        else
        {
            Side1 = false;
        }
        groundCheck();
        if (Side1)
        {
            #region Dashing
            var dashInput = Input.GetButtonDown("Dash");
            var dashInputUp = Input.GetButtonUp("Dash");
            if (isDashing && dashInputUp || slowDownLength <= 0 && isDashing)
            {
                hasDashedAir = true;
                slowDownLength = 2;
                isDashing = false;
                isActuallyDashing = true;
                dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (dashingDir == Vector2.zero)
                {
                    if (FacingRight)
                    {
                        dashingDir = new Vector2(1, 0);
                    }
                    else
                    {
                        dashingDir = new Vector2(-1, 0);
                    }
                }
                timeManager.ResetTime();
                cameraController.FinishedDash();
                isDashing = false;
                slowDownLength = 2f;
                StartCoroutine(StopDashing());
                //add dash stop
            }
            if (isActuallyDashing)
            {
                rb.velocity = dashingDir.normalized * dashSpeed;
                return;
            }

            if (dashInput && canDash)
            {
                slowDownLength = 2f;
                canResetDash = false;
                canDash = false;
                isDashing = true;
                MeleeAttackManager.canAction = false;
                StartCoroutine(TimeBeforeSlow());
            }




            #endregion
        }
        if (MeleeAttackManager.canAction && MeleeAttackManager.canAttack)
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

            if (isGrounded && Input.GetAxis("Horizontal") != 0)
            {
                playerAnim.SetBool("Run", true);
                playerAnim.SetBool("Jump", false);
            }
            else
            {
                playerAnim.SetBool("Run", false);
            }
            if (!isGrounded && isJumping)
            {
                playerAnim.SetBool("Jump", true);
            }
            else if (isGrounded)
            {
                playerAnim.SetBool("Jump", false);
            }

            if (!isGrounded)
            {
                lastTimeGrounded += Time.deltaTime;
            }

 
            #region Jump
  
            if (Input.GetButtonDown("Jump") && lastTimeGrounded < CoyoteTime && !isJumping && jumpBufferTemp <= 0 && !isActuallyDashing && canJump || Input.GetButtonDown("Jump") && isGrounded && lastTimeGrounded < CoyoteTime && !isJumping && canJump)
            {
                canJump = false;
                isJumping = true;
                canResetJump = false;
                StartCoroutine(CanJumpReset());
                Jump();
                Debug.Log("jumped no buffer");
                
            }
            if (jumpBufferTemp > 0 && !isJumping)
            {
                jumpBufferTemp -= Time.deltaTime;
                StartCoroutine(CanJumpReset());
            }
            else if (Input.GetButtonDown("Jump") && !isGrounded && !isJumping)
            {
                jumpBufferTemp = jumpBufferTime;
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

    }
    private void FixedUpdate()
    {
        if (slowDownLength >= 0)
        {
            slowDownLength -= Time.unscaledDeltaTime;
        }
        if (MeleeAttackManager.canAction && MeleeAttackManager.canAttack)
        {
            #region Speed
            float targetSpeed = moveDirection * moveSpeed;
            float speedDiff = targetSpeed - rb.velocity.x;
            //? = if statement
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, velocityPoweer) * Mathf.Sign(speedDiff);
            rb.AddForce(movement * Vector2.right);
            #endregion

        }
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
            if (canResetJump)
            {
                canJump = true;
            }
            if (canResetDash)
            {
                canDash = true;
            }
        }
        else if (!isGrounded)
        {
            //Debug.Log(raycastHit.collider);
            lastTimeGrounded += Time.deltaTime;
            hasDashedAir = false;
        }
        else
        {
            isGrounded = false;
        }
    
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (colideWithTilemap)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                lastTimeGrounded = 0;
                isGrounded = true;
                canDash = true;
            }
        }
        
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (colideWithTilemap)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
            {
                lastTimeGrounded = 0;
                isGrounded = false;
            }
        }
    }

    public void onJumpUp()
    {
        if (rb.velocity.y > 0 && !hasDashedAir)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCut), ForceMode2D.Impulse);
            StartCoroutine(CanJumpReset());
        }
    }

    private void setGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
    private void beginDashSlow()
    {
        canResetDash = false;
        slowDownLength = 2f;
        Debug.Log("TimeSlow");
        cameraController.IsDashing();
        timeManager.SlowDownTime();
    }


    private IEnumerator StopDashing()
    {
        yield return new WaitForSecondsRealtime(dashTime);
        
        isDashing = false;
        isActuallyDashing = false;
        MeleeAttackManager.canAction = true;
        canResetDash = true;
        onJumpUp();
        canResetJump = true;
    }
    private IEnumerator jumpReset()
    {
        yield return new WaitForSecondsRealtime(.1f);
        canJump = true;
        isJumping = false;
    }
    private IEnumerator CanJumpReset()
    {
        yield return new WaitForSecondsRealtime(.15f);
        canResetJump = true;
    }
    private IEnumerator TimeBeforeSlow()
    {
        yield return new WaitForSecondsRealtime(dashStartTime);
        if (!isActuallyDashing) {
            beginDashSlow();
        }


    } 
}

