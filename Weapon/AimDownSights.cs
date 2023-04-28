using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    [Header("No Aiming Position")]
    public Vector3 originalPosition;   // original position of the weapon
    [Header("No Aiming Rotation")]
    public Vector3 originalRotation;   // original rotation of the weapon as Euler angles

    [Header("Aiming Position")]
    public Vector3 aimPosition;        // position to aim down sights
    [Header("Aiming Rotation")]
    public Vector3 aimRotation;        // rotation to aim down sights as Euler angles

    public float transitionSpeed = 10f;   // speed at which weapon transitions between positions

    private bool isAiming = false;     // flag to keep track if player is aiming

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))  // check if right mouse button is pressed
        {
            if (!isAiming)  // if not already aiming, then start aiming
            {
                isAiming = true;
            }
        }
        else
        {
            if (isAiming)   // if already aiming, then stop aiming
            {
                isAiming = false;
            }
        }

        // smoothly move weapon to target position and rotation based on isAiming flag
        if (isAiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * transitionSpeed);
            transform.localRotation = Quaternion.Euler(aimRotation);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * transitionSpeed);
            transform.localRotation = Quaternion.Euler(originalRotation);
        }
    }
}