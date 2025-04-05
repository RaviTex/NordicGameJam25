using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryBar : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField] private Image bar;
    [SerializeField] private int resourceCurrent = 0;
    [SerializeField] private int resourceMax = 5;
    private void Start()
    {
        UpdateBarAndResourceText();
        
    }

    private void UpdateBarAndResourceText()
    {
        float fillAmount = (float) resourceCurrent / resourceMax;
        bar.fillAmount = fillAmount;
    }
    public bool ChangeResourceByAmount()
    {
            resourceCurrent ++;
            resourceCurrent = Mathf.Clamp(value: resourceCurrent, min: 0, resourceMax);
        UpdateBarAndResourceText();
        return true;
    }
}
