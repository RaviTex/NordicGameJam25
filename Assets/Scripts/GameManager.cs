using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<ZonesPair> zonesPairs;

    public int curZonePairActive;
    private int _oldCurZonePairActive;


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
        if (_oldCurZonePairActive != curZonePairActive)
        {
            for (int i = 0; i < zonesPairs.Count; i++)
            {
                if (i == curZonePairActive)
                {
                    zonesPairs[i].pickUpZone.SetActive(true);
                    zonesPairs[i].dropOffZone.SetActive(true);
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
}