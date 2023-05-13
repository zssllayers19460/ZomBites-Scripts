using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquippedWeapon = 2;
    public int currentlyEquippedMelee = 0;
    [HideInInspector] public GameObject currentWeaponObject = null;
    [HideInInspector] public GameObject currentMeleeWeapon = null;
    [HideInInspector] public Transform currentWeaponBarrel = null;
    public Transform currentMeleeWeaponPoint = null;
    public KeyCode equipPrimary = KeyCode.Alpha1;
    public KeyCode equipSecondary = KeyCode.Alpha2;
    public KeyCode useMeleeKnife = KeyCode.V;
    public KeyCode equipMeleeWeapon = KeyCode.G;

    public Transform WeaponHolder = null;
    public Transform attackPoint = null;
    public Weapon defaultWeapon = null;
    public Weapon defaultKnife = null;

    private Weapon previousWeapon;
    private Melee previousMeleeWeapon;

    private Camera cam;
    private Inventory inventory;
    private Animator anim;
    private PlayerHUD hud;
    private Knife knife;
    private Shooting shooting;
    private LethalShooting lethalShooting;

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
            StorePreviousWeapon();
            UnequipMeleeWeapon();
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }
        if (Input.GetKeyDown(equipSecondary) && inventory.GetItem(1) != null && currentlyEquippedWeapon != 1)
        {
            StorePreviousWeapon();
            UnequipMeleeWeapon();
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(1));
        }
        if (Input.GetKeyDown(useMeleeKnife) && inventory.GetItem(2) != null && currentlyEquippedWeapon != 2)
        {
            StorePreviousWeapon();
            UnequipWeapon();
            UnequipMeleeWeapon();
            EquipWeapon(inventory.GetItem(2));
            knife.UseKnife();
            UnequipWeapon();
            EquipPreviousWeapon();
        }
        if (Input.GetKeyDown(equipMeleeWeapon) && inventory.GetMeleeItem(0) != null && currentlyEquippedMelee != 0)
        {
            StorePreviousMelee();
            UnequipMeleeWeapon();
            UnequipWeapon();
            EquipMelee(inventory.GetMeleeItem(0));
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        StorePreviousWeapon();
        currentlyEquippedWeapon = (int)weapon.weaponStyle;
        currentWeaponObject = Instantiate(weapon.weaponPrefab, WeaponHolder);
        currentWeaponBarrel = currentWeaponObject.transform.Find("Barrel");
        hud.UpdateWeaponUI(weapon);
        shooting.enabled = true;
        lethalShooting.enabled = false;
    }

    public void EquipMelee(Melee melee)
    {
        StorePreviousMelee();
        currentlyEquippedMelee = (int)melee.handHeldStyle;
        currentMeleeWeapon = Instantiate(melee.meleePrefab, WeaponHolder);
        currentMeleeWeaponPoint = currentMeleeWeapon.transform.Find("Point");
        shooting.enabled = false;
        lethalShooting.enabled = true;
    }

    public void UnequipWeapon()
    {
        // Destroy the current weapon object
        Destroy(currentWeaponObject);
    }

    public void UnequipMeleeWeapon()
    {
        // Destroy the current melee weapon object
        Destroy(currentMeleeWeapon);
    }

    private void StorePreviousWeapon()
    {
        previousWeapon = inventory.GetItem(currentlyEquippedWeapon);
    }

    private void StorePreviousMelee()
    {
        previousMeleeWeapon = inventory.GetMeleeItem(currentlyEquippedMelee);
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
        shooting = GetComponent<Shooting>();
        lethalShooting = GetComponent<LethalShooting>();
    }
}