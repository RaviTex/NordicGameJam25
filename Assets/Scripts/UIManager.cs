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
        timeLeftText.text = timeLeft.ToString("0.00");
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
