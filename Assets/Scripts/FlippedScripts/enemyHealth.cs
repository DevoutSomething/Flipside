using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public bool damageable = true;
    public bool giveUpwardForce = true;
    public bool bounceCollide;

    public int health = 1;
    private int currentHealth;
    private void Start()
    {
            currentHealth = health;
    }

    public void Damage(int amount)
    {
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
}
