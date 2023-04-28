
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismemberment : MonoBehaviour
{
    public GameObject dismemberedPartPrefab; // Prefab of the dismembered part to be instantiated
    public float scaleDownDuration = 0.5f; // Duration for scaling down the bone to zero scale

    private bool isHit = false; // Flag to track if the bone has been hit

    void Update()
    {
        // Check for input or collision to trigger dismemberment
        if (Input.GetKeyDown(KeyCode.Mouse0) || isHit)
        {
            transform.localScale = Vector3.zero;
           
            // Instantiate the dismembered part
            Instantiate(dismemberedPartPrefab, transform.position, transform.rotation, transform.parent);
            // Destroy the bone game object
            //Destroy(gameObject, scaleDownDuration); // no becuse i dont want the acual head beight destroyed
        }
    }
}