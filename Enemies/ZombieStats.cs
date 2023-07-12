using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    public bool canAttack = true;
    public int damage;
    public float attackSpeed;
    public int maxNumDeadZombies = 10; // Maximum number of dead zombies to keep track of
    private float deathTime;

    private ZombieController zombieController;
    private ScoreboardManager scoreboardManager;

    private static List<ZombieStats> deadZombies = new List<ZombieStats>();

    private void Start()
    {
        GetReferences();
        InitVariable();
    }

    /*private void Update()
    {
        Debug.Log("Zombie Health is " + health);
    }*/

    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    }

    public override void Die()
    {
        if (isDead)
        {
            return; // Zombie is already dead, exit the method, okay great ChatGPT but this is the issue where is dont count them all up 
                    // i counted 8 bodies on the floor but only 5 on the scoreboard hmmm
        }

        base.Die();
        zombieController.Die();

        if (deadZombies.Count >= maxNumDeadZombies)
        {
            ZombieStats oldestZombie = deadZombies[0];
            foreach (ZombieStats zombie in deadZombies)
            {
                if (zombie.deathTime < oldestZombie.deathTime)
                {
                    oldestZombie = zombie;
                }
            }
            deadZombies.Remove(oldestZombie);
            Destroy(oldestZombie.gameObject);
        }

        deadZombies.Add(this);
        deathTime = Time.time;

        scoreboardManager.UpdateZombieKills(1);
    }

    public void InitVariable()
    {
        maxHealth = 150;
        SetHealthTo(maxHealth);
        isDead = false;
        damage = 25;
        attackSpeed = 2f;
        canAttack = true;
    }

    public void GetReferences()
    {
        zombieController = GetComponent<ZombieController>();
        scoreboardManager = FindObjectOfType<ScoreboardManager>();
    }
}