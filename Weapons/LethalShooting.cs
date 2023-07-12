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
    private int currentAmmo;

    // Boolean Variables
    public bool canThrow;
    public bool canReload;

    // UI References
    [SerializeField] private Image grenadeUI;
    [SerializeField] private Image grenadeImage;
    [SerializeField] private TextMeshProUGUI grenadeText;
    [SerializeField] private Image flashbangOverlayImage;

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

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadMelee();
        }*/

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
        }
        else if (canThrow && canReload && currentMelee.handHeldStyle == HandHeldStyle.FlashbangGrenade)
        {
            projectileThrower.ThrowProjectile();

            StartCoroutine(ExplodeGrenade(currentMelee));
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

        GameObject flashbang = GameObject.FindWithTag("Flashbang");
        if (flashbang != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(flashbang.transform.position, currentMelee.explosionRadius);
            foreach (Collider collider in hitColliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Apply explosion force to the object
                    rb.AddExplosionForce(currentMelee.explosionForce, flashbang.transform.position, currentMelee.explosionRadius);
                    flashbangOverlayImage.gameObject.SetActive(true);
                }

                if (collider.CompareTag("Enemy"))
                {
                    CharacterStats enemyStats = collider.transform.GetComponent<CharacterStats>();
                    if (enemyStats != null)
                    {
                        // Calculate the distance from the explosion center
                        float distance = Vector3.Distance(collider.transform.position, flashbang.transform.position);

                        // Calculate the damage based on the distance
                        float normalizedDistance = distance / currentMelee.explosionRadius;
                        float damageMultiplier = 1f - normalizedDistance;

                        // Apply the damage
                        int damage = Mathf.RoundToInt(currentMelee.damage * damageMultiplier);
                        enemyStats.TakeDamage(damage);
                        flashbangOverlayImage.gameObject.SetActive(true);
                        // Stun the enemy or something for better effect on zombies
                    }
                }
                else if (collider.CompareTag("Player"))
                {
                    CharacterStats playerStats = collider.transform.GetComponent<CharacterStats>();
                    if (playerStats != null)
                    {
                        // Calculate the distance from the explosion center
                        float distance = Vector3.Distance(collider.transform.position, flashbang.transform.position);

                        // Calculate the damage based on the distance
                        float normalizedDistance = distance / currentMelee.explosionRadius;
                        float damageMultiplier = 1f - normalizedDistance;

                        // Apply the damage
                        int damage = Mathf.RoundToInt(currentMelee.damage * damageMultiplier);
                        playerStats.TakeDamage(damage);
                        flashbangOverlayImage.gameObject.SetActive(true);
                        // for player just make the screen mostly white for a while and maybe some other things
                    }
                }
            }
            flashbangOverlayImage.gameObject.SetActive(false);
            Destroy(flashbang);
        }

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