using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        GetReferences();
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Collider col in ragdollColliders)
        {
            // this needs to be like this otherwise the main capsule collider gets disabled
            if (col.CompareTag("Enemy") && col != GetComponent<Collider>())
            {
                col.enabled = false;
            }
        }
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    // Disable collider so you don't walk into it when the zombie is dead
    public void DisableRagdollColliders()
    {
        foreach (Collider col in ragdollColliders)
        {
            if (col.CompareTag("Enemy") && col != GetComponent<Collider>())
            {
                col.enabled = false;
            }
        }
    }

    private void GetReferences()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }
}