using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text timeLeftText;
    public float timeLeft = 50f;
    public DeliveryBar deliveryBar;
    private bool _isPulsing;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timeLeftText.text = timeLeft.ToString(timeLeft >= 10f ? "0" : "0.00");
        if (timeLeft <= 10f)
        {
            if (!_isPulsing)
            {
                StartCoroutine(PulseText());
            }
        }
    }
    
    private IEnumerator PulseText()
    {
        _isPulsing = true;
        Vector3 originalScale = timeLeftText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float duration = 1f;
        float elapsedTime = 0f;

        while (timeLeft <= 10f)
        {
            float speed = Mathf.Lerp(5f, 1f, timeLeft / 10f);
            duration =  1f / speed;
            while (elapsedTime < duration)
            {
                timeLeftText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                timeLeftText.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
        }

        timeLeftText.transform.localScale = originalScale;
        _isPulsing = false;
    }

    public void UpdateDeliveryBar()
    {
        deliveryBar.ChangeResourceByAmount();
    }

    public void Play()
    {
        LoadScene("Level1");
    }

    public void LoadMainMenu()
    {
        LoadScene("Menu");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}