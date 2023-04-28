using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float cookTime = 1.5f; // how long it takes to cook the grenade
    private float countdown;
    public float blastRadius = 50f;
    public float explosionForce = 100f;
    public int damage = 25;
    public GameObject explosionEffect;
    public AudioClip explosionSound;
    public int magSize = 1;
    public int magCount = 3;
    public float throwRange = 10f;
    public float throwSpeed = 20f;
    public KeyCode cookKey = KeyCode.Mouse1;

    public GameObject explosionMarkPrefab;

    private bool isCooking = false; // keep track of whether player is cooking the grenade
    private bool hasExploded = false; // keep track of whether the grenade has exploded or not
    private AudioSource source;

    // Start is called before the first frame update
    private void Start()
    {
        countdown = delay;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(cookKey)) // if the player starts cooking the grenade
        {
            isCooking = true;
        }
        else if (Input.GetKeyUp(cookKey)) // if the player stops cooking the grenade
        {
            isCooking = false;
        }

        if (isCooking)
        {
            countdown -= Time.deltaTime * 2f; // decrement countdown timer faster
        }
        else
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0f && !hasExploded)
        {
            ExplodeNade();
            hasExploded = true;
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
    }

    private void ExplodeNade()
    {
        // Play Explosion sound
        //AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Spawn explosion mark at the bottom of the ground with the correct rotation
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Vector3 position = hit.point + (hit.normal * 0.001f); // offset the position slightly to avoid z-fighting
            Instantiate(explosionMarkPrefab, position, rotation);
        }

        // Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObjects in colliders)
        {
            // Get Rigidbody from current object
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }

            // Get Rigidbody from children of current object
            Rigidbody[] childRbs = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody childRb in childRbs)
            {
                if (childRb.gameObject != gameObject)
                {
                    childRb.AddExplosionForce(explosionForce, transform.position, blastRadius);
                }
            }

            CharacterStats stats = nearbyObjects.GetComponent<CharacterStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }

        // remove grenade gameobject
        Destroy(gameObject);    
    }
}