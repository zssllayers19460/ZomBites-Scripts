using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashEffectController : MonoBehaviour
{
    public void InstantiateMuzzleFlash()
    {
        Inventory inventory = GetComponentInChildren<Inventory>();
        EquipmentManager manager = GetComponentInChildren<EquipmentManager>();

        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        if (currentWeapon != null)
        {
            Instantiate(currentWeapon.muzzleFlash, manager.currentWeaponBarrel);
        }
    }
}
