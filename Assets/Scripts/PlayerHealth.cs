using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int Health = 1;
    public int TempHealth = 1;
    
    public Animator playerAnimator;
    public float deathTimer;
    private float deathTimerTEMP;
    public GameObject spawnPoint;
    public bool death;
    public Vector2 respawnPoint;
    private meleeAttackManager MeleeAttackManager;
    
    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        MeleeAttackManager = gameObject.GetComponent<meleeAttackManager>();
        deathTimerTEMP = deathTimer;
        respawnPoint = spawnPoint.transform.position;
    }
    private void Update()
    {
        if (TempHealth <= 0 && death != true)
        {
            death = true;
            StartDeathTimer();
            playerAnimator.SetBool("Death", true);
            playerAnimator.SetBool("Run", false);
            playerAnimator.SetBool("jump", false);


        }
        if (death && deathTimerTEMP >= 0)
        {
            deathTimerTEMP -= Time.fixedDeltaTime;
        }
        if(deathTimerTEMP <= 0 && death)
        {
            RespawnPlayer();
        }
    }
    void StartDeathTimer()
    {
        MeleeAttackManager.canAction = false;
        death = true;
        deathTimerTEMP = deathTimer;
    }

    void RespawnPlayer()
    {
        death = false;
        playerAnimator.SetBool("Death", false);
        gameObject.transform.position = respawnPoint;
        TempHealth = 1;
        MeleeAttackManager.canAction = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("obstacle"))
        {
            TempHealth -= 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("obstacle"))
        {
            TempHealth -= 1;
        }
    }
}
