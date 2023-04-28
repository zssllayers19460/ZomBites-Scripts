using UnityEngine;

public class OceanAudio : MonoBehaviour
{
    public AudioSource waterAudio; // Reference to the water audio source
    public float maxDistance = 10f; // Maximum distance from the water
    public float fadeSpeed = 1f; // Speed of audio fade

    private Transform player; // Reference to the player's transform
    private float targetVolume = 1f; // Target volume for the audio

    private void Start()
    {
        player = Camera.main.transform; // Get the main camera's transform as the player's transform
    }

    private void Update()
    {
        // Calculate the distance between the player and the water
        float distance = Vector3.Distance(player.position, transform.position);

        // Calculate the target volume based on the player's distance from the water
        float newTargetVolume = Mathf.Clamp01(1f - (distance / maxDistance));

        // Smoothly interpolate the target volume to the new target volume
        targetVolume = Mathf.Lerp(targetVolume, newTargetVolume, Time.deltaTime * fadeSpeed);

        // Set the audio source's volume to the target volume
        waterAudio.volume = targetVolume;
    }
}