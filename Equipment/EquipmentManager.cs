using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquippedWeapon = 2;
    [HideInInspector] public GameObject currentWeaponObject = null;
    [HideInInspector] public Transform currentWeaponBarrel = null;
    public KeyCode equipPrimary = KeyCode.Alpha1;
    public KeyCode equipSecondary = KeyCode.Alpha2;
    public KeyCode equipMelee = KeyCode.V;

    public Transform WeaponHolder = null;
    public Weapon defaultWeapon = null;
    public Weapon knife = null;

    private Weapon previousWeapon;

    private PlayerHUD hud;
    private Camera cam;
    private Inventory inventory;
    private Animator anim;
    private AudioSource source;
    private Shooting shooting;

    private void Start()
    {
        GetReferences();
        inventory.InitVariables(); // Call InitVariables() in Inventory
        InitVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(equipPrimary) && inventory.GetItem(0) != null && currentlyEquippedWeapon != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }
        if (Input.GetKeyDown(equipSecondary) && inventory.GetItem(1) != null && currentlyEquippedWeapon != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(1));
        }
        if (Input.GetKeyDown(equipMelee) && inventory.GetItem(2) != null && currentlyEquippedWeapon != 2)
        {
            StorePreviousWeapon();
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(2));
            UseKnife();
            UnequipWeapon();
            EquipPreviousWeapon();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        StorePreviousWeapon();
        currentlyEquippedWeapon = (int)weapon.weaponStyle;
        currentWeaponObject = Instantiate(weapon.weaponPrefab, WeaponHolder);
        currentWeaponBarrel = currentWeaponObject.transform.Find("Barrel");
        hud.UpdateWeaponUI(weapon);
    }

    public void UnequipWeapon()
    {
        // Destroy the current weapon object
        Destroy(currentWeaponObject);
    }

    public void UseKnife()
    {
        shooting.CheckCanShoot(currentlyEquippedWeapon);

        if (shooting.canShoot && shooting.canReload)
        {
            Weapon currentWeapon = inventory.GetItem(currentlyEquippedWeapon);

            if (currentWeapon.weaponStyle == WeaponStyle.Melee)
            {
                if (Input.GetKeyDown(equipMelee))
                {
                    currentWeapon.weaponAudioSO.PlayKnifeSliceClip(source);
                }

                //recoil.RecoilFire(recoilX, recoilY, recoilZ);

                //BulletProjectile();

                RaycastShoot(currentWeapon);

                //UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
            }
        }
    }

    public void RaycastShoot(Weapon currentWeapon)
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        float currentWeaponRange = currentWeapon.range;

        if (Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            print(hit.transform.name);

            if (hit.transform.tag == "Enemy")
            {
                CharacterStats enemyStats = hit.transform.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeapon.damage);
                    // Spawn Hit Particles
                    shooting.SpawnBloodParticles(hit.point, hit.normal);

                    Instantiate(shooting.bulletFleshImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform.Find("ZombieF_root"));
                    currentWeapon.weaponAudioSO.PlayKnifeStabClip(source);
                }
            }
            if (hit.transform.tag == "Dirt")
            {
                SpawnDirtParticle(hit.point, hit.normal);
            }
        }
    }

    private void StorePreviousWeapon()
    {
        previousWeapon = inventory.GetItem(currentlyEquippedWeapon);
    }

    public void EquipPreviousWeapon()
    {
        if (previousWeapon != null)
        {
            UnequipWeapon();
            EquipWeapon(previousWeapon);
        }
    }

    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(shooting.dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void InitVariables()
    {
        inventory.AddItem(defaultWeapon);
        inventory.AddItem(knife);
        EquipWeapon(inventory.GetItem(1));
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        hud = GetComponent<PlayerHUD>();
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        shooting = GetComponent<Shooting>();
    }
}



/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquippedWeapon = 2;
    [HideInInspector] public GameObject currentWeaponObject = null;
    [HideInInspector] public Transform currentWeaponBarrel = null;
    public KeyCode equipPrimary = KeyCode.Alpha1;
    public KeyCode equipSecondary = KeyCode.Alpha2;
    public KeyCode equipMelee = KeyCode.V;

    public Transform WeaponHolder = null;
    public Weapon defaultWeapon = null;
    public Weapon knife = null;

    private Weapon previousWeapon;

    private PlayerHUD hud;
    private Camera cam;
    private Inventory inventory;
    private Animator anim;
    private AudioSource source;
    private Shooting shooting;

    private void Start()
    {
        GetReferences();
        inventory.InitVariables(); // Call InitVariables() in Inventory
        InitVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(equipPrimary) && inventory.GetItem(0) != null && currentlyEquippedWeapon != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }
        if (Input.GetKeyDown(equipSecondary) && inventory.GetItem(1) != null && currentlyEquippedWeapon != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(1));
        }
        if (Input.GetKeyDown(equipMelee) && inventory.GetItem(2) != null && currentlyEquippedWeapon != 2)
        {
            StorePreviousWeapon();
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(2));
            UseKnife();
            UnequipWeapon();
            EquipPreviousWeapon();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        StorePreviousWeapon();
        currentlyEquippedWeapon = (int)weapon.weaponStyle;
        currentWeaponObject = Instantiate(weapon.weaponPrefab, WeaponHolder);
        currentWeaponBarrel = currentWeaponObject.transform.Find("Barrel");
        hud.UpdateWeaponUI(weapon);
	}

    public void UnequipWeapon()
    {
        // Destroy the current weapon object
        Destroy(currentWeaponObject);
    }

    public void UseKnife()
    {
        shooting.CheckCanShoot(currentlyEquippedWeapon);

        if (shooting.canShoot && shooting.canReload)
        {
            Weapon currentWeapon = inventory.GetItem(currentlyEquippedWeapon);

            if (currentWeapon.weaponStyle == WeaponStyle.Melee)
            {
                if (Input.GetKeyDown(equipMelee))
                {
                    currentWeapon.weaponAudioSO.PlayKnifeSliceClip(source);
                }
                
                //recoil.RecoilFire(recoilX, recoilY, recoilZ);

                //BulletProjectile();

                RaycastShoot(currentWeapon);

                //UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
            }
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
                    // Spawn Hit Particles
                    shooting.SpawnBloodParticles(hit.point, hit.normal);

                    Instantiate(shooting.bulletFleshImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform.Find("ZombieF_root"));
                    currentWeapon.weaponAudioSO.PlayKnifeStabClip(source);
                }
            }
            if(hit.transform.tag == "Dirt")
            {
                SpawnDirtParticle(hit.point, hit.normal);
            }
        }
    }

    private void StorePreviousWeapon()
    {
        previousWeapon = inventory.GetItem(currentlyEquippedWeapon);
    }

    public void EquipPreviousWeapon()
    {
        if (previousWeapon != null)
        {
            UnequipWeapon();
            EquipWeapon(previousWeapon);
        }
    }

    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(shooting.dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void InitVariables()
	{
    	inventory.AddItem(defaultWeapon);
        inventory.AddItem(knife);
    	EquipWeapon(inventory.GetItem(1));
	}

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        hud = GetComponent<PlayerHUD>();
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        shooting = GetComponent<Shooting>();
    }
} */