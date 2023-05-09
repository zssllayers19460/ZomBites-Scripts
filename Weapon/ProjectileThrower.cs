using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    private Transform attackPoint;

    public void ThrowProjectile()
    {
        Inventory inventory = GetComponentInParent<Inventory>();
        EquipmentManager manager = GetComponentInParent<EquipmentManager>();
        
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);
        //Transform attackPoint = transform.parent.Find("AttackPoint");

        Transform attackPoint = transform.parent.parent.Find("AttackPoint");
        if (attackPoint == null) 
        {
            Debug.LogError("Could not find AttackPoint object.");
            return;
        }

        Rigidbody projectileInstance = Instantiate(projectilePrefab, transform.position, attackPoint.rotation);

        //projectileInstance.transform.parent = transform.parent;
        projectileInstance.AddForce(transform.forward * currentMelee.throwForce, ForceMode.Impulse);
    }
}