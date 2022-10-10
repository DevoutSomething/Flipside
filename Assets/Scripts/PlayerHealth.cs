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
    public bool death;
    public Vector2 respawnPoint;
    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        deathTimerTEMP = deathTimer;
        respawnPoint = gameObject.transform.position;
    }
    private void Update()
    {
        if(TempHealth <= 0 || death == true)
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
        death = true;
        deathTimerTEMP = deathTimer;
    }

    void RespawnPlayer()
    {
        death = false;
        playerAnimator.SetBool("Death", false);
        gameObject.transform.position = respawnPoint;
        TempHealth = 1;
    }
}
