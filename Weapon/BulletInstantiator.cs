using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstantiator : MonoBehaviour
{
    public GameObject bulletPrefab;

    public void InstantiateBullet(Transform spawnTransform)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnTransform.position, spawnTransform.rotation);
    }
}