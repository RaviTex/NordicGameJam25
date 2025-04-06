using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<string> tutorialTexts;

    private TMP_Text _tutorialText;

    private int _curTextIndex = 0;

    public GameObject pickUpStation;
    public GameObject dropOffStation;

    private void Start()
    {
        _tutorialText = transform.GetChild(0).GetComponent<TMP_Text>();
        GameManager.Instance.playerController.isInputEnabled = false;
        GameManager.Instance.PauseGame();
        pickUpStation.SetActive(false);
        dropOffStation.SetActive(false);
    }

    private void Update()
    {
        if (_curTextIndex < tutorialTexts.Count)
            _tutorialText.text = tutorialTexts[_curTextIndex];
        else
            _tutorialText.gameObject.SetActive(false);
        if (_curTextIndex < 2)
        {
            AudioManager.instance.StopEngineSound();
        }
        if (_curTextIndex == 2)
        {
            GameManager.Instance.playerController.isInputEnabled = true;
            GameManager.Instance.ResumeGame();
        }

        if (_curTextIndex == 4)
        {
            pickUpStation.SetActive(true);
            UIManager.Instance.EnableGameUIForTutorial();
        }

        if (_curTextIndex == 5)
            dropOffStation.SetActive(true);
    }

    public void Next()
    {
        _curTextIndex++;
    }
}