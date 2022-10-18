using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttackManager : MonoBehaviour
{
    public float defaultForce;
    public float upwardsForce;
    public float movementTime = .1f;
    private bool meleeAttack;
    public GameObject player;
    public Animator meleeAnimator;
    public float attackCooldown;
    public float timeCantAction;
    public bool canAction;
    public bool canAttack;
    private Animator anim;
    private CharecterController charecterController;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        canAttack = true;
        canAction = true;
        anim = player.GetComponentInChildren<Animator>();
        charecterController = GetComponent<CharecterController>();
    }
    private void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2")  && canAttack && canAction)    
        {
            meleeAttack = true;
            canAttack = false;
            canAction = false;
            Debug.Log("working melee");
            StartCoroutine(AttackCooldown());
            StartCoroutine(AttackNoAction());
            if (charecterController.isGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            meleeAttack = false;
        }

        if(meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            anim.SetTrigger("UpwardAttack");
            meleeAnimator.SetTrigger("AttackUp");
            
            Debug.Log("1");
        }
        if(meleeAttack && Input.GetAxis("Vertical") < 0 && !charecterController.isGrounded)
        {
            anim.SetTrigger("DownwardAttack");
            meleeAnimator.SetTrigger("AttackDown");

            Debug.Log("2");
        }
        if(meleeAttack && Input.GetAxis("Vertical") == 0 || meleeAttack && (Input.GetAxis("Vertical") < 0 && charecterController.isGrounded))
        {
            anim.SetTrigger("ForwardAttack");
            meleeAnimator.SetTrigger("AttackSide");
            Debug.Log("3");
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSecondsRealtime(attackCooldown);
        canAttack = true;

    }
    private IEnumerator AttackNoAction()
    {
        yield return new WaitForSecondsRealtime(timeCantAction);
        canAction = true;

    }
   
}
