using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<ZonesPair> zonesPairs;

    public int curZonePairActive = 0;
    private int _oldCurZonePairActive = 0;

    private GameObject _curDropOffZone;
    private GameObject _curPickUpZone;
    private GameObject _pickUpMarker;
    private GameObject _dropOffMarker;
    public Vector3 markerOffset;
    public Image pickUpZoneImage;
    public Image dropOffZoneImage;
    [SerializeField] private float markerDisappearDistance;
    private TMP_Text _dropOffDistanceText;
    private TMP_Text _pickUpDistanceText;

    private Camera _mainCamera;
    public PlayerController playerController;

    public List<float> timeLeft;


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
        _mainCamera = Camera.main;
        if (dropOffZoneImage && pickUpZoneImage)
        {
            _dropOffDistanceText = dropOffZoneImage.transform.parent.GetChild(1).GetComponent<TMP_Text>();
            _pickUpDistanceText = pickUpZoneImage.transform.parent.GetChild(1).GetComponent<TMP_Text>();
            _dropOffMarker = dropOffZoneImage.transform.parent.gameObject;
            _pickUpMarker = pickUpZoneImage.transform.parent.gameObject;
        }

        UpdateZones();
    }


    private void Update()
    {
        if (_oldCurZonePairActive != curZonePairActive)
        {
            UpdateZones();
        }

        UpdateMarker();

        if (curZonePairActive > zonesPairs.Count)
        {
            WonLevel();
            return;
        }

        timeLeft[curZonePairActive] -= Time.deltaTime;
        if (timeLeft[curZonePairActive] <= 0)
        {
            GameOver();
            timeLeft[curZonePairActive] = 0;
        }

        UIManager.Instance.timeLeft = timeLeft[curZonePairActive];
    }

    public void GameOver()
    {
        UIManager.Instance.LoadMainMenu();
    }

    public void WonLevel()
    {
        UIManager.Instance.LoadWinScreen();
    }

    private void UpdateMarker()
    {
        if (_curPickUpZone && _curDropOffZone && pickUpZoneImage && dropOffZoneImage)
        {
            _dropOffMarker.transform.position = MarkerPosition(true);
            _pickUpMarker.transform.position = MarkerPosition(false);
            float dropOffDistance = Vector3.Distance(_curDropOffZone.transform.position, playerController.transform.position);
            if(dropOffDistance < markerDisappearDistance)
                _pickUpMarker.SetActive(false);
            _dropOffDistanceText.text = dropOffDistance.ToString("0") + "m";
            float pickUpDistance = Vector3.Distance(_curPickUpZone.transform.position, playerController.transform.position);
            if(pickUpDistance < markerDisappearDistance)
                _pickUpMarker.SetActive(false);
            _pickUpDistanceText.text = pickUpDistance.ToString("0") + "m";
        }
    }

    private Vector2 MarkerPosition(bool isDropOff)
    {
        float minX = isDropOff ? dropOffZoneImage.GetPixelAdjustedRect().width / 2 : pickUpZoneImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = isDropOff ? dropOffZoneImage.GetPixelAdjustedRect().height / 2 : pickUpZoneImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;
        Vector2 screenPos =
            _mainCamera.WorldToScreenPoint((isDropOff ? _curDropOffZone.transform.position : _curPickUpZone.transform.position) + markerOffset);
        if (Vector3.Dot((isDropOff ? _curDropOffZone.transform.position : _curPickUpZone.transform.position) - _mainCamera.transform.position,
                _mainCamera.transform.forward) < 0)
        {
            screenPos.x = screenPos.x < Screen.width / 2 ? minX : maxX;
        }

        screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);
        return screenPos;
    }

    private void UpdateZones()
    {
        for (int i = 0; i < zonesPairs.Count; i++)
        {
            if (i == curZonePairActive)
            {
                zonesPairs[i].pickUpZone.SetActive(true);
                zonesPairs[i].dropOffZone.SetActive(true);
                _pickUpMarker.SetActive(true);
                _dropOffMarker.SetActive(true);
                _curPickUpZone = zonesPairs[i].pickUpZone;
                _curDropOffZone = zonesPairs[i].dropOffZone;
            }
            else
            {
                zonesPairs[i].pickUpZone.SetActive(false);
                zonesPairs[i].dropOffZone.SetActive(false);
            }
        }

        _oldCurZonePairActive = curZonePairActive;
    }

    public void DisableMarker(bool isDropOff)
    {
        if (isDropOff)
        {
            _dropOffMarker.SetActive(false);
        }
        else
        {
            _pickUpMarker.SetActive(false);
        }
    }
}