using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange;
    [SerializeField] private float weaponDestroyDelay;
    [SerializeField] private LayerMask pickupLayer;

    [SerializeField] private Image weaponPickup;
    [SerializeField] private Image weaponPickupIcon;
    [SerializeField] private TextMeshProUGUI weaponPickupText;

    // Script References
    private Camera cam;
    private Inventory inventory;

    private void Start()
    {
        GetReferences();
        weaponPickupIcon.gameObject.SetActive(false);
        weaponPickupText.gameObject.SetActive(false);
        weaponPickup.gameObject.SetActive(false);
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            weaponPickup.gameObject.SetActive(true);
            weaponPickupIcon.gameObject.SetActive(true);
            weaponPickupText.gameObject.SetActive(true);

            // Update UI text and icon    // for some reason pickup is added to the end of the text so it needs this lin
            string itemName = hit.transform.name.Replace("Pickup", "");
            weaponPickupText.text = "Press E to swap " + itemName;
            Item item = hit.transform.GetComponent<ItemObject>().item;
            weaponPickupIcon.sprite = item.icon;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Weapon newWeapon = item as Weapon;
                
                print("Picked up or Hit: " + hit.transform.name);

                inventory.AddItem(newWeapon);
                Destroy(hit.transform.gameObject, weaponDestroyDelay);
            }
        }
        else
        {
            weaponPickup.gameObject.SetActive(false);
            weaponPickupIcon.gameObject.SetActive(false);
            weaponPickupText.gameObject.SetActive(false);
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
    }
}