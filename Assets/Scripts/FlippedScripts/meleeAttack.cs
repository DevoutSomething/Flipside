using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 1;
    private Rigidbody2D rb;
    private meleeAttackManager MeleeAttackManager;
    private Vector2 direction;
    private bool collided;
    private bool downwardStrike;
    private CharecterController characterController;


    private void Start()
    {
        characterController = GetComponentInParent<CharecterController>();
        rb = GetComponentInParent<Rigidbody2D>();
        MeleeAttackManager = GetComponentInParent<meleeAttackManager>();
    }
    private void FixedUpdate()
    {
        //HandleMovement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<enemyHealth>())
        {
            HandleCollision(collision.GetComponent<enemyHealth>());
        }
    }
    void HandleCollision(enemyHealth objHealth)
    {
        if(objHealth.giveUpwardForce && Input.GetAxis("Vertical")< 0 && !characterController.isGrounded)
        {
            direction = Vector2.up;
            downwardStrike = true;
            collided = true;
        }
        if (Input.GetAxis("Vertical") > 0 && !characterController.isGrounded)
        {
            direction = Vector2.down;
            collided = true;
        }
        if((Input.GetAxis("Vertical") <= 0 && characterController.isGrounded) || Input.GetAxis("Vertical") == 0)
        {
            if (characterController.FacingRight)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
            collided = true;
        }
        objHealth.Damage(damageAmount);
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovment()
    {
        if (collided)
        {
            if (downwardStrike)
            {
                rb.AddForce(direction * MeleeAttackManager.upwardsForce);
            }
            else
            {
                rb.AddForce(direction * MeleeAttackManager.defaultForce);
            }
        }
    }
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSecondsRealtime(MeleeAttackManager.movementTime);
        collided = false;
        downwardStrike = false;
        
    }
}
