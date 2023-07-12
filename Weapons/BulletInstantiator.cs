using UnityEngine;

public class BulletInstantiator : MonoBehaviour
{
    private GameObject currentBullet;
    private GameObject bulletTrailPrefab; // Trail Renderer prefab

    private float destroyDelay = 8f;

    public void InstantiateBullet()
    {
        Inventory inventory = GetComponentInParent<Inventory>();
        EquipmentManager manager = GetComponentInParent<EquipmentManager>();

        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        // Instantiate bullet/projectile
        currentBullet = Instantiate(currentWeapon.bulletProjectilePrefab, manager.currentWeaponBarrel.position, Quaternion.identity);

        // Set the bullet's rotation to match the forward direction of the barrel
        currentBullet.transform.rotation = manager.currentWeaponBarrel.rotation;

        // Instantiate the Trail Renderer prefab and parent it to the bullet
        if (currentWeapon.bulletTrail != null)
        {
            bulletTrailPrefab = Instantiate(currentWeapon.bulletTrail, currentBullet.transform);
            TrailRenderer bulletTrailRenderer = bulletTrailPrefab.GetComponent<TrailRenderer>();
            if (bulletTrailRenderer != null)
            {
                bulletTrailRenderer.enabled = true;
            }
        }

        // Calculate direction towards the center of the screen
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);
        Vector3 targetDirection = ray.direction;

        // Set the bullet's rotation to face the target direction (center of the screen)
        currentBullet.transform.rotation = Quaternion.LookRotation(targetDirection);

        // Add forces to bullet towards the center of the screen
        Rigidbody bulletRigidbody = currentBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(targetDirection * currentWeapon.bulletSpeed, ForceMode.Force);

        Destroy(currentBullet, destroyDelay);
    }
}