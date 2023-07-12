using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectController : MonoBehaviour
{
    public void ExplosionEffect()
    {
        Inventory inventory = GetComponentInChildren<Inventory>();
        EquipmentManager manager = GetComponentInChildren<EquipmentManager>();
        
        Melee currentMelee = inventory.GetMeleeItem(manager.currentlyEquippedMelee);

        GameObject[] grenades = GameObject.FindGameObjectsWithTag("Grenade");
        foreach (GameObject grenade in grenades)
        {
            Instantiate(currentMelee.explosionEffect, grenade.transform.position, grenade.transform.rotation);
            Destroy(grenade);
            break;
        }

        GameObject[] flashbangs = GameObject.FindGameObjectsWithTag("Flashbang");
        foreach (GameObject flashbang in flashbangs)
        {
            Instantiate(currentMelee.explosionEffect, flashbang.transform.position, flashbang.transform.rotation);
            Destroy(flashbang);
            break;
        }
    }
}
