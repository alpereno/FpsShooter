using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    //Parent class of Player and Enemy
    public float startingHealth = 100;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void takeHit(float damage, RaycastHit hit)
    {
        //some stuff with hit 
        //detect hit point instantiate particle 
        takeDamage(damage);
    }

    public void takeDamage(float damage) {
        health -= damage;

        if (health <= 0 & !dead)
        {
            die();
        }
    }

    private void die()
    {
        health = 0;
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

    public float getHealth() {
        return health;
    }
}
