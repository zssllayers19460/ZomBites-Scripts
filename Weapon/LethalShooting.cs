using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalShooting : MonoBehaviour
{
    // Variables
    public Transform attackPoint;
    public new Transform camera;

    // Boolean Variables
    public bool canThrow;

    // References
    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;
    private Rigidbody grenadeRb;
    private ProjectileThrower projectileThrower;
    private AudioSource source;

    private void Start()
    {
        GetReferences();
        canThrow = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        if (canThrow && currentMelee.handHeldStyle == HandHeldStyle.FragGrenade)
        {
            projectileThrower.ThrowProjectile();

            // Start the coroutine to explode the grenade
            StartCoroutine(ExplodeGrenade(currentMelee));
        }
    }

    private IEnumerator ExplodeGrenade(Melee currentMelee)
    {
        // Wait for the explosion delay
        yield return new WaitForSeconds(currentMelee.explosionDelay);

        // Explode the grenade at the position of the grenade
        Explode(projectileThrower.transform.position, currentMelee);
    }

    public void Explode(Vector3 explosionPosition, Melee currentMelee)
    {
        // Instantiate the explosion effect at the position of the grenade   *EDIT  which it doesnt
        GameObject explosion = Instantiate(currentMelee.explosionEffect, explosionPosition, Quaternion.identity);

        // Destroy the explosion effect after a short delay
        Destroy(explosion, currentMelee.explosionDelay);

        currentMelee.meleeAudioSO.PlayExplosionSound(source);

        // Apply damage to enemies within the explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, currentMelee.explosionRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                CharacterStats enemyStats = collider.transform.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentMelee.damage);
                }
            }
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        grenadeRb = GetComponentInChildren<Rigidbody>();
        projectileThrower = GetComponentInChildren<ProjectileThrower>();
        source = GetComponent<AudioSource>();

    }
}