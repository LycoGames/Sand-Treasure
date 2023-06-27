using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapacityBar : MonoBehaviour
{
    [SerializeField] private Image capacityBarFill;
    [SerializeField] private TextMeshProUGUI maxCapacityText;
    [SerializeField] private TextMeshProUGUI currentCapacityText;
    private Stats playerStats;
    private float playerMaxCapacity;
    private float playerCurrentCapacity;

    public void Initialize(Stats stats)
    {
        playerStats = stats;
        playerStats.OnStackCapacityChange += UpdateMaxCapacity;
        playerStats.OnStackCapacityChange += ReFillBar;

        // SetCurrentCapacityText();
    }

    public void UpdateCapacity(double value)
    {
        int valueForText = (int)value;
        playerCurrentCapacity = (float)value;
        currentCapacityText.text = valueForText.ToString();
        capacityBarFill.DOFillAmount((playerCurrentCapacity / playerMaxCapacity), 0.5f);
    }

    public void UpdateMaxCapacity(float value)
    {
        print("capacity bar max cap: " + value);
        maxCapacityText.text = value.ToString();
        playerMaxCapacity = value;
    }

    private void SetCurrentCapacityText()
    {
        currentCapacityText.text = "0";
    }

    private void ReFillBar(float value)
    {
        capacityBarFill.DOFillAmount((playerCurrentCapacity / playerMaxCapacity), 0.5f);
    }
}