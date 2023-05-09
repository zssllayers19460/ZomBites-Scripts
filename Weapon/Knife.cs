using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public GameObject bloodSplatParticleEffect;
    public GameObject dirtParticleEffect;
    public GameObject concreteParticleEffect;
    public GameObject metalParticleEffect;
    public GameObject plasterSplatParticleEffect;
    public GameObject bulletFleshImpactPrefab;

    private Camera cam;
    private EquipmentManager manager;
    private Inventory inventory;
    private AudioSource source;
    private Shooting shooting;

    private void Start()
    {
        GetReferences();
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
                    SpawnBloodSplatParticle(hit.point, hit.normal);

                    Instantiate(bulletFleshImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform.Find("ZombieF_root"));
                    currentWeapon.weaponAudioSO.PlayKnifeStabClip(source);
                }
            }
            if (hit.transform.tag == "Dirt")
            {
                SpawnDirtParticle(hit.point, hit.normal);
            }
            if (hit.transform.tag == "Metal")
            {
                SpawnMetalParticle(hit.point, hit.normal);
            }
            if (hit.transform.tag == "Concrete")
            {
                SpawnContreteParticle(hit.point, hit.normal);
            }
            if (hit.transform.tag == "Plaster")
            {
                SpawnPlasterParticle(hit.point, hit.normal);
            }
        }
    }

    public void UseKnife()
    {
        shooting.CheckCanShoot(manager.currentlyEquippedWeapon);

        if (shooting.canShoot && shooting.canReload)
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            if (currentWeapon.weaponStyle == WeaponStyle.Melee)
            {
                if (Input.GetKeyDown(manager.useMeleeKnife))
                {
                    currentWeapon.weaponAudioSO.PlayKnifeSliceClip(source);
                }

                RaycastShoot(currentWeapon);
            }
        }
    }

    public void SpawnBloodParticles(Vector3 position, Vector3 normal)
    {
        Instantiate(bulletFleshImpactPrefab, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnBloodSplatParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(bloodSplatParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnContreteParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(concreteParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnMetalParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(metalParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnPlasterParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(plasterSplatParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        manager = GetComponent<EquipmentManager>();
        inventory = GetComponent<Inventory>();
        source = GetComponent<AudioSource>();
        shooting = GetComponent<Shooting>();
    }
}
