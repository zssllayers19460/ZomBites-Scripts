using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LethalShooting : MonoBehaviour
{
    // Variables
    public Transform attackPoint;
    public new Transform camera;
    private RaycastHit hit;
    public bool lethalIsEmpty = false;
    public bool nonLethalIsEmpty = false;
    public int currentLethalAmmo;
    public int currentLethalAmmoStored;
    public int currentNonLethalAmmo;
    public int currentNonLethalAmmoStored;

    // Boolean Variables
    public bool canThrow;
    public bool canReload;

    // UI References
    [SerializeField] private Image grenadeUI;
    [SerializeField] private Image grenadeImage;
    [SerializeField] private TextMeshProUGUI grenadeText;

    // References
    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;
    private ProjectileThrower projectileThrower;
    private AudioSource source;
    private WeaponPickup weaponPickup;
    private PlayerHUD hud;

    private void Start()
    {
        GetReferences();
        canThrow = true;
        canReload = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ThrowGrenade();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadMelee(0);
        }

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Grenade") && Vector3.Distance(transform.position, hit.transform.position) <= weaponPickup.pickupRange)
        {
            grenadeUI.gameObject.SetActive(true);
            grenadeImage.gameObject.SetActive(true);
            grenadeText.gameObject.SetActive(true);
            grenadeText.text = "Press E to throw grenade";

            // *DEBUGGING ONLY
            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickupGrenade();
            }
        }
        else
        {
            grenadeUI.gameObject.SetActive(false);
            grenadeImage.gameObject.SetActive(false);
            grenadeText.gameObject.SetActive(false);
        }
    }

    public void ThrowGrenade()
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        if (canThrow && canReload && currentMelee.handHeldStyle == HandHeldStyle.FragGrenade)
        {
            projectileThrower.ThrowProjectile();

            StartCoroutine(ExplodeGrenade(currentMelee));
            UseAmmo((int)currentMelee.handHeldType, 1, 0);
        }
    }

    private IEnumerator ExplodeGrenade(Melee currentMelee)
    {
        yield return new WaitForSeconds(currentMelee.explosionDelay);

        Explode(currentMelee);
    }

    public void Explode(Melee currentMelee)
    {
        ExplosionEffectController explosionEffectController = GetComponentInParent<ExplosionEffectController>();
        explosionEffectController.ExplosionEffect();

        currentMelee.meleeAudioSO.PlayExplosionSound(source);

        GameObject grenade = GameObject.FindWithTag("Grenade");
        if (grenade != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(grenade.transform.position, currentMelee.explosionRadius);
            foreach (Collider collider in hitColliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Apply explosion force to the object
                    rb.AddExplosionForce(currentMelee.explosionForce, grenade.transform.position, currentMelee.explosionRadius);
                }

                if (collider.CompareTag("Enemy"))
                {
                    CharacterStats enemyStats = collider.transform.GetComponent<CharacterStats>();
                    if (enemyStats != null)
                    {
                        // Calculate the distance from the explosion center
                        float distance = Vector3.Distance(collider.transform.position, grenade.transform.position);

                        // Calculate the damage based on the distance
                        float normalizedDistance = distance / currentMelee.explosionRadius;
                        float damageMultiplier = 1f - normalizedDistance;

                        // Apply the damage
                        int damage = Mathf.RoundToInt(currentMelee.damage * damageMultiplier);
                        enemyStats.TakeDamage(damage);
                    }
                }
                else if (collider.CompareTag("Player"))
                {
                    CharacterStats playerStats = collider.transform.GetComponent<CharacterStats>();
                    if (playerStats != null)
                    {
                        // Calculate the distance from the explosion center
                        float distance = Vector3.Distance(collider.transform.position, grenade.transform.position);

                        // Calculate the damage based on the distance
                        float normalizedDistance = distance / currentMelee.explosionRadius;
                        float damageMultiplier = 1f - normalizedDistance;

                        // Apply the damage
                        int damage = Mathf.RoundToInt(currentMelee.damage * damageMultiplier);
                        playerStats.TakeDamage(damage);
                    }
                }
            }
            Destroy(grenade);
        }
        // Display UI for grenade
        /*grenadeImage.gameObject.SetActive(true);
        grenadeUI.gameObject.SetActive(true);
        grenadeText.gameObject.SetActive(true);
        grenadeText.text = "Press E to Throw Grenade";*/
    }

    private void PickupGrenade()
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        if (currentMelee != null && currentMelee.handHeldStyle == HandHeldStyle.FragGrenade)
        {
            // Add the grenade back to the inventory
            inventory.AddMeleeItem(currentMelee);

            // Destroy the grenade from the game view
            Destroy(hit.transform.gameObject);
        }
    }

    // Drawing Explosion Radius *DEBUGGING ONLY 
    /*private void OnDrawGizmos()
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);
        VisualizeExplosionRadius(transform.position, currentMelee.explosionRadius);
    }

    private void VisualizeExplosionRadius(Vector3 center, float radius)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }*/

    private void UseAmmo(int slot, int currentLethalAmmoUsed, int currentStoredLethalAmmoUsed)
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        if (slot == 0)
        {
            if (currentLethalAmmo <= 0)
            {
                lethalIsEmpty = true;
                CheckCanThrow(slot);
            }
            else
            {
                currentLethalAmmo -= currentLethalAmmoUsed;
                currentLethalAmmoStored -= currentStoredLethalAmmoUsed;
                hud.UpdateWeaponAmmoUI(currentLethalAmmo, currentLethalAmmoStored);
            }
            if (currentLethalAmmo < inventory.GetMeleeItem(0).grenadeToHold)
            {
                //currentMelee.weaponAudioSO.PlayLastBulletClip(source);
            }
        }

        if (slot == 1)
        {
            if (currentNonLethalAmmo <= 0)
            {
                nonLethalIsEmpty = true;
                CheckCanThrow(slot);
            }
            else
            {
                currentNonLethalAmmo -= currentLethalAmmoUsed;
                currentNonLethalAmmoStored -= currentStoredLethalAmmoUsed;
                hud.UpdateWeaponAmmoUI(currentNonLethalAmmo, currentNonLethalAmmoStored);
            }
            if (currentNonLethalAmmo < inventory.GetMeleeItem(0).grenadeToHold)
            {
                //currentMelee.weaponAudioSO.PlayLastBulletClip(source);
            }
        }
    }

    public void AddAmmo(int slot, int currentLethalAmmoAdded, int currentStoredLethalAmmoAdded)
    {
        if (slot == 0)
        {

            currentLethalAmmo += currentLethalAmmoAdded;
            currentLethalAmmoStored += currentStoredLethalAmmoAdded;
            hud.UpdateWeaponAmmoUI(currentLethalAmmo, currentLethalAmmoStored);
        }

        if (slot == 1)
        {

            currentNonLethalAmmo += currentLethalAmmoAdded;
            currentNonLethalAmmoStored += currentStoredLethalAmmoAdded;
            hud.UpdateWeaponAmmoUI(currentNonLethalAmmo, currentNonLethalAmmoStored);
        }
    }

    private void ReloadMelee(int slot)
    {
        if (slot == 0)
        {
            int meleeAmmoToReload = inventory.GetMeleeItem(0).grenadeToHold - currentLethalAmmo;
            Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

            // If we have enough ammo to reload the magazine
            if (currentLethalAmmoStored >= meleeAmmoToReload)
            {
                CheckCanThrow(slot);
                // If current magazine is full, skip reloading
                if (currentLethalAmmo == inventory.GetMeleeItem(0).grenadeToHold)
                {
                    return;
                }

                // Reload
                AddAmmo(slot, meleeAmmoToReload, 0);
                UseAmmo(slot, 0, meleeAmmoToReload);

                // Reload sound
                //currentMelee.weaponAudioSO.PlayReloadClip(source);
                //currentMelee.weaponAudioSO.PlayRocketReloadClip(source);

                lethalIsEmpty = false;

            }
            else
            {
                //Debug.Log("Not enough ammo to reload");
            }
        }

        if (slot == 1)
        {
            int meleeAmmoToReload = inventory.GetMeleeItem(1).grenadeToHold - currentNonLethalAmmo;
            Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

            // If we have enough ammo to reload the magazine
            if (currentNonLethalAmmoStored >= meleeAmmoToReload)
            {
                CheckCanThrow(slot);
                // If current magazine is full, skip reloading
                if (currentNonLethalAmmo == inventory.GetMeleeItem(1).grenadeToHold)
                {
                    return;
                }

                // Reload
                AddAmmo(slot, meleeAmmoToReload, 0);
                UseAmmo(slot, 0, meleeAmmoToReload);

                // Reload sound
                //currentMelee.weaponAudioSO.PlayReloadClip(source);
                //currentMelee.weaponAudioSO.PlayRocketReloadClip(source);

                nonLethalIsEmpty = false;
            }
            else
            {
                //Debug.Log("Not enough ammo to reload");
            }
        }
    }

    public void CheckCanThrow(int slot)
    {
        if(slot == 0)
        {
            if(lethalIsEmpty)
                canThrow = false;
            else
                canThrow = true;
        }
        
        if(slot == 1)
        {
            if(nonLethalIsEmpty)
                canThrow = false;
            else
                canThrow = true;
        }
    }

    public void InitAmmo(int slot, Melee melee)
    {
        if (slot == 0)
        {
            currentLethalAmmo = currentLethalAmmo = melee.grenadeToHold;
            currentLethalAmmoStored = melee.grenadesInInventory;
        }
        if (slot == 1)
        {
            currentNonLethalAmmo = currentNonLethalAmmo = melee.grenadeToHold;
            currentNonLethalAmmoStored = melee.grenadesInInventory;
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        projectileThrower = GetComponentInChildren<ProjectileThrower>();
        source = GetComponent<AudioSource>();
        weaponPickup = GetComponent<WeaponPickup>();
        hud = GetComponent<PlayerHUD>();
    }
}