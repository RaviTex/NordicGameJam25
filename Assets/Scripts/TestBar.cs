using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBar : MonoBehaviour
{
    [SerializeField] private int woodValue = 10;
    [SerializeField] private DeliveryBar resourceBarTracker;

    public void ChangeWood()
    {
        bool succesfulDropOff = resourceBarTracker.ChangeResourceByAmount(woodValue);

        if (succesfulDropOff)
            Debug.Log("DropOff successful");
        else
            Debug.Log("DropOff faild");
                    
    }
}
