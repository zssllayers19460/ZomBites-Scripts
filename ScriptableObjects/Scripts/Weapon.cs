using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Add a New Weapon...")]
public class Weapon : Item
{
    public GameObject weaponPrefab;
    public GameObject muzzleFlash;
    public GameObject bulletTrail;       // New Varible
    public GameObject bulletProjectilePrefab;
    public GameObject knifeSlashEffect;
    private Recoil recoil;
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float aimingRecoilX;
    public float aimingRecoilY;
    public float aimingRecoilZ;
    public int magazineSize;
    public int storedAmmo;
    public float range;
    public int damage;      // this is the damage varible i need to get, add up 
    public int headShotDamage;
    public float fireRate;
    public float bulletSpeed;
    public float aimingSpread;
    public float spread;
    public WeaponType weaponType;
    public WeaponStyle weaponStyle;
    public FireType fireType;

    private void OnEnable()
    {
        recoil = weaponPrefab.GetComponentInParent<Recoil>();
    }

    public void RecoilFire()
    {
        if (weaponType == WeaponType.AssaultRifle || weaponType == WeaponType.SubMachineGun || weaponType == WeaponType.LightMachineGun || weaponType == WeaponType.Shotgun || weaponType == WeaponType.Sniper || weaponType == WeaponType.Pistol || weaponType == WeaponType.Launcher || weaponType == WeaponType.Minigun)
        {
            if (recoil != null)
            {
                recoil.RecoilFire();
            }
        }
        else if (weaponType == WeaponType.Melee)
        {
            // do nothing for melee weapons
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