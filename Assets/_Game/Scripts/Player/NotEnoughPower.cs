using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotEnoughPower : MonoBehaviour
{
    [SerializeField] private GameObject notEnoughPowerText;
    [SerializeField] private Vector3 offset;
    [SerializeField] private int destroyTime;

    public void InstantiateText()
    {
        var instance = Instantiate(notEnoughPowerText, this.transform.position+offset, Quaternion.identity,this.transform);
        Destroy(instance, destroyTime);
    }
}