using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Melee/Add a New Melee...")]
public class Melee : Item
{
    public GameObject meleePrefab;
    public GameObject explosionEffect;
    public Sprite dangerGrenadeImage;
    public float throwForce;
    public float explosionDelay;
    public float explosionRadius;
    public float explosionForce;
    public int damage;
    public int storedGrenades;  // amount of grenades allowed to hold in the hand at once
    public int maxGrenades; // amount of grenades allowed in the inventory
    public HandHeldStyle handHeldStyle;
    public HandHeldType handHeldType;
}

public enum HandHeldStyle
{
    FragGrenade,
    SmokeGrenade,
    FlashbangGrenade,
    StunGrenade,
    DecoyGrenade,
    GasGrenade,
    Equipment
}

public enum HandHeldType
{
    Lethal,
    NonLethal
}