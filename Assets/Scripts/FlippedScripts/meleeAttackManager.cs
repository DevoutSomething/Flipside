using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttackManager : MonoBehaviour
{
    public float defaultForce;
    public float upwardsForce;
    public float movementTime = .1f;
    private bool meleeAttack;
    public GameObject meleeStuff;
    public GameObject player;
    private Animator meleeAnimator;

    private Animator anim;
    private CharecterController charecterController;
    private void Start()
    {
        meleeAnimator = meleeStuff.GetComponentInChildren<Animator>();
        anim = player.GetComponentInChildren<Animator>();
        charecterController = GetComponent<CharecterController>();
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
            Debug.Log("working melee");
        }
        else
        {
            meleeAttack = false;
        }

        if(meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            anim.SetTrigger("UpwardAttack");
            Debug.Log("1");
        }
        if(meleeAttack && Input.GetAxis("Vertical") < 0 && !charecterController.isGrounded)
        {
            anim.SetTrigger("DownwardAttack");
            Debug.Log("2");
        }
        if(meleeAttack && Input.GetAxis("Vertical") == 0 || meleeAttack && (Input.GetAxis("Vertical") < 0 && charecterController.isGrounded))
        {
            anim.SetTrigger("ForwardAttack");
            Debug.Log("3");
        }
    }



}
