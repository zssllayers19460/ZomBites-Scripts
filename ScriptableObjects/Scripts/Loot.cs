using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot/Add a New Loot...")]
public class Loot : LootItemScriptableObject
{
    public GameObject lootPrefab;
    public float spawnChance;
    public float hungerRegenrationAmount;
    public float thirstRegenrationAmount;
    public float healthRegenrationAmount;
    public LootType lootType;
}

public enum LootType
{
    Food,
    Water,
    Health,
    Ammo,
    Equipment,
    None
}