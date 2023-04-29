using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquippedWeapon = 2;
    [HideInInspector] public GameObject currentWeaponObject = null;
    [HideInInspector] public Transform currentWeaponBarrel = null;
    public KeyCode equipPrimary = KeyCode.Alpha1;
    public KeyCode equipSecondary = KeyCode.Alpha2;
    public KeyCode equipMelee = KeyCode.V;

    public Transform WeaponHolder = null;
    public Weapon defaultWeapon = null;
    public Weapon defaultKnife = null;

    private Weapon previousWeapon;

    private Camera cam;
    private Inventory inventory;
    private Animator anim;
    private PlayerHUD hud;
    private Knife knife;

    private void Start()
    {
        GetReferences();
        inventory.InitVariables(); // Call InitVariables() in Inventory
        InitVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(equipPrimary) && inventory.GetItem(0) != null && currentlyEquippedWeapon != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }
        if (Input.GetKeyDown(equipSecondary) && inventory.GetItem(1) != null && currentlyEquippedWeapon != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(1));
        }
        if (Input.GetKeyDown(equipMelee) && inventory.GetItem(2) != null && currentlyEquippedWeapon != 2)
        {
            StorePreviousWeapon();
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(2));
            knife.UseKnife();
            UnequipWeapon();
            EquipPreviousWeapon();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        StorePreviousWeapon();
        currentlyEquippedWeapon = (int)weapon.weaponStyle;
        currentWeaponObject = Instantiate(weapon.weaponPrefab, WeaponHolder);
        currentWeaponBarrel = currentWeaponObject.transform.Find("Barrel");
        hud.UpdateWeaponUI(weapon);
    }

    public void UnequipWeapon()
    {
        // Destroy the current weapon object
        Destroy(currentWeaponObject);
    }

    private void StorePreviousWeapon()
    {
        previousWeapon = inventory.GetItem(currentlyEquippedWeapon);
    }

    public void EquipPreviousWeapon()
    {
        if (previousWeapon != null)
        {
            UnequipWeapon();
            EquipWeapon(previousWeapon);
        }
    }

    public void InitVariables()
    {
        inventory.AddItem(defaultWeapon);
        inventory.AddItem(defaultKnife);
        EquipWeapon(inventory.GetItem(1));
    }

    private void GetReferences()
    {
        inventory = GetComponent<Inventory>();
        hud = GetComponent<PlayerHUD>();
        anim = GetComponent<Animator>();
        knife = GetComponent<Knife>();
    }
}