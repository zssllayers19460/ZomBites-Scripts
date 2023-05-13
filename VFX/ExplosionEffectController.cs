using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectController : MonoBehaviour
{
    // References
    private Inventory inventory;
    private EquipmentManager manager;

    private void Start()
    {
        GetReferences();
    }

    public void ExplosionEffect()
    {
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        GameObject[] grenades = GameObject.FindGameObjectsWithTag("Grenade");
        foreach (GameObject grenade in grenades)
        {
            Instantiate(currentMelee.explosionEffect, grenade.transform.position, grenade.transform.rotation);
            Destroy(grenade);
            break;
        }
    }

    private void GetReferences()
    {
        inventory = GetComponentInChildren<Inventory>();
        manager = GetComponentInChildren<EquipmentManager>();
    }
}
