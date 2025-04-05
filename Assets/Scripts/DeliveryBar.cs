using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryBar : MonoBehaviour
{
    [SerializeField] private Image bar;

    [SerializeField] private int resourceCurrent = 0;
    [SerializeField] private int resourceMax = 5;
    
    private Image _barBackground;

    private void Start()
    {
        bar.fillAmount = 0f;
        _barBackground = GetComponent<Image>();
        _barBackground.color = new Color(0, 0, 0, 0f);
    }

    private void UpdateBarAndResourceText()
    {
        float fillAmountTarget = (float)resourceCurrent / resourceMax;
        StartCoroutine(UpdateBarCoroutine(fillAmountTarget));
        bar.fillAmount = fillAmountTarget;
    }
    
    private IEnumerator UpdateBarCoroutine(float targetFillAmount)
    {
        float currentFillAmount = bar.fillAmount;
        float elapsedTime = 0f;
        float duration = 1f; // Duration of the animation

        while (elapsedTime < duration)
        {
            bar.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bar.fillAmount = targetFillAmount;
    }

    public void ChangeResourceByAmount()
    {
        resourceCurrent++;
        resourceCurrent = Mathf.Clamp(value: resourceCurrent, min: 0, resourceMax);
        _barBackground.color = new Color(0, 0, 0, 0.8f);
        UpdateBarAndResourceText();
    }
}