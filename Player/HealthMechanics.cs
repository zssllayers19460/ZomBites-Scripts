using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMechanics : MonoBehaviour
{
    public float hungerRate = 0.5f; // Rate at which hunger increases per second
    public float thirstRate = 0.3f; // Rate at which thirst increases per second
    public float hungerThreshold = 70f; // Threshold at which hunger starts affecting health
    public float thirstThreshold = 60f; // Threshold at which thirst starts affecting health

    [HideInInspector] public float currentHunger = 0f;
    [HideInInspector] public float currentThirst = 0f;

    public float hungerDamagePerSecond = 5f; // Amount of damage per second when hunger reaches the threshold
    public float thirstDamagePerSecond = 3f; // Amount of damage per second when thirst reaches the threshold
    public float damageInterval = 3f; // Time interval between each damage tick

    private float damageTimer = 0f; // Timer to track the time between damage ticks

    private void Update()
    {
        IncreaseHunger();
        IncreaseThirst();

        float modifiedHungerDamage = ApplyDamageMultiplier(hungerDamagePerSecond, currentHunger);
        float modifiedThirstDamage = ApplyDamageMultiplier(thirstDamagePerSecond, currentThirst);

        if (currentHunger >= hungerThreshold)
        {
            HandleDamageTick(modifiedHungerDamage);
        }

        if (currentThirst >= thirstThreshold)
        {
            HandleDamageTick(modifiedThirstDamage);
        }
    }

    private void IncreaseHunger()
    {
        currentHunger += hungerRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, 100f);
    }

    private void IncreaseThirst()
    {
        currentThirst += thirstRate * Time.deltaTime;
        currentThirst = Mathf.Clamp(currentThirst, 0f, 100f);
    }

    private float ApplyDamageMultiplier(float baseDamage, float currentValue)
    {
        if (currentValue >= 99f)
        {
            return baseDamage * 3f; // Triple the damage if the value is 100 or above
        }
        else if (currentValue >= 80f)
        {
            return baseDamage * 2f; // Double the damage if the value is 80 or above
        }
        else if (currentValue >= 30f)
        {
            return baseDamage * 1f; // Double the damage if the value is 30 or above
        }
        else
        {
            return baseDamage;
        }
    }

    private void HandleDamageTick(float damagePerSecond)
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            GetComponent<CharacterStats>().TakeDamage(Mathf.FloorToInt(damagePerSecond));
            damageTimer = 0f;
        }
    }
}