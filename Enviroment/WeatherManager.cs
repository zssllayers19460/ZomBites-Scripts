using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public GameObject rainParticleSystem;  // Drag and drop the rain particle system prefab onto this variable in the Inspector
    public Transform playerTransform;  // Reference to the player's transform
    public float rainChance = 1f;

    private GameObject currentRain;  // Reference to the instantiated rain particle system

    private void Update()
    {
        // Check the chance of rain activation based on the rainChance value
        if (Random.value < rainChance)
        {
            // Activate the rain particle system if the random value is less than the rainChance
            ActivateRain();
        }
        else
        {
            // Deactivate the rain particle system if it's active
            DeactivateRain();
        }

        // Update the rain particle system position if it's active
        if (currentRain != null)
        {
            currentRain.transform.position = playerTransform.position + Vector3.up * 10f;
        }
    }

    public void ActivateRain()
    {
        if (currentRain != null)
        {
            // Rain is already active, no need to instantiate it again
            return;
        }

        // Instantiate the rain particle system
        currentRain = Instantiate(rainParticleSystem, playerTransform.position + Vector3.up * 10f, Quaternion.identity);
        currentRain.SetActive(true);

        Debug.Log("Rain activated!");
    }

    public void DeactivateRain()
    {
        if (currentRain != null)
        {
            // Destroy the rain particle system
            Destroy(currentRain);
            currentRain = null;
        }

        Debug.Log("Rain deactivated!");
    }
}