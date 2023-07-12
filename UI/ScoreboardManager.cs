using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI zombieKillsText;
    public TextMeshProUGUI playerKillsText;
    public TextMeshProUGUI damageCountText;
    public TextMeshProUGUI nameText;

    private int zombieKills;
    private int playerKills;
    private int damageCount;
    public string playerName;

    private Inventory inventory;
    private EquipmentManager manager;

    private void Start()
    {
        GetReferences();
        UpdateNameText();
    }

    private void UpdateNameText()
    {
        nameText.text = playerName;
    }

    public void UpdateDamageCount(int damage)
    {
        damageCount += damage;
        damageCountText.text = damageCount.ToString();
    }

    public void UpdatePlayerKills(int value)
    {
        playerKills += value;
        playerKillsText.text = playerKills.ToString();
    }

    public void UpdateZombieKills(int value)
    {
        zombieKills += value;
        zombieKillsText.text = zombieKills.ToString();
    }

    private void GetReferences()
    {
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
    }
}