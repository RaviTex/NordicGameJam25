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
    public UIState uiState;
    private UIState _oldUIState;

    [Header("Different UI Panels")] 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameUI;

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

    private void Start()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (uiState == UIState.MainMenu)
        {
            mainMenu.SetActive(true);
            gameUI.SetActive(false);
        }
        else if (uiState == UIState.Game)
        {
            mainMenu.SetActive(false);
            gameUI.SetActive(true);
        }
        _oldUIState = uiState;
    }

    private void Update()
    {
        if (uiState != _oldUIState)
        {
            UpdateUI();
        }
        if (uiState == UIState.Game)
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
            duration = 1f / speed;
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
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        LoadScene("Menu");
    }
    public void LoadWinScreen()
    {
        LoadScene("WinScene");
    }
    public void LoadTutorial()
    {
        LoadScene("Tutorial");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

public enum UIState
{
    MainMenu,
    Game,
    Tutorial,
    Win,
    GameOver
}