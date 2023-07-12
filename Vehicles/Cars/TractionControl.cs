using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractionControl : MonoBehaviour
{
    public WheelCollider[] driveWheels;
    public float maxSlipLimit = 0.2f;
    public float throttleSensitivity = 1.0f;

    private Rigidbody car;

    private void Start()
    {
        car = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (WheelCollider wheel in driveWheels)
        {
            ApplyTractionControl(wheel);
        }
    }

    private void ApplyTractionControl(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            float wheelSlip = Mathf.Abs(hit.forwardSlip);
            float throttleInput = Input.GetAxis("Vertical");
            float tractionControl = (1 - wheelSlip) * throttleSensitivity * throttleInput;

            if (wheelSlip > maxSlipLimit)
            {
                wheel.motorTorque -= tractionControl * Time.deltaTime * wheel.radius * wheel.rpm;
                wheel.brakeTorque += tractionControl * Time.deltaTime * wheel.radius * wheel.rpm;
            }
        }
    }
}