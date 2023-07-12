using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLootDrop : MonoBehaviour
{
    public AudioClip lootAudio;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Shooting shooting = FindObjectOfType<Shooting>();
            shooting.AddLootAmmo();

            // Play the sound effect
            GetComponent<AudioSource>().clip = lootAudio;
            GetComponent<AudioSource>().Play();

            LootItem lootItem = GetComponent<LootItem>();
            lootItem.RemoveLoot();
        }
    }
}
