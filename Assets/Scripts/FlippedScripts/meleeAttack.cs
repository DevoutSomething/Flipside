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
    private CharacterController characterController;


    private void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        rb = GetComponentInParent<Rigidbody2D>();
        MeleeAttackManager = GetComponentInParent<meleeAttackManager>();
    }
    private void FixedUpdate()
    {
        //HandleMovement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
