using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 0 = Primary, 1 = Secondary, 2 = Melee
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private Melee[] melee;

    // Script References
    private Shooting shooting;

    private void Awake()
    {
        GetReferences();
        InitVariables();
    }

    public void AddItem(Weapon newItem)
    {
        int newItemIndex = (int)newItem.weaponStyle;

        if (weapons[newItemIndex] != null)
        {
            RemoveItem(newItemIndex);
        }
        weapons[newItemIndex] = newItem;

        shooting.InitAmmo((int)newItem.weaponStyle, newItem);
    }

    public void AddMeleeItem(Melee newMeleeItem)
    {
        int newMeleeIndex = (int)newMeleeItem.handHeldStyle;

        if (melee[newMeleeIndex] != null)
        {
            RemoveMeleeItem(newMeleeIndex);
        }
        melee[newMeleeIndex] = newMeleeItem;

        //lethalShooting.InitAmmo((int)newMeleeItem.handHeldStyle, newMeleeItem);
    }

    public void RemoveItem(int index)
    {
        weapons[index] = null;
    }

    public void RemoveMeleeItem(int index)
    {
        melee[index] = null;
    }

    public Weapon GetItem(int index)
    {
        return weapons[index];
    }

    public Melee GetMeleeItem(int index)
    {
        return melee[index];
    }

    public void InitVariables()
    {
        weapons = new Weapon[3];
        melee = new Melee[4];
    }

    private void GetReferences()
    {
        // Script References
        shooting = GetComponent<Shooting>();
    }
}
