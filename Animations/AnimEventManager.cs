using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    // Script References
    private Inventory inventory;
    private EquipmentManager manager;
    private Shooting shooting;

    private void Awake()
    {
        GetReferences();
    }

    public void DestroyWeapon()
    {
        Destroy(manager.currentWeaponObject);
    }

    public void InstantiateWeapon()
    {
        manager.currentWeaponObject = Instantiate(inventory.GetItem(manager.currentlyEquippedWeapon).weaponPrefab, manager.WeaponHolder);
        //manager.currentWeaponBarrel = manager.currentWeaponObject.transform.GetChild(0);
    }

    public void StartReload()
    {
        shooting.canReload = false;
    }

    public void EndReload()
    {
        shooting.canReload = true;
    }

    private void GetReferences()
    {
        manager = GetComponentInParent<EquipmentManager>();
        inventory = GetComponentInParent<Inventory>();
        shooting = GetComponentInParent<Shooting>();
    }
}
