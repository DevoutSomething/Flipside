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
    public bool canTransitionState;
    public float timeCantTransition;
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

        if (Input.GetButtonDown("Fire2") && canAction)    
        {
            meleeAttack = true;
            canAction = false;
            Debug.Log("working melee");
            canAction = false;
            StartCoroutine(AttackNoAction());
            if (charecterController.isGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            meleeAttack = false;
            anim.SetBool("UpwardAttack", false);
            anim.SetBool("DownwardAttack", false);
            anim.SetBool("ForwardAttack", false);                                               
            anim.SetBool("ForwardAttack", false);
            anim.SetBool("UpwardAttackAir", false);
            

        }


        if (meleeAttack && Input.GetAxis("Vertical") > 0 && !charecterController.isGrounded)
        {
            anim.SetBool("UpwardAttackAir", true);
            meleeAnimator.SetTrigger("AttackUpAir"); 
           
        }
        if(meleeAttack && Input.GetAxis("Vertical") > 0 && charecterController.isGrounded   )
        {
            anim.SetBool("UpwardAttack", true);
            meleeAnimator.SetTrigger("AttackUp");
        }
        
            if (meleeAttack && Input.GetAxis("Vertical") < 0 && !charecterController.isGrounded)
        {
            anim.SetBool("DownwardAttack", true);
            meleeAnimator.SetTrigger("AttackDown");
        }
        if(meleeAttack && Input.GetAxis("Vertical") == 0 || meleeAttack && (Input.GetAxis("Vertical") < 0 && charecterController.isGrounded))
        {
            anim.SetBool("ForwardAttack", true);
            meleeAnimator.SetTrigger("AttackSide");
        }
        if(Input.GetAxis("Horizontal") != 0 && charecterController.isGrounded )
        {
            Debug.Log("archer left me");
            //ResetAnim();
            anim.SetBool("Run", true);
            meleeAnimator.SetBool("Idle2",true);
        }
        if (Input.GetButtonDown("Jump") && charecterController.isGrounded)
        {
            Debug.Log("Archer saved me");
            ResetAnim();
            anim.SetBool("Jump", true);
            meleeAnimator.SetBool("Idle2", true);
        }

    }
    private void ResetAnim()
    {
        meleeAttack = false;
        anim.SetBool("UpwardAttack", false);
        anim.SetBool("DownwardAttack", false);
        anim.SetBool("ForwardAttack", false);
        anim.SetBool("ForwardAttack", false);
        anim.SetBool("UpwardAttackAir", false);
        meleeAnimator.ResetTrigger("AttackDown");
        meleeAnimator.ResetTrigger("AttackSide");
        meleeAnimator.ResetTrigger("AttackUp");
        meleeAnimator.ResetTrigger("AttackUpAir");
    }
    private IEnumerator AttackNoAction()
    {
        yield return new WaitForSecondsRealtime(timeCantAction);
        canAction = true;

    }
    private IEnumerator AttackTransition()
    {
        yield return new WaitForSecondsRealtime(timeCantTransition);
        canTransitionState = true;
    }
    //archer fucked up
    //archer fucked up again
}
