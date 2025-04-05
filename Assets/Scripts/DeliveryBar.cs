using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryBar : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField] private Image bar;
    [SerializeField] private int resourceCurrent = 100;
    [SerializeField] private int resourceMax = 100;
    [Space]
    [SerializeField] private bool overkillPossible;
    private void Start()
    {
        UpdateBarAndResourceText();
    }

    private void UpdateBarAndResourceText()
    {
        if (resourceMax <= 0)
        {
            bar.fillAmount = 0;
            return;
        }

        float fillAmount = (float) resourceCurrent / resourceMax;

        bar.fillAmount = fillAmount;
    }
    public bool ChangeResourceByAmount(int amount)
    {
        if (!overkillPossible && resourceCurrent + amount < 0)
        
            return false;
            resourceCurrent += amount;
            resourceCurrent = Mathf.Clamp(value: resourceCurrent, min: 0, resourceMax);
        bar.fillAmount = (float) (resourceCurrent / resourceMax);
        return true;
    }
}
