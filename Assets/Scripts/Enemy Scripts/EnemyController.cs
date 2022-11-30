using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BoxCollider2D boxCollider2d;
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    [Header("Turning")]
    public bool facingRight;
    public float turnTimePerm = 1;
    public float turnDistance;
    [SerializeField] private float turnTimer;
    [Header("Obstacles")]
    public GameObject raySWall;
    public GameObject rayWall;
    public GameObject rayFloor;
    public GameObject rayPlayer;
    [SerializeField] private bool seeSW;
    [SerializeField] private bool seeW;
    public float floorCheckOfset;
    [Header("Movement")]
    public float chargeMultiplyer;
    public float speed;
    public float rayDistance;
    private Rigidbody2D rb;
    public float jumpForce;
    public float jumpTimerPerm;
    private float jumpTimer;
    private bool canMoveForward;
    [SerializeField] private bool stopMoving;
    [Header("Player")]
    private bool canSeePlayer;
    public float playerSearchDistance;
    private bool seePlayerR;
    private bool seePlayerL;
    [Header("Combat")]
    [SerializeField] private bool attackMode;
    public float agroTime;
    [SerializeField] private float agroTimer;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        turnTimer = 0;
    }
    void Update()
    {
        MoveForward();
        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
        }
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        Checkwalls();
        LookForPlayer();
        if (agroTimer <= 0)
        {
            attackMode = false;
        }
    }
    private void Checkwalls()
    {
        #region Direction
        float leftMultiply;
        if (facingRight)
        {
            leftMultiply = 1;
        }
        else
        {
            leftMultiply = -1;
        }
        #endregion
        canMoveForward = true;
        #region CheckShortWall
        RaycastHit2D raycastHitSW = Physics2D.Raycast(raySWall.transform.position, Vector2.right * leftMultiply, rayDistance, groundLayer);
        if (raycastHitSW.collider != null)
        {
            Debug.DrawRay(raySWall.transform.position, Vector2.right * leftMultiply * raycastHitSW.distance, Color.red);
            if (raycastHitSW.distance < turnDistance)
            {
                seeSW = true;
            }
        }
        else
        {
            Debug.DrawRay(raySWall.transform.position, Vector2.right * leftMultiply * rayDistance, Color.green);
            seeSW = false;
        }
        #endregion
        #region CheckWall
        RaycastHit2D raycastHitW = Physics2D.Raycast(rayWall.transform.position, Vector2.right * leftMultiply, rayDistance, groundLayer);
        if (raycastHitW.collider != null)
        {
            Debug.DrawRay(rayWall.transform.position, Vector2.right * leftMultiply * raycastHitW.distance, Color.red);
            if (raycastHitW.distance < turnDistance)
            {
                seeW = true;
            }
        }
        else
        {
            Debug.DrawRay(rayWall.transform.position, Vector2.right * leftMultiply * rayDistance, Color.green);
            seeW = false;
        }
        #endregion
        #region CheckFloor
        Vector2 fRayStartPoint;
        fRayStartPoint = rayFloor.transform.position;
        fRayStartPoint.x = rayFloor.transform.position.x + (floorCheckOfset * leftMultiply);
        RaycastHit2D raycastHitF = Physics2D.Raycast(fRayStartPoint, Vector2.down, rayDistance, groundLayer);
        if (raycastHitF.collider != null)
        {
            Debug.DrawRay(fRayStartPoint, Vector2.down * raycastHitF.distance, Color.green);
        }
        else
        {
            canMoveForward = false;
            Debug.DrawRay(fRayStartPoint, Vector2.down * rayDistance, Color.red);
            if (!attackMode)
            {
                TurnAround();
                
            }
            else
            {
                stopMoving = true;
            }
        }
        #endregion
        #region Reaction
        if (seeW)
        {
            canMoveForward = false;
            if (!attackMode)
            {
                TurnAround();
            }
            else
            {
                stopMoving = true;
            }
        }
        if (!seeW && seeSW)
        {
            if (!attackMode)
            {
                Jump();
            }
            else
            {
                stopMoving = true;
            }
        }
        #endregion
        if (canMoveForward)
        {
            stopMoving = false;
        }
    }
    public void TurnAround()
    {
        //Debug.Log("turn around");
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        if (transform.localScale.x < 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
        turnTimer = turnTimePerm;
    }
    private void MoveForward()
    {
        float moveSpeed;
        if (attackMode)
        {
            moveSpeed = speed * chargeMultiplyer;
            if (stopMoving)
            {
                moveSpeed = 0;
            }
        }
        else
        {
            moveSpeed = speed;
        }
        if (facingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (!facingRight)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        
    }
    private void Jump()
    {
        //Debug.Log("Jump");
        if (jumpTimer <= 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = jumpTimerPerm;
        }
    }
    private void LookForPlayer()
    {
        
        RaycastHit2D raycastHitPlayerR = Physics2D.Raycast(rayPlayer.transform.position, Vector2.right, playerSearchDistance, playerLayer);
        if (raycastHitPlayerR.collider != null)
        {
            Debug.DrawRay(rayPlayer.transform.position, Vector2.right * raycastHitPlayerR.distance, Color.red);
            seePlayerR = true;
        }
        else
        {
            Debug.DrawRay(rayPlayer.transform.position, Vector2.right * playerSearchDistance, Color.green);
            seePlayerR = false;
        }
        RaycastHit2D raycastHitPlayerL = Physics2D.Raycast(rayPlayer.transform.position, Vector2.left, playerSearchDistance, playerLayer);
        if (raycastHitPlayerL.collider != null)
        {
            Debug.DrawRay(rayPlayer.transform.position, Vector2.left * raycastHitPlayerL.distance, Color.red);
            seePlayerL = true;
        }
        else
        {
            Debug.DrawRay(rayPlayer.transform.position, Vector2.left * playerSearchDistance, Color.green);
            seePlayerL = false;
        }
        if (seePlayerR || seePlayerL)
        {
            canSeePlayer = true;
            attackMode = true;
            agroTimer = agroTime;
            if (facingRight && seePlayerL)
            {
                TurnAround();
                if (stopMoving)
                {
                    agroTimer -= Time.deltaTime;
                }
            }
            if (!facingRight && seePlayerR)
            {
                TurnAround();
                if (stopMoving)
                {
                    agroTimer -= Time.deltaTime;
                }
            }
        }
        if (!seePlayerL && !seePlayerR)
        {
            canSeePlayer = false;
            if (attackMode && agroTimer > 0)
            {
                agroTimer -= Time.deltaTime;
            }
        }
        
    }
}
