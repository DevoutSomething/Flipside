using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 1;
    [SerializeField] private Rigidbody2D rb;
    private meleeAttackManager MeleeAttackManager;
    private Vector2 direction;
    private bool collided;
    private bool downwardStrike;
    private CharecterController characterController;
    private bool bounceMultActive;
    private float bounceMult;
    private void Start()
    {
        characterController = GetComponentInParent<CharecterController>();
        rb = GetComponentInParent<Rigidbody2D>();
        MeleeAttackManager = GetComponentInParent<meleeAttackManager>();
    }
    private void FixedUpdate()
    {
        HandleMovment();
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
        
        if(objHealth.giveUpwardForce && Input.GetAxis("Vertical")< 0 && !characterController.isGrounded && objHealth.bounceCollide)
        {
            bounceMultActive = true;
            direction = Vector2.up;
            downwardStrike = true;
            collided = true;
        }
        else if (objHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !characterController.isGrounded)
        {
            direction = Vector2.up;
            downwardStrike = true;
            collided = true;
        }
        else if (Input.GetAxis("Vertical") > 0 && !characterController.isGrounded && objHealth.bounceCollide)
        {
            bounceMultActive = true;
            direction = Vector2.down;
            collided = true;
        }

        else if (Input.GetAxis("Vertical") > 0 && !characterController.isGrounded)
        {
            direction = Vector2.down;
            collided = true;
        }
       
            if ((Input.GetAxis("Vertical") <= 0 && characterController.isGrounded) || Input.GetAxis("Vertical") == 0)
        {
            if (characterController.FacingRight && objHealth.bounceCollide)
            {
                bounceMultActive = true;
                direction = Vector2.left;
                collided = true;
            }
            else if (characterController.FacingRight == false && objHealth.bounceCollide)
            {
                bounceMultActive = true;
                direction = Vector2.right;
                collided = true;
            }
            else if (characterController.FacingRight)
            {
                direction = Vector2.left;
                collided = true;
            }
            else if(characterController.FacingRight == false)
            {
                direction = Vector2.right;
                collided = true;
            }
        }
        objHealth.Damage(damageAmount);
        bounceMult = objHealth.bounceMult;
        if(objHealth.giveDashReset)
        {
            characterController.canDash = true;
            Debug.Log("gave dash");
        }
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovment()
    {
        if (collided)
        {
            if (downwardStrike)
            {
                rb.velocity = new Vector2(0, 0);
                if (bounceMultActive)
                {
                    rb.AddForce(direction * (MeleeAttackManager.upwardsForce * bounceMult));
                    bounceMultActive = false;
                }
                else
                {
                    rb.AddForce(direction * MeleeAttackManager.upwardsForce);
                }
                
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (bounceMultActive)
                {
                    rb.AddForce(direction * MeleeAttackManager.defaultForce * bounceMult);
                    bounceMultActive = false;
                }
                else
                {
                    rb.AddForce(direction * MeleeAttackManager.defaultForce);
                }
            }
            collided = false;
        }
    }
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSecondsRealtime(MeleeAttackManager.movementTime);
        collided = false;
        downwardStrike = false;
        
    }
    

}
