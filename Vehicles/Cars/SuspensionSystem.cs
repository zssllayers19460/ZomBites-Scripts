using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionSystem : MonoBehaviour
{
    public float suspensionDistance = 0.3f;
    public float suspensionStiffness = 300f;
    public float suspensionDamping = 300f;

    private Rigidbody carRigidbody;
    private RaycastHit hit;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, suspensionDistance))
        {
            Vector3 suspensionForce = CalculateSuspensionForce();
            ApplySuspensionForce(suspensionForce);
        }
    }

    private Vector3 CalculateSuspensionForce()
    {
        float compression = suspensionDistance - hit.distance;
        float suspensionVelocity = Vector3.Dot(carRigidbody.GetPointVelocity(hit.point), transform.up);
        float suspensionForceMagnitude = compression * suspensionStiffness - suspensionVelocity * suspensionDamping;

        return transform.up * suspensionForceMagnitude;
    }

    private void ApplySuspensionForce(Vector3 force)
    {
        carRigidbody.AddForceAtPosition(force, hit.point);
    }
}