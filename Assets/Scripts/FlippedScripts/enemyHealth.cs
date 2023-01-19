using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public bool damageable = true;
    public bool giveUpwardForce = true;
    public bool bounceCollide;
    public bool giveDashReset;
    public int health = 1;
    public float bounceMult;
    private int currentHealth;
    private void Start()
    {
            currentHealth = health;
    }

    public void Damage(int amount)
    {
        //checks if enemy can take damage
        if(damageable && currentHealth > 0)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //make death animation
                gameObject.SetActive(false);
            }
        }
    }
    public void PlayerDied()
    {
        currentHealth = health;
        gameObject.SetActive(true);
    }
}
