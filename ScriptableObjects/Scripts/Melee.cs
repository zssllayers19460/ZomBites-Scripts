using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Add a New Melee...")]
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
    public int grenadeToHold;  // amount of grenades allowed to hold in the hand at once
    public int grenadesInInventory; // amount of grenades allowed in the inventory
    public MeleeAudioSO meleeAudioSO;
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