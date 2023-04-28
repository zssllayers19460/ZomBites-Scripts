using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool isDead;

    private void Start()
    {
        InitVariables();
    }

    public virtual void CheckHealth()
    {
        if(health <= 0)
        {
            health = 0;
            Die();
        }

        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;  
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetHealthTo(int healthToSetTo)
    {
        health = healthToSetTo;
        CheckHealth();
    }

    public void TakeDamage(int damage)
    {
        int healthAfterDamage = health - damage;
        SetHealthTo(healthAfterDamage);
    }

    public void Heal(int heal)
    {
        int healthAfterHeal = health + heal;
        SetHealthTo(healthAfterHeal);
    }

    public virtual void InitVariables()
    {
        maxHealth = 150;
        SetHealthTo(maxHealth);
        isDead = false;
    }
}
