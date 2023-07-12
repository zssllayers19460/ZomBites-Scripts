using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    public float pickupRange;
    [SerializeField] private float weaponDestroyDelay;
    [SerializeField] private LayerMask pickupLayer;

    [SerializeField] private Image pickupIndicatorImage;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI pickupText;

     // Customizable pickup prompt text

    // Script References
    private Camera cam;
    private Inventory inventory;

    private void Start()
    {
        GetReferences();
        weaponIcon.gameObject.SetActive(false);
        pickupText.gameObject.SetActive(false);
        pickupIndicatorImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            pickupIndicatorImage.gameObject.SetActive(true);
            weaponIcon.gameObject.SetActive(true);
            pickupText.gameObject.SetActive(true);

            Item item = hit.transform.GetComponent<ItemObject>().item;
            string itemName = item.name;
            pickupText.text = $"Press {pickupKey} to pick up {itemName}"; // Using string interpolation
            //pickupText.text = "Press " + pickupKey + " to pick up " + itemName; // Using concatenation
            weaponIcon.sprite = item.icon;

            if (Input.GetKeyDown(pickupKey))
            {
                if (item is Weapon newWeapon)
                {
                    inventory.AddItem(newWeapon);
                }
                if (item is Melee newMelee)
                {
                    inventory.AddMeleeItem(newMelee);
                }

                print("Picked up or Hit: " + hit.transform.name);
                Destroy(hit.transform.gameObject, weaponDestroyDelay);
            }
        }
        else
        {
            pickupIndicatorImage.gameObject.SetActive(false);
            weaponIcon.gameObject.SetActive(false);
            pickupText.gameObject.SetActive(false);
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
    }
}