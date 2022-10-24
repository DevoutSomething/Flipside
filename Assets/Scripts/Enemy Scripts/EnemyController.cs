using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool facingRight;
    private bool canSeePlayer;
    public Collider2D wallColider;
    public Collider2D floorColider;
    public float timePerm = 1;
    private float timer;

    private void Start()
    {
        float timer = timePerm;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TurnAround();
            timer = timePerm;
        }
    }

    private void CheckForGround()
    {

    }
    private void CheckForWall()
    {

    }
    private void TurnAround()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        if (transform.localScale.x < 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }
    }
}
