using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private int baseValue;
    private int maxValue;

    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetValues(int _baseValue, int _maxValue)
    {
        baseValue = _baseValue;
        maxValue = _maxValue;

        amountText.text = baseValue.ToString();

        CalculateFillAmount(); 
    }

    private void CalculateFillAmount()
    {
        float fillAmount = (float) baseValue / (float) maxValue;
        fillImage.fillAmount = fillAmount;
    }
}
