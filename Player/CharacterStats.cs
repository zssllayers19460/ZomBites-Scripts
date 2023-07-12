using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool isDead;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;
    private Animator anim;

    private void Start()
    {
        InitVariables();

        // Disable ragdoll at start
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    public void ReduceHunger(float amount)
    {
        // Reduce the current hunger by the specified amount
        GetComponent<HealthMechanics>().currentHunger -= amount;
        GetComponent<HealthMechanics>().currentHunger = Mathf.Clamp(GetComponent<HealthMechanics>().currentHunger, 0f, 100f);
    }

    public void ReduceThirst(float amount)
    {
        // Reduce the current thirst by the specified amount
        GetComponent<HealthMechanics>().currentThirst -= amount;
        GetComponent<HealthMechanics>().currentThirst = Mathf.Clamp(GetComponent<HealthMechanics>().currentThirst, 0f, 100f);
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

        /* Ragdoll for the player */
        if (CompareTag("Player"))

        {
            foreach (Collider col in ragdollColliders)
            {
                col.enabled = true;
            }
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                rb.isKinematic = false;
            }
        }
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

        // Get ragdoll colliders and rigidbodies excluding the Character Controller collider
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Exclude the Character Controller collider from the ragdoll colliders
        List<Collider> colliderList = new List<Collider>(ragdollColliders);
        colliderList.RemoveAll(col => col is CharacterController);
        ragdollColliders = colliderList.ToArray();
    }
}