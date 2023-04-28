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
    public GameObject dirtParticleEffect = null;
    public GameObject waterParticleEffect = null;
    public GameObject brickParticleEffect = null;
    public GameObject concreteParticleEffect = null;
    public GameObject metalParticleEffect = null;
    public GameObject woodParticleEffect = null;
    public GameObject stoneParticleEffect = null;
    public GameObject sandParticleEffect = null;
    public GameObject bulletFleshImpactPrefab = null;

    // References
    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;
    private PlayerHUD hud;
    private Recoil recoil;
    private AudioSource source;
    private Hitmarker hitmarker;
    private BulletInstantiator bulletInstantiator;
    

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
                    SpawnBloodParticles(hit.point, hit.normal);

                    Instantiate(bulletFleshImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform.Find("ZombieF_root"));
                }
            }

            if(hit.transform.tag == "Dirt")
            {
                SpawnDirtParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Water")
            {
                SpawnWaterParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Metal")
            {
                SpawnMetalParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Concrete")
            {
                SpawnConcreteParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Brick")
            {
                SpawnBrickParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Wood")
            {
                SpawnWoodParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Stone")
            {
                SpawnStoneParticle(hit.point, hit.normal);
            }
            if(hit.transform.tag == "Sand")
            {
                SpawnSandParticle(hit.point, hit.normal);
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

    public void SpawnBloodParticles(Vector3 position, Vector3 normal)
    {
        Instantiate(bulletFleshImpactPrefab, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnWaterParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(waterParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnMetalParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(metalParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnBrickParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(brickParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnConcreteParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(concreteParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnWoodParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(woodParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnStoneParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(stoneParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnSandParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(sandParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
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

                recoil.RecoilFire(recoilX, recoilY, recoilZ);
            }

            if (currentWeapon.weaponType == WeaponType.Launcher)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    recoil.RecoilFire(recoilX, recoilY, recoilZ);

                    bulletInstantiator.InstantiateBullet(manager.currentWeaponBarrel);

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
                CheckCanShoot(manager.currentlyEquippedWeapon);
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
                CheckCanShoot(manager.currentlyEquippedWeapon);
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
                CheckCanShoot(manager.currentlyEquippedWeapon);
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
                CheckCanShoot(slot);
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
                CheckCanShoot(slot);
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
                CheckCanShoot(slot);
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
    }
}