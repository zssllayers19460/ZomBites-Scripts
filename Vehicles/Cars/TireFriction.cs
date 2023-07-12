using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireFriction : MonoBehaviour
{
    public float forwardFrictionCoefficient = 2.0f;
    public float sidewaysFrictionCoefficient = 1.5f;

    private WheelCollider wheelCollider;
    private WheelHit wheelHit;

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        ApplyFriction();
    }

    private void ApplyFriction()
    {
        if (wheelCollider.GetGroundHit(out wheelHit))
        {
            Vector3 relativeVelocity = wheelCollider.attachedRigidbody.GetPointVelocity(wheelHit.point);
            float forwardSlip = Mathf.Abs(wheelHit.forwardSlip);
            float sidewaysSlip = Mathf.Abs(wheelHit.sidewaysSlip);

            // Apply forward friction
            float forwardForce = relativeVelocity.magnitude * forwardFrictionCoefficient * forwardSlip;
            wheelCollider.attachedRigidbody.AddForceAtPosition(transform.forward * forwardForce, wheelHit.point);

            // Apply sideways friction
            float sidewaysForce = relativeVelocity.magnitude * sidewaysFrictionCoefficient * sidewaysSlip;
            wheelCollider.attachedRigidbody.AddForceAtPosition(transform.right * -Mathf.Sign(wheelHit.sidewaysSlip) * sidewaysForce, wheelHit.point);
        }
    }
}