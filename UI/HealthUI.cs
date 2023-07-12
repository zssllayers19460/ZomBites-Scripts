using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private int baseValue;
    private int maxValue;

    [SerializeField] private Image fillImage;

    public void SetValues(int _baseValue, int _maxValue)
    {
        baseValue = _baseValue;
        maxValue = _maxValue;

        CalculateFillAmount(); 
    }

    private void CalculateFillAmount()
    {
        float fillAmount = (float) baseValue / (float) maxValue;
        fillImage.fillAmount = fillAmount;
    }
}
