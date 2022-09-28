using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttackManager : MonoBehaviour
{
    public float defaultForce;
    public float upwardsForce;
    public float movementTime = .1f;
    private bool meleeAttack;
    private Animator meleeAnimator;

    private Animator anim;

    private CharecterController charecterController;
    private void Start()
    {
        anim = GetComponent<Animator>();
        charecterController = GetComponent<CharecterController>();
        meleeAnimator = GetComponentInChildren<meleeAttack>().gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            meleeAttack = true;
        }
        else
        {
            meleeAttack = false;
        }

        if(meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            anim.SetTrigger("UpwardAttack");
            meleeAnimator.SetTrigger("UpwardAttackSwipe");
        }
        if(meleeAttack && Input.GetAxis("Vertical") < 0 && !charecterController.isGrounded)
        {
            anim.SetTrigger("DownwardAttack");
            meleeAnimator.SetTrigger("DownwardAttackSwipe");
        }
        if(meleeAttack && Input.GetAxis("Vertical") == 0 || meleeAttack && (Input.GetAxis("Vertical") < 0 && charecterController.isGrounded))
        {
            anim.SetTrigger("ForwardAttack");
            meleeAnimator.SetTrigger("ForwardAttackSwipe");
        }
    }



}
