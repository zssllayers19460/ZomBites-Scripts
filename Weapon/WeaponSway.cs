using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.001f;   // amount of weapon sway
    public float maxSwayAmount = 0.002f;   // maximum amount of weapon sway

    // Update is called once per frame
    void Update()
    {
        float swayX = -Input.GetAxis("Mouse X") * swayAmount;   // Get mouse X input for left-right sway
        float swayY = -Input.GetAxis("Mouse Y") * swayAmount;   // Get mouse Y input for up-down sway

        // Apply left-right sway to the right of the weapon when looking left, and to the left when looking right
        swayX = Mathf.Clamp(swayX, -maxSwayAmount, maxSwayAmount);
        swayY = Mathf.Clamp(swayY, -maxSwayAmount, maxSwayAmount);
        Vector3 sway = new Vector3(swayX, swayY, 0f);   // Swap swayX and swayY to achieve desired sway effect
        transform.localPosition += sway;
    }
}