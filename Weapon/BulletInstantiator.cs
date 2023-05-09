using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstantiator : MonoBehaviour
{
    //public float bulletSpeed = 10f; // The speed of the bullet

    // References
    private Inventory inventory;
    private EquipmentManager manager;

    private void Start()
    {
        GetReferences();
    }

    public void InstantiateBullet(Weapon currentWeapon)
    {
        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(currentWeapon.bulletProjectilePrefab, manager.currentWeaponBarrel.position, Quaternion.identity);

        //Add forces to bullet
        Rigidbody bulletRigidbody = currentBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(manager.currentWeaponBarrel.forward * currentWeapon.range * currentWeapon.bulletSpeed, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void GetReferences()
    {
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
    }
}