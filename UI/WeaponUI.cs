using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI magazineSizeText;
    [SerializeField] private TextMeshProUGUI storedAmmoText;

    private Inventory inventory;
    private EquipmentManager manager;

    private void Start()
    {
        GetReferences();

        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        if (currentWeapon != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = currentWeapon.icon;
            magazineSizeText.text = currentWeapon.magazineSize.ToString();
            storedAmmoText.text = currentWeapon.storedAmmo.ToString();
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void UpdateStats(Sprite weaponIcon, int magazineSize, int storedAmmo)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = weaponIcon;
        magazineSizeText.text = magazineSize.ToString();
        storedAmmoText.text = storedAmmo.ToString();
    }   

    public void UpdateAmmoUI(int magazineSize, int storedAmmo)
    {
        icon.gameObject.SetActive(true);
        magazineSizeText.text = magazineSize.ToString();
        storedAmmoText.text = storedAmmo.ToString();
    }

    private void GetReferences()
    {
        manager = FindObjectOfType<EquipmentManager>();
        inventory = FindObjectOfType<Inventory>();
    }
}
