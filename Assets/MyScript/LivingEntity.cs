using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    float startingHealth = 100;
    public float health;
    public bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        dead = false;
    }

    public void TakeHit(float damage)
    {
        health = health - damage;
        if (health <= 0 && !dead)
        {
            health = 0;
            Die();
        }
    }
    protected virtual void Die()
    {
        dead = true;
        // health = startingHealth;
        if (OnDeath != null)
        {
            OnDeath();
        }
    }
}
