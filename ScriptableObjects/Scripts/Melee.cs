using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add a New Melee...")]
public class Melee : Item
{
    public GameObject meleePrefab;
    public GameObject explosionEffect;
    public float throwForce;
    public float explosionDelay;
    public float explosionRadius;
    public int damage;
    public MeleeAudioSO meleeAudioSO;
    public HandHeldStyle handHeldStyle;
    public HandHeldType handHeldType;
}

public enum HandHeldStyle
{
    FragGrenade,
    ChemicalGrenade,
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