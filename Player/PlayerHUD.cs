using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private WeaponUI weaponUI;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.SetValues(currentHealth, maxHealth);
    }

    public void UpdateWeaponUI(Weapon newWeapon)
    {
        weaponUI.UpdateStats(newWeapon.icon, newWeapon.magazineSize, newWeapon.storedAmmo);
    }

    public void UpdateWeaponAmmoUI(int currentAmmo, int storedAmmo)
    {
        weaponUI.UpdateAmmoUI(currentAmmo, storedAmmo);
    }
}
