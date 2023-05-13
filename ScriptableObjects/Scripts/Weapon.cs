using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add a New Weapon...")]
public class Weapon : Item
{
    public GameObject weaponPrefab; // Prefab to the weapon itself
    public GameObject muzzleFlash;
    public GameObject bulletProjectilePrefab;
    public GameObject knifeSlashEffect;
    public WeaponAudioSO weaponAudioSO;
    private Recoil recoil;
    public float recoilX; // Define recoilX variable
    public float recoilY; // Define recoilY variable
    public float recoilZ; // Define recoilZ variable
    public int magazineSize;  // Bullets in magazine
    public int storedAmmo; // bullets in stock
    public float range; // Weapon range
    public int damage;
    public float fireRate; // Weapon rate of fire
    public float bulletSpeed;
    public WeaponType weaponType; // Weapon types
    public WeaponStyle weaponStyle; // Weapon style
    public FireType fireType;

    private void OnEnable()
    {
        recoil = weaponPrefab.GetComponentInParent<Recoil>(); // Modified this line
    }

    public void RecoilFire()
    {
        if (weaponType == WeaponType.AssaultRifle || weaponType == WeaponType.SubMachineGun || weaponType == WeaponType.LightMachineGun || weaponType == WeaponType.Shotgun || weaponType == WeaponType.Sniper || weaponType == WeaponType.Pistol || weaponType == WeaponType.Launcher || weaponType == WeaponType.Minigun)
        {
            recoil.RecoilFire(recoilX, recoilY, recoilZ);
        }
        else if (weaponType == WeaponType.Melee)
        {
            // do nothing for melee weapons you dont want recoil for melee weapons, just think about it
        }
    } 
}

public enum WeaponType
{
    AssaultRifle,
    SubMachineGun,
    LightMachineGun,
    Shotgun,
    Sniper,
    Pistol,
    Launcher,
    Minigun,
    Melee
}

public enum WeaponStyle
{
    // Primary = 0
    Primary,
    // Secondary = 1
    Secondary,
    // Melee = 2
    Melee
}

public enum FireType
{
    Auto,
    Semi,
    Burst
}