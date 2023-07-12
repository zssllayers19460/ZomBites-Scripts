using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUI : MonoBehaviour
{
    public Image fillImage;

    public void UpdateFillImage(float currentThirst, float thirstThreshold)
    {
        float fillAmount = currentThirst / thirstThreshold;
        fillAmount = 1f - fillAmount; // Invert the fill direction

        if (fillImage != null)
        {
            fillImage.fillAmount = fillAmount;
        }
    }
}