using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Handheld Weapon")]
public class HandheldWeapon : Item
{
    public GameObject weaponPrefab; // Prefab to the weapon itself
    public GameObject explosionEffect; // Effect to play when the weapon explodes
    public float range; // Weapon range
    public float throwForce; // Force applied to the object when thrown
    public float explosionRadius; // Explosion radius for grenades and other explosive weapons
    public float fuseTime; // Time before the grenade explodes
    public int maxAmmo; // Maximum ammo capacity
    public DamageType damageType; // Type of damage dealt by the weapon
    public HandheldWeaponType handheldWeaponType;
    public HandheldWeaponStyle handheldWeaponStyle;
}

public enum DamageType
{
    Fragmentation,
    Concussion,
    Incendiary,
    None // For weapons that do not deal damage, such as flashbangs
}

public enum HandheldWeaponType
{
    Grenade,
    Flashbang,
    SmokeBomb
     // add more types here
}

public enum HandheldWeaponStyle
{
    OneHanded,
    TwoHanded
    // add more styles here
}