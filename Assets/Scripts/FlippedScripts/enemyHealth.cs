using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public bool damageable = true;
    public bool giveUpwardForce = true;
    public int health = 1;
    private int currentHealth;

    private void Start()
    {
        {
            currentHealth = health;
        }
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);
        }
    }
}
