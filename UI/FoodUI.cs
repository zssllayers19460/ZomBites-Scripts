using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    public Image fillImage;

    private HealthMechanics healthMechanics; // Reference to the HealthMechanics script

    private void Start()
    {
        healthMechanics = FindObjectOfType<HealthMechanics>();
    }

    private void Update()
    {
        if (healthMechanics != null)
        {
            UpdateFillImage();
        }
    }

    public void UpdateFillImage()
    {
        float fillAmount = healthMechanics.currentHunger / healthMechanics.hungerThreshold;
        fillAmount = 1f - fillAmount; // Invert the fill direction

        if (fillImage != null)
        {
            fillImage.fillAmount = fillAmount;
        }
    }
}