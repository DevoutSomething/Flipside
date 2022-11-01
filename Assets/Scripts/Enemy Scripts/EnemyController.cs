using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask groundLayer;
    private BoxCollider2D boxCollider2d;
    public bool facingRight;
    private bool canSeePlayer;
    public float turnTimePerm = 1;
    [SerializeField] private float turnTimer;
    public float speed;
    public float rayDistance;
    public GameObject rayObject;

    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        turnTimer = turnTimePerm;
    }
    void Update()
    {
        MoveForward();
        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
        }
        Checkwalls();
    }
    private void Checkwalls()
    {
        float leftMultiply;
        if (facingRight)
        {
            leftMultiply = 1;
        }
        else
        {
            leftMultiply = -1;
        }
        RaycastHit2D raycastHit = Physics2D.Raycast(rayObject.transform.position, Vector2.right * leftMultiply, rayDistance, groundLayer);
        {
            if (raycastHit.collider != null)
            {
                Debug.DrawRay(rayObject.transform.position, Vector2.right * leftMultiply * raycastHit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(rayObject.transform.position, Vector2.right * leftMultiply * raycastHit.distance, Color.green);
            }
        }
    }
    public void TurnAround()
    {
        if (turnTimer <= 0)
        {
            Debug.Log("turn around");
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            if (transform.localScale.x < 0)
            {
                facingRight = true;
            }
            else
            {
                facingRight = false;
            }
            turnTimer = turnTimePerm;
        }
    }
    private void MoveForward()
    {
        if (facingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (!facingRight)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }
}
