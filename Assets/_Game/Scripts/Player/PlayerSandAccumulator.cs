using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.Saving;
using _Game.Scripts.States;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using LiquidVolumeFX;
using TMPro;

[Serializable]
public class SandAccumulatorData
{
    public Dictionary<int, float> LiquidLayers;
    public float CurrentCapacity;

    public SandAccumulatorData(Dictionary<int, float> liquidVolumeDictionary, float currentCapacity)
    {
        LiquidLayers = liquidVolumeDictionary;
        CurrentCapacity = currentCapacity;
    }
}

public class PlayerSandAccumulator : MonoBehaviour, ISaveable
{
    [SerializeField] private LiquidVolume liquidVolume;
    public LiquidVolume LiquidVolume => (liquidVolume);
    [SerializeField] private Effects playerEffects;
    [SerializeField] private float maxCapacity;
    [SerializeField] private float fillAmount;
    [SerializeField] private TextMeshProUGUI capacityIsFullText;
    [SerializeField] private Stats stats;
    [SerializeField] private StateController playerState;
    [SerializeField] private BumpChecker bumpChecker;

    private SandType currentSandType;
    private ParticleSystem.MainModule dustEffect;
    private double currentCapacityValue;
    private const int Blue = 0;
    private const int Pink = 1;
    private const int Yellow = 2;
    private const int Green = 3;
    private const int Purple = 4;
    private const int Red = 5;
    private InGameUI inGameUI;
    public Action OnCapacityFull;

    void Start()
    {
        dustEffect = playerEffects.effect.main;
        currentSandType = SandType.Yellow;
        stats.OnStackCapacityChange += UpdateMaxCapacity;
    }

    public void Initialize()
    {
        if (inGameUI == null)
        {
            GetInGameUI();
        }

        GetMaxCapacity();
        inGameUI.CapacityBar.UpdateMaxCapacity(maxCapacity);
        UpdateInGameUICapacityBar();
    }

    public void AccumulateSand()
    {
        switch (currentSandType)
        {
            case SandType.Blue:
                liquidVolume.liquidLayers[Blue].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
            case SandType.Pink:
                liquidVolume.liquidLayers[Pink].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
            case SandType.Yellow:
                liquidVolume.liquidLayers[Yellow].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
            case SandType.Green:
                liquidVolume.liquidLayers[Green].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
            case SandType.Purple:
                liquidVolume.liquidLayers[Purple].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
            case SandType.Red:
                liquidVolume.liquidLayers[Red].amount += fillAmount / maxCapacity;
                IncreaseCurrentCapacityValue();
                break;
        }

        UpdateLayers();
    }

    private void UpdateLayers()
    {
        liquidVolume.UpdateLayers();
    }


    public void ChangeDustColor(SandType type)
    {
        SetSandType(type);
        switch (type)
        {
            case SandType.Blue:
                dustEffect.startColor = new Color(0f, 0.38f, 0.46f, 0.4f);
                break;
            case SandType.Pink:
                dustEffect.startColor = new Color(1f, 0.27f, 0.75f, 0.4f);
                break;
            case SandType.Yellow:
                dustEffect.startColor = new Color(0.34f, 0.25f, 0.06f, 0.4f);
                break;
            case SandType.Green:
                dustEffect.startColor = new Color(0.01f, 0.4f, 0f, 0.4f);
                break;
            case SandType.Purple:
                dustEffect.startColor = new Color(0.46f, 0.22f, 0.85f, 0.4f);
                break;
            case SandType.Red:
                dustEffect.startColor = new Color(0.7f, 0f, 0f, 0.4f);
                break;
        }
    }

    public bool CanAccumulateSand()
    {
        return currentCapacityValue < maxCapacity;
        // return liquidVolumeFX.liquidLayers[0].amount + liquidVolumeFX.liquidLayers[1].amount +
        //     liquidVolumeFX.liquidLayers[2].amount < 1;
    }

    public bool IsCapacityEmpty()
    {
        return currentCapacityValue == 0;
    }

    public void ResetCapacity()
    {
        capacityIsFullText.enabled = false;
        currentCapacityValue = 0;
        inGameUI.CapacityBar.UpdateCapacity(currentCapacityValue);
        bumpChecker.ChangeIsPlayerCapacityFull(false);
    }

    private void IncreaseCurrentCapacityValue()
    {
        currentCapacityValue += Math.Round((fillAmount / maxCapacity) * maxCapacity, 2);
        inGameUI.CapacityBar.UpdateCapacity(currentCapacityValue);
        if (!CanAccumulateSand())
        {
            PlayerCapacityIsFull();
        }

        // print("current capacity value" + currentCapacityValue + "/" + maxCapacity);
    }

    private void PlayerCapacityIsFull()
    {
        currentCapacityValue = maxCapacity;
        UpdateInGameUICapacityBar();
        capacityIsFullText.enabled = true;
        playerState.ChangeState(playerState.IdleState);
        bumpChecker.ChangeIsPlayerCapacityFull(true);
        OnCapacityFull?.Invoke();
    }

    private void UpdateInGameUICapacityBar()
    {
        inGameUI.CapacityBar.UpdateCapacity(currentCapacityValue);
    }

    private void SetSandType(SandType type)
    {
        currentSandType = type;
    }

    private void UpdateMaxCapacity(float value)
    {
        maxCapacity = value;
    }

    public object CaptureState()
    {
        Dictionary<int, float> liquidVolumeDictionary = new();
        for (int i = 0; i < 5; i++)
        {
            liquidVolumeDictionary.Add(i, liquidVolume.liquidLayers[i].amount);
        }

        return new SandAccumulatorData(liquidVolumeDictionary, (float)currentCapacityValue);
    }

    public void RestoreState(object state)
    {
        var data = (SandAccumulatorData)state;
        foreach (var layer in data.LiquidLayers)
        {
            liquidVolume.liquidLayers[layer.Key].amount = layer.Value;
        }

        UpdateLayers();
        GetInGameUI();
        currentCapacityValue = data.CurrentCapacity;
        GetMaxCapacity();
        //print("current cap: " + currentCapacityValue + "max cap: " + maxCapacity);
        if (currentCapacityValue >= maxCapacity)
        {
            //print("current cap: " + currentCapacityValue + "max cap: " + maxCapacity);
            PlayerCapacityIsFull();
        }
    }

    private void GetInGameUI()
    {
        inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
    }

    private void GetMaxCapacity()
    {
        maxCapacity = stats.GetStat(Stat.StackCapacity);
    }
}