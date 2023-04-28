using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollow : MonoBehaviour
{
    private ZombieController zombieController;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        zombieController = GetComponentInParent<ZombieController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (zombieController.isDead) // only update position and rotation if zombie is alive
        {
            Transform zombieTransform = zombieController.transform.GetChild(0);
            capsuleCollider.transform.position = zombieTransform.position;
            capsuleCollider.transform.rotation = zombieTransform.rotation;
        }
    }
}