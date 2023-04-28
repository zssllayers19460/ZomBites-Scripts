using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 0 = Primary, 1 = Secondary, 2 = Melee
    [SerializeField] private Weapon[] weapons;

    // Script References
    private Shooting shooting;

    private void Awake()
    {
        GetReferences();
        InitVariables();
    }

    public void AddItem(Weapon newItem)
    {
        int newItemIndex = (int) newItem.weaponStyle;

        if(weapons[newItemIndex] != null)
        {
            RemoveItem(newItemIndex);
        }
        weapons[newItemIndex] = newItem;

        shooting.InitAmmo((int)newItem.weaponStyle, newItem);
    }

    public void RemoveItem(int index)
    {
        weapons[index] = null;
    }

    public Weapon GetItem(int index)
    {
        return weapons[index];
    }

    public void InitVariables()
    {
        weapons = new Weapon[3];
    }

    private void GetReferences()
    {
        // Script References
        shooting = GetComponent<Shooting>();
    }
}
