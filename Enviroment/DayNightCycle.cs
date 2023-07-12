using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // References to the sun and moon sphere transforms
    public Transform sunTransform;
    public Transform moonTransform;

    public Light sunLight;
    public Light moonLight;

    // Reference to the skybox material
    public Material[] morningSkybox, afternoonSkybox, eveningSkybox, nightSkybox;

    // Length of a full day in seconds
    public float dayLengthInSeconds = 1440f; // 24 minutes

    // Current time of day represented as a value between 0 and 1
    [Range(0f, 1f)]
    public float currentTimeOfDay = 0f;

    // Radius of the orbit around the world center
    public float orbitRadius = 10f;

    // Center position of the orbit
    public Vector3 orbitCenter;

    private void Update()
    {
        // Update the current time of day
        UpdateTimeOfDay();

        // Update the rotation of the sun and moon
        UpdateSunMoonRotation();

        // Update the position of the sun and moon
        UpdateSunMoonPosition();

        // Update the skybox rotation
        UpdateSkyboxRotation();
    }

    // Updates the current time of day based on the day length
    private void UpdateTimeOfDay()
    {
        // Increment currentTimeOfDay based on the real time
        currentTimeOfDay += Time.deltaTime / dayLengthInSeconds;

        // Wrap the current time of day back to 0 when it reaches 1
        if (currentTimeOfDay >= 1f)
        {
            currentTimeOfDay -= 1f;
        }
    }

    // Updates the rotation of the sun and moon based on the current time of day
    private void UpdateSunMoonRotation()
    {
        float rotationAngle = currentTimeOfDay * 360f;

        // Rotate the sun transform based on the current time of day
        sunTransform.localRotation = Quaternion.Euler(rotationAngle, 0f, 0f);

        // Rotate the moon transform based on the current time of day, offset by 180 degrees
        moonTransform.localRotation = Quaternion.Euler(rotationAngle + 180f, 0f, 0f);
    }

    // Updates the position of the sun and moon based on the current time of day
    private void UpdateSunMoonPosition()
    {
        float orbitAngle = currentTimeOfDay * 360f;

        // Calculate the position on the orbit based on the orbit angle and radius "(0f, 0f, orbitAngle)" to make it rotate north to south
        Vector3 orbitPosition = Quaternion.Euler(0f, 0f, orbitAngle) * new Vector3(orbitRadius, 0f, 0f);

        // Set the position of the sun by adding the orbit position to the orbit center
        sunTransform.position = orbitCenter + orbitPosition;

        // Set the position of the moon by subtracting the orbit position from the orbit center
        moonTransform.position = orbitCenter - orbitPosition;

        // Check if the sun's Y position is below the desired threshold to enable the moon
        if (sunTransform.position.y <= -orbitCenter.y)
        {
            // Enable the moon game object and light
            moonTransform.gameObject.SetActive(true);
            moonLight.enabled = true;
        }
        else
        {
            // Disable the moon game object and light
            moonTransform.gameObject.SetActive(false);
            moonLight.enabled = false;
        }

        // Check if the moon's Y position is below the desired threshold to disable the sun
        if (moonTransform.position.y <= -orbitCenter.y)
        {
            // Disable the sun game object and light
            sunTransform.gameObject.SetActive(true);
            sunLight.enabled = true;
        }
        else
        {
            // Enable the sun game object and light
            sunTransform.gameObject.SetActive(false);
            sunLight.enabled = false;
        }
    }

    // Updates the skybox rotation based on the current time of day
    private void UpdateSkyboxRotation()
    {
        // Calculate the rotationAngle based on the current time of day
        float rotationAngle = currentTimeOfDay * 360f;

        // Determine which array of skybox materials to use based on the current time of day
        Material[] currentSkyboxMaterials;
        if (currentTimeOfDay < 0.25f)
        {
            currentSkyboxMaterials = morningSkybox;
        }
        else if (currentTimeOfDay < 0.5f)
        {
            currentSkyboxMaterials = afternoonSkybox;
        }
        else if (currentTimeOfDay < 0.75f)
        {
            currentSkyboxMaterials = eveningSkybox;
        }
        else
        {
            currentSkyboxMaterials = nightSkybox;
        }

        // Select a random material from the currentSkyboxMaterials array
        Material randomMaterial = currentSkyboxMaterials[Random.Range(0, currentSkyboxMaterials.Length)];

        // Assign the random material to the skybox
        RenderSettings.skybox = randomMaterial;

        // Set the rotation of the skybox materials
        foreach (Material material in currentSkyboxMaterials)
        {
            material.SetFloat("_Rotation", rotationAngle);
        }
    }
}