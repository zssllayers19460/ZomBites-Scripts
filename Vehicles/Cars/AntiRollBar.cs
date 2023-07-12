using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public float antiRoll = 500f;

    private Rigidbody car;

    private void Start()
    {
        car = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float travelL = GetWheelTravel(leftWheel);
        float travelR = GetWheelTravel(rightWheel);

        float antiRollForce = (travelL - travelR) * antiRoll;

        ApplyAntiRollForce(leftWheel, rightWheel, -antiRollForce);
        ApplyAntiRollForce(rightWheel, leftWheel, antiRollForce);
    }

    private float GetWheelTravel(WheelCollider wheel)
    {
        WheelHit hit;
        bool grounded = wheel.GetGroundHit(out hit);
        if (grounded)
        {
            return 1.0f - (hit.point.y - wheel.transform.position.y) / wheel.suspensionDistance;
        }
        return 1.0f;
    }

    private void ApplyAntiRollForce(WheelCollider wheel, WheelCollider oppositeWheel, float force)
    {
        if (wheel.isGrounded && oppositeWheel.isGrounded)
        {
            float forcePerWheel = force / 2.0f;
            car.AddForceAtPosition(wheel.transform.up * -forcePerWheel, wheel.transform.position);
            car.AddForceAtPosition(oppositeWheel.transform.up * forcePerWheel, oppositeWheel.transform.position);
        }
    }
}