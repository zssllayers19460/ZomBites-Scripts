using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectController : MonoBehaviour
{
    // Particle Effect References
    public GameObject dirtParticleEffect;
    public GameObject waterParticleEffect;
    public GameObject brickParticleEffect;
    public GameObject concreteParticleEffect;
    public GameObject metalParticleEffect;
    public GameObject woodParticleEffect;
    public GameObject stoneParticleEffect;
    public GameObject sandParticleEffect;


    public void SpawnDirtParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(dirtParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnWaterParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(waterParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnMetalParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(metalParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnBrickParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(brickParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnConcreteParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(concreteParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnWoodParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(woodParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnStoneParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(stoneParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void SpawnSandParticle(Vector3 position, Vector3 normal)
    {
        Instantiate(sandParticleEffect, position, Quaternion.FromToRotation(Vector3.up, normal));
    }
}
