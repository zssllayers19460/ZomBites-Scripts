using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script derives from CharcterStats 
   without that this is nothing */

public class PlayerStats : CharacterStats
{
    private PlayerHUD hud;

    private void Awake()
    {
        GetReferences();
        InitVariables();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        hud.UpdateHealth(health, maxHealth);
    }

    private void GetReferences()
    {
        hud = GetComponent<PlayerHUD>();
    }
}
