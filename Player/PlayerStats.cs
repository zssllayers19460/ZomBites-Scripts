using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script derives from CharcterStats 
   without that this is nothing */

public class PlayerStats : CharacterStats
{
    private PlayerHUD hud;
    private UiManager uiManager;
    private ScoreboardManager scoreboardManager;

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

    public override void Die()
    {
        base.Die();
        uiManager.SetActiveHud(false);
        scoreboardManager.UpdatePlayerKills(1);
    }

    private void GetReferences()
    {
        hud = GetComponent<PlayerHUD>();
        uiManager = GetComponent<UiManager>();
        scoreboardManager = FindObjectOfType<ScoreboardManager>();
    }
}