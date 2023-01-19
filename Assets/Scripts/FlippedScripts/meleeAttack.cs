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
    public float groundMult;
    private float mult;
    private float multiHitMult = 1;
    public float multiHitMultAddition;
    public float upForceOnSide;
    private bool EnemyHitDie;
    private bool UpStrike;
    private bool Side1;
    private GameObject gameManager;


    private void Start()
    {
        characterController = GetComponentInParent<CharecterController>();
        rb = GetComponentInParent<Rigidbody2D>();
        MeleeAttackManager = GetComponentInParent<meleeAttackManager>();
        gameManager = GameObject.Find("GameManager");

    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks what enemy is hit
        if (collision.GetComponent<enemyHealth>())
        {
            HandleCollision(collision.GetComponent<enemyHealth>());
        }
    }
    void HandleCollision(enemyHealth objHealth)
    {
        //check if object affects vertical momentum then sets variables for collision
        if (objHealth.giveUpwardForce)
        {
            if (Input.GetAxis("Vertical") < 0 && !characterController.isGrounded && objHealth.bounceCollide)
            {
                bounceMultActive = true;
                direction = Vector2.up;
                downwardStrike = true;
                collided = true;
            }
            else if (Input.GetAxis("Vertical") < 0 && !characterController.isGrounded)
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
                UpStrike = true;
            }

            else if (Input.GetAxis("Vertical") > 0 && !characterController.isGrounded)
            {
                direction = Vector2.down;
                collided = true;
                UpStrike = true;
            }

        }
        //processes what kind of thing collided with

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
        //checks if object can take damage then deals damage
        objHealth.Damage(damageAmount);
        if (objHealth.damageable)
        {
            EnemyHitDie = true;
        }
        else
        {
            EnemyHitDie = false;
        }
        //processes bounce mult
        bounceMult = objHealth.bounceMult;
        if(objHealth.giveDashReset)
        {
            characterController.canDash = true;
            Debug.Log("gave dash");
        }
        StartCoroutine(NoLongerColliding());
    }

    //runs check and handles all force application
    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            mult = groundMult;
        }
        else if (bounceMultActive)
        {
            bounceMultActive = false;
            mult = mult * bounceMult;
        }
        else
        {
            mult = 1;
        }
       
        if (collided)
        {
            if (downwardStrike)
            {
                rb.velocity = new Vector2(rb.velocity.x * .1f, 0);
                rb.AddForce(direction * MeleeAttackManager.upwardsForce * mult);
            }
            else if (characterController.isGrounded && UpStrike)
            {
                rb.AddForce(-direction * MeleeAttackManager.upwardsForce * mult);
            }
            else
            {
                Vector2 Y = new Vector2(0, 1);
                if (EnemyHitDie)
                {
                    rb.AddForce(-direction * MeleeAttackManager.defaultForce * mult * multiHitMult);
                    rb.AddForce(Y * upForceOnSide * mult);
                }
                else
                {
                    rb.AddForce(direction * MeleeAttackManager.defaultForce * mult * multiHitMult);
                }
            }
            collided = false;
            multiHitMult -= multiHitMultAddition;
        }
    }
    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSecondsRealtime(MeleeAttackManager.movementTime);
        collided = false;
        downwardStrike = false;
        multiHitMult = 1f;
    }
    

}
