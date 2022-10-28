using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool facingRight;
    private bool canSeePlayer;
    public Collider2D wallColider;
    public Collider2D floorColider;
    public float turnTimePerm = 1;
    [SerializeField] private float turnTimer;
    public float speed;

    private void Start()
    {
        turnTimer = turnTimePerm;
    }
    void Update()
    {
        MoveForward();
        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
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
