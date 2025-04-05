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

    public int curZonePairActive;
    private int _oldCurZonePairActive;
    
    private GameObject _curDropOffZone;
    private GameObject _curPickUpZone;
    private GameObject _pickUpMarker;
    private GameObject _dropOffMarker;
    public Image pickUpZoneImage;
    public Image dropOffZoneImage;
    private TMP_Text _dropOffDistanceText;
    private TMP_Text _pickUpDistanceText;
    
    public Camera _mainCamera;
    public PlayerController playerController;


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
        UpdateZones();
        _dropOffDistanceText = dropOffZoneImage.transform.parent.GetChild(1).GetComponent<TMP_Text>();
        _pickUpDistanceText = pickUpZoneImage.transform.parent.GetChild(1).GetComponent<TMP_Text>();
        _dropOffMarker = dropOffZoneImage.transform.parent.gameObject;
        _pickUpMarker = pickUpZoneImage.transform.parent.gameObject;
    }


    private void Update()
    {
        if (_oldCurZonePairActive != curZonePairActive)
        {
            UpdateZones();
        }
        
        if(_curPickUpZone && _curDropOffZone)
        {
            _dropOffMarker.transform.position = MarkerPosition(true);
            _pickUpMarker.transform.position = MarkerPosition(false);
            _dropOffDistanceText.text = Vector3.Distance(_curDropOffZone.transform.position, playerController.transform.position).ToString("F1") + "m";
            _pickUpDistanceText.text = Vector3.Distance(_curPickUpZone.transform.position, playerController.transform.position).ToString("F1") + "m";
        }
    }

    private Vector2 MarkerPosition(bool isDropOff)
    {
        float minX = isDropOff ? dropOffZoneImage.GetPixelAdjustedRect().width / 2 : pickUpZoneImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = isDropOff ? dropOffZoneImage.GetPixelAdjustedRect().height / 2 : pickUpZoneImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;
        Vector2 screenPos = _mainCamera.WorldToScreenPoint(isDropOff ? _curDropOffZone.transform.position : _curPickUpZone.transform.position);
        if(Vector3.Dot((isDropOff ? _curDropOffZone.transform.position : _curPickUpZone.transform.position) - _mainCamera.transform.position, _mainCamera.transform.forward) < 0)
        {
            if(screenPos.x < Screen.width / 2)
            {
                screenPos.x = maxX;
            }
            else
            {
                screenPos.x = minX;
            }
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
}