using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    public float timer;
    public float flashTimer;
    public float flashDuration;
    public float spinSpeed;
    public float rotation;

    public float destroyLootAfterAudioDelay = 2f;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 10f) // timer greater than 10
        {
            rotation += spinSpeed * Time.deltaTime;
        }
        else if (timer > 0)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0) // if flashTimer is less than or equal too 0 
            {
                flashTimer = flashDuration;
                Flash();
            }
        }
        else
        {
            Destroy(gameObject);
        }
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void Flash()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = !renderer.enabled;

        Light light = GetComponent<Light>();
        light.enabled = !light.enabled;
    }

    public void RemoveLoot()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;

        Light light = GetComponent<Light>();
        light.enabled = false;

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        Destroy(gameObject, destroyLootAfterAudioDelay);
    }
}
