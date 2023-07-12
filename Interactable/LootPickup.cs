using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootComponent : MonoBehaviour
{
    public Loot loot;
    public bool isPickedUp = false;
}

public class LootPickup : MonoBehaviour
{
    public float lootPickupRange;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private LayerMask lootLayer;

    // Script References
    private Camera cam;
    private Inventory inventory;

    // Is called at the start of the game
    private void Start()
    {
        GetReferences();
    }

    // Is called every one frame update
    private void Update()
    {
        // Cast a ray to the middle of the screen
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * lootPickupRange, Color.red);
        if (Physics.Raycast(ray, out hit, lootPickupRange, lootLayer))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            LootComponent lootComponent = hit.transform.GetComponent<LootComponent>();
            if (lootComponent != null && !lootComponent.isPickedUp && Input.GetKeyDown(pickupKey))
            {
                lootComponent.isPickedUp = true;
                inventory.AddLootItem(lootComponent.loot);
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            return;
        }
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
    }
}