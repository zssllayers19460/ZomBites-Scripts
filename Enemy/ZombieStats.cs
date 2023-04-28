using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    public bool canAttack = true;
    public int damage;
    public float attackSpeed;
    public float destroyDeadZombieDelay = 20f;

    private ZombieController zombieController;
    

    private void Start()
    {
        GetReferences();
        InitVariable();
    }

    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        zombieController.Die();
        Destroy(gameObject, destroyDeadZombieDelay);
    }

    public void InitVariable()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
        damage = 25;
        attackSpeed = 2f;
        canAttack = true;
    }

    public void GetReferences()
    {
        zombieController = GetComponent<ZombieController>();
    }
}