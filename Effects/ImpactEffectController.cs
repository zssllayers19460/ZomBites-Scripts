using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectController : MonoBehaviour
{
    // Particle Effect References
    [SerializeField] private GameObject dirtParticleEffect;
    [SerializeField] private GameObject waterParticleEffect;
    [SerializeField] private GameObject concreteParticleEffect;
    [SerializeField] private GameObject metalParticleEffect;
    [SerializeField] private GameObject woodParticleEffect;
    [SerializeField] private GameObject stoneParticleEffect;
    [SerializeField] private GameObject sandParticleEffect;
    [SerializeField] private float destroyDelay = 5f;

    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        GameObject dirtParticle = Instantiate(dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(dirtParticle, destroyDelay);
    }

    public void SpawnWaterParticle(Vector3 position, Vector3 normal)
    {
        GameObject waterParticle = Instantiate(waterParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(waterParticle, destroyDelay);
    }

    public void SpawnMetalParticle(Vector3 position, Vector3 normal)
    {
        GameObject metalParticle = Instantiate(metalParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(metalParticle, destroyDelay);
    }

    public void SpawnConcreteParticle(Vector3 position, Vector3 normal)
    {
        GameObject concreteParticle = Instantiate(concreteParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(concreteParticle, destroyDelay);
    }

    public void SpawnWoodParticle(Vector3 position, Vector3 normal)
    {
        GameObject woodParticle = Instantiate(woodParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(woodParticle, destroyDelay);
    }

    public void SpawnStoneParticle(Vector3 position, Vector3 normal)
    {
        GameObject stoneParticle = Instantiate(stoneParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(stoneParticle, destroyDelay);
    }

    public void SpawnSandParticle(Vector3 position, Vector3 normal)
    {
        GameObject sandParticle = Instantiate(sandParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
        Destroy(sandParticle, destroyDelay);
    }
}