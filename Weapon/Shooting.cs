using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Variables
    private float lastShootTime = 0f;
    private int bulletsShot = 0;

    // Recoil Variables
    private float recoilX;
    private float recoilY;
    private float recoilZ;

    // Boolean Variables
    public bool canShoot;
    public bool canReload = true;

    // Ammo Variables
    [HideInInspector] public bool primaryMagazineIsEmpty = false;
    [HideInInspector] public bool secondaryMagazineIsEmpty = false;
    [HideInInspector] public bool meleeMagazineIsEmpty = false;
    [HideInInspector] public int primaryCurrentAmmo;
    [HideInInspector] public int primaryCurrentAmmoStorage;
    [HideInInspector] public int secondaryCurrentAmmo;
    [HideInInspector] public int secondaryCurrentAmmoStorage;
    [HideInInspector] public int meleeCurrentAmmo;
    [HideInInspector] public int meleeCurrentAmmoStorage;

    // Particles
    public GameObject zombieBloodVFX;

    // References
    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;
    private PlayerHUD hud;
    private Recoil recoil;
    private AudioSource source;
    private Hitmarker hitmarker;
    private BulletInstantiator bulletInstantiator;
    private BloodEffectController bloodEffectController;
    private ImpactEffectController impactEffectController;
    

    private void Start()
    {
        GetReferences();
        canShoot = true;
        canReload = true;
    }

    private void Update()
    {
        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);
        // Update recoil values
        recoilX = currentWeapon.recoilX;
        recoilY = currentWeapon.recoilY;
        recoilZ = currentWeapon.recoilZ;
        // Check if we are shooting the weapon
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }

        // Check if we are reloading the weapon
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload(manager.currentlyEquippedWeapon);
        }
    }

    public void RaycastShoot (Weapon currentWeapon)
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        float currentWeaponRange = currentWeapon.range;

        if(Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            //bulletInstantiator.OnCollisionEnter(new Collision());

            print(hit.transform.name);

            if(hit.transform.tag == "Enemy")
            {
                CharacterStats enemyStats = hit.transform.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeapon.damage);

                    if (hitmarker != null)
                    {
                        hitmarker.ShowHitmarker();
                    }

                    // Spawn Hit Particles
                    SpawnBloodVisualEffect(hit);
                    // Spawn Hit Particles
                    //bloodEffectController.PlayBloodEffect();
                }
            }

            if(hit.transform.tag == "Dirt")
            {
                impactEffectController.SpawnDirtParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Water")
            {
                impactEffectController.SpawnWaterParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Metal")
            {
                impactEffectController.SpawnMetalParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Concrete")
            {
                impactEffectController.SpawnConcreteParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Brick")
            {
                impactEffectController.SpawnBrickParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Wood")
            {
                impactEffectController.SpawnWoodParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Stone")
            {
                impactEffectController.SpawnStoneParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Sand")
            {
                impactEffectController.SpawnSandParticle(hit.point, hit.normal);
            }
        }
        

        if (currentWeapon.weaponStyle == WeaponStyle.Primary || currentWeapon.weaponStyle == WeaponStyle.Secondary)
        {
            Instantiate(currentWeapon.muzzleFlash, manager.currentWeaponBarrel);
        }
        else if(currentWeapon.weaponStyle == WeaponStyle.Melee)
        {
            // Do nothing or Instantiate the knife slash effect
            // Instantiate(currentWeapon.knifeSlashEffect, manager.currentWeaponBarrel);
            //Instantiate(currentWeapon.knifeSlashEffect, transform.position, transform.rotation);
        }
    }

    public void SpawnBloodVisualEffect(RaycastHit hit)
    {
        Instantiate(zombieBloodVFX, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
    }
    
    public void Shoot()
    {
        CheckCanShoot(manager.currentlyEquippedWeapon);

        if (canShoot && canReload)
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            if (currentWeapon.fireType == FireType.Semi)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    bulletInstantiator.InstantiateBullet(currentWeapon);

                    recoil.RecoilFire(recoilX, recoilY, recoilZ);

                    RaycastShoot(currentWeapon);

                    UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
                    
                    currentWeapon.weaponAudioSO.PlayShootingClip(source);
                    currentWeapon.weaponAudioSO.PlayCasingClip(source);

                }
            }

            if (currentWeapon.fireType == FireType.Auto)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (Time.time >= lastShootTime + currentWeapon.fireRate)
                    {
                        bulletInstantiator.InstantiateBullet(currentWeapon);

                        recoil.RecoilFire(recoilX, recoilY, recoilZ);

                        lastShootTime = Time.time;
                        RaycastShoot(currentWeapon);
                        UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
                        currentWeapon.weaponAudioSO.PlayShootingClip(source); // "true/false is for looping"
                        currentWeapon.weaponAudioSO.PlayCasingClip(source);
                    }
                }
            }

            if (currentWeapon.fireType == FireType.Burst)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (bulletsShot == 0)
                    {
                        StartCoroutine(BurstFireCoroutine(currentWeapon));
                    }
                }
            }

            IEnumerator BurstFireCoroutine(Weapon currentWeapon)
            {
                int shotsFired = 0;
                while (shotsFired < 3)
                {
                    if (Time.time >= lastShootTime + currentWeapon.fireRate)
                    {
                        bulletInstantiator.InstantiateBullet(currentWeapon);

                        recoil.RecoilFire(recoilX, recoilY, recoilZ);

                        RaycastShoot(currentWeapon);

                        UseAmmo((int)currentWeapon.weaponStyle, 1, 0);

                        currentWeapon.weaponAudioSO.PlayShootingClip(source);
                        currentWeapon.weaponAudioSO.PlayCasingClip(source);

                        shotsFired++;
                        lastShootTime = Time.time;
                    }
                    yield return null;
                }
                bulletsShot = 0;
            }

            if (currentWeapon.weaponType == WeaponType.Minigun)
            {
                // Instantiate Bullet prefab
                bulletInstantiator.InstantiateBullet(currentWeapon);

                recoil.RecoilFire(recoilX, recoilY, recoilZ);
            }

            if (currentWeapon.weaponType == WeaponType.Launcher)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    bulletInstantiator.InstantiateBullet(currentWeapon);

                    recoil.RecoilFire(recoilX, recoilY, recoilZ);

                    currentWeapon.weaponAudioSO.PlayRocketFireClip(source);

                    // Perform raycast to check if the bullet hits something
                    Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;
                    float currentWeaponRange = currentWeapon.range;
                    if (Physics.Raycast(ray, out hit, currentWeaponRange))
                    {
                       
                        // Play rocket explosion audio
                        currentWeapon.weaponAudioSO.PlayRocketExplosionClip(source);
                    }
                }
            }

            if (currentWeapon.weaponStyle == WeaponStyle.Melee)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // Perform raycast to check if the knife hits something
                    Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;

                    float currentWeaponRange = currentWeapon.range;

                    if (Physics.Raycast(ray, out hit, currentWeaponRange) && hit.transform.tag == "Enemy")
                    {
                        // if we hit something like a enemy then take damage
                        CharacterStats enemyStats = hit.transform.GetComponent<CharacterStats>();
                        enemyStats.TakeDamage(currentWeapon.damage);

                        // play stab audio clip
                        currentWeapon.weaponAudioSO.PlayKnifeStabClip(source);
                    }
                    else
                    {
                        // play slice audio clip
                        currentWeapon.weaponAudioSO.PlayKnifeSliceClip(source);
                    }
                }
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);
            currentWeapon.weaponAudioSO.PlayDryFireClip(source);

            // if holding a rocket then play the rocket dry fire clip
            if(currentWeapon.weaponType == WeaponType.Launcher)
            {
                currentWeapon.weaponAudioSO.PlayRocketDryFireClip(source);
            }
            // Debug.Log("No Ammo");
        }
    }

    /*private void BulletProjectile()
    {
        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);
        
        if (currentWeapon.weaponStyle == WeaponStyle.Primary || currentWeapon.weaponStyle == WeaponStyle.Secondary)
        {
            GameObject bullet = Instantiate(currentWeapon.bulletProjectilePrefab, transform.root);
            bullet.transform.position = manager.currentWeaponBarrel.position;
            bullet.transform.rotation = manager.currentWeaponBarrel.rotation;
            Rigidbody bulletRigidbody = bullet.GetComponentInChildren<Rigidbody>();
            if (bulletRigidbody != null)
            {
                // Add debug ray
                Debug.DrawRay(bullet.transform.position, bullet.transform.forward * currentWeapon.bulletSpeed, Color.red, 5f);
                bulletRigidbody.AddForce(bullet.transform.forward * currentWeapon.bulletSpeed);
            }
        }
        else
        {
            // do nothing, as melee weapons don't use bullets
        }
    }*/

    private void UseAmmo(int slot, int currentAmmoUsed, int currentStoredAmmoUsed)
    {
        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        if(slot == 0)
        {
            if(primaryCurrentAmmo <= 0)
            {
                primaryMagazineIsEmpty = true;
                CheckCanShoot(slot);
            }
            else
            {
                primaryCurrentAmmo -= currentAmmoUsed;
                primaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                hud.UpdateWeaponAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage);
            }
            if(primaryCurrentAmmo < inventory.GetItem(0).magazineSize / 2.5)
            {
                currentWeapon.weaponAudioSO.PlayLastBulletClip(source);
            }
        }

        if(slot == 1)
        {
            if(secondaryCurrentAmmo <= 0)
            {
                secondaryMagazineIsEmpty = true;
                CheckCanShoot(slot);
            }
            else
            {
                secondaryCurrentAmmo -= currentAmmoUsed;
                secondaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                hud.UpdateWeaponAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
            }
            if(secondaryCurrentAmmo < inventory.GetItem(1).magazineSize / 2.5)
            {
                currentWeapon.weaponAudioSO.PlayLastBulletClip(source);
            }
        }

        if(slot == 2)
        {
            if(meleeCurrentAmmo <= 0)
            {
                meleeMagazineIsEmpty = true;
                CheckCanShoot(slot);
            }
            else
            {
                meleeCurrentAmmo -= currentAmmoUsed;
                meleeCurrentAmmoStorage -= currentStoredAmmoUsed;
                hud.UpdateWeaponAmmoUI(meleeCurrentAmmo, meleeCurrentAmmoStorage);
            }
            if(meleeCurrentAmmo < inventory.GetItem(2).magazineSize / 2.5)
            {
                currentWeapon.weaponAudioSO.PlayLastBulletClip(source);
            }
        }
    }

    public void AddAmmo(int slot, int currentAmmoAdded, int currentStoredAmmoAdded)
    {
        if(slot == 0)
        {
        
            primaryCurrentAmmo += currentAmmoAdded;
            primaryCurrentAmmoStorage += currentStoredAmmoAdded;
            hud.UpdateWeaponAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage);
        }

        if(slot == 1)
        {
        
            secondaryCurrentAmmo += currentAmmoAdded;
            secondaryCurrentAmmoStorage += currentStoredAmmoAdded;
            hud.UpdateWeaponAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
        }

        if(slot == 2)
        {
            meleeCurrentAmmo += currentAmmoAdded;
            meleeCurrentAmmoStorage += currentStoredAmmoAdded;
            hud.UpdateWeaponAmmoUI(meleeCurrentAmmo, meleeCurrentAmmoStorage);
        }
    }

    private void Reload(int slot)
    {
        if(slot == 0)
        {   
            int ammoToReload = inventory.GetItem(0).magazineSize - primaryCurrentAmmo;
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            // If we have enough ammo to reload the magazine
            if(primaryCurrentAmmoStorage >= ammoToReload)
            {
                CheckCanShoot(slot);
                // If current magazine is full, skip reloading
                if(primaryCurrentAmmo == inventory.GetItem(0).magazineSize)
                {
                    return;
                }

                // Reload
                AddAmmo(slot, ammoToReload, 0);
                UseAmmo(slot, 0, ammoToReload);

                // Reload sound
                currentWeapon.weaponAudioSO.PlayReloadClip(source);
                currentWeapon.weaponAudioSO.PlayRocketReloadClip(source);

                primaryMagazineIsEmpty = false;
                
            }
            else
            {
                //Debug.Log("Not enough ammo to reload");
            }
        }

        if(slot == 1)
        {
            int ammoToReload = inventory.GetItem(1).magazineSize - secondaryCurrentAmmo;
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            // If we have enough ammo to reload the magazine
            if(secondaryCurrentAmmoStorage >= ammoToReload)
            {
                CheckCanShoot(slot);
                // If current magazine is full, skip reloading
                if(secondaryCurrentAmmo == inventory.GetItem(1).magazineSize)
                {
                    return;
                }

                // Reload
                AddAmmo(slot, ammoToReload, 0);
                UseAmmo(slot, 0, ammoToReload);

                // Reload sound
                currentWeapon.weaponAudioSO.PlayReloadClip(source);
                currentWeapon.weaponAudioSO.PlayRocketReloadClip(source);

                secondaryMagazineIsEmpty = false;
            }
            else
            {
                //Debug.Log("Not enough ammo to reload");
            }
        }

        /*if (slot == 2)
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);
            if (currentWeapon.weaponStyle != WeaponStyle.Melee) // Skip reloading if it's a melee weapon
            {
                int ammoToReload = inventory.GetItem(2).magazineSize - meleeCurrentAmmo;
            }
        }*/

        if(slot == 2)
        {
            int ammoToReload = inventory.GetItem(2).magazineSize - meleeCurrentAmmo;
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            // If we have enough ammo to reload the magazine
            if(meleeCurrentAmmoStorage >= ammoToReload)
            {
                CheckCanShoot(slot);
                // If current magazine is full, skip reloading
                if(meleeCurrentAmmo == inventory.GetItem(2).magazineSize)
                {
                    return;
                }
            
                // Reload
                AddAmmo(slot, ammoToReload, 0);
                UseAmmo(slot, 0, ammoToReload);

                // Reload sound
                currentWeapon.weaponAudioSO.PlayReloadClip(source);
                currentWeapon.weaponAudioSO.PlayRocketReloadClip(source);

                meleeMagazineIsEmpty = false;
            }
            else
            {
                //Debug.Log("Not enough ammo to reload");
            }
        }
    }

    public void CheckCanShoot(int slot)
    {
        if(slot == 0)
        {
            if(primaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
        
        if(slot == 1)
        {
            if(secondaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
        if(slot == 2)
        {
            if(secondaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
    }

    public void InitAmmo(int slot, Weapon weapon)
    {
        if(slot == 0)
        {
            primaryCurrentAmmo = primaryCurrentAmmo = weapon.magazineSize;
            primaryCurrentAmmoStorage = weapon.storedAmmo;
        }
        if(slot == 1)
        {
            secondaryCurrentAmmo = secondaryCurrentAmmo = weapon.magazineSize;
            secondaryCurrentAmmoStorage = weapon.storedAmmo;
        }
        if(slot == 2)
        {
            meleeCurrentAmmo = meleeCurrentAmmo = weapon.magazineSize;
            meleeCurrentAmmoStorage = weapon.storedAmmo;
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        hud = GetComponent<PlayerHUD>();
        recoil = GetComponentInChildren<Recoil>();
        source = GetComponent<AudioSource>();
        hitmarker = GetComponentInChildren<Hitmarker>();
        bulletInstantiator = GetComponentInChildren<BulletInstantiator>();
        bloodEffectController = GetComponentInParent<BloodEffectController>();
        impactEffectController = GetComponentInParent<ImpactEffectController>();
    }
}