using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    //public float delay = 0.35f;
    private Transform attackPoint;

    public void ThrowProjectile()
    {
        Inventory inventory = GetComponentInParent<Inventory>();
        EquipmentManager manager = GetComponentInParent<EquipmentManager>();
        AudioSource source = GetComponentInParent<AudioSource>();

        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);
        Transform attackPoint = transform.parent.parent.Find("AttackPoint");
        if (attackPoint == null)
        {
            Debug.LogError("Could not find AttackPoint object.");
            return;
        }

        Rigidbody projectileInstance = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);

        Vector3 throwDirection = attackPoint.forward;
        projectileInstance.AddForce(throwDirection * currentMelee.throwForce, ForceMode.Impulse);
    
        //StartCoroutine(DelayedEquipPreviousWeapon(manager));
    }

    /*private IEnumerator DelayedEquipPreviousWeapon(EquipmentManager manager)
    {
        yield return new WaitForSeconds(delay);
        manager.EquipPreviousWeapon();
        Destroy(gameObject);
    }*/
}