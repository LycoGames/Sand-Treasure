using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    [SerializeField] private List<Transform> wheels = new List<Transform>();
    [SerializeField] private int rotateSpeed;
    
    public void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
    }
}