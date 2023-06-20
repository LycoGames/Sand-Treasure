using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.States;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using LiquidVolumeFX;
using TMPro;
using Unity.VisualScripting;

public class PlayerSandAccumulator : MonoBehaviour
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

    [SerializeField] private Color pinkColor;
    private InGameUI inGameUI;

    void Start()
    {
        inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
        capacityIsFullText.enabled = false;
        dustEffect = playerEffects.effect.main;
        currentSandType = SandType.Yellow;
        maxCapacity = stats.GetStat(Stat.StackCapacity);
        stats.OnStackCapacityChange += UpdateMaxCapacity;
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
                dustEffect.startColor = new Color(0f, 204f, 255f, 100f);
                break;
            case SandType.Pink:
                dustEffect.startColor = pinkColor;
                break;
            case SandType.Yellow:
                dustEffect.startColor = Color.white;
                break;
        }
    }

    public bool CanAccumulateSand()
    {
        return currentCapacityValue < maxCapacity;
        // float sum = liquidVolumeFX.liquidLayers.Sum(liquidLayerValue => liquidLayerValue.amount);
        // return sum < 1;
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
        currentCapacityValue += Math.Round((fillAmount / maxCapacity) * 100, 2);
        inGameUI.CapacityBar.UpdateCapacity(currentCapacityValue);
        if (!CanAccumulateSand())
        {
            currentCapacityValue = maxCapacity;
            inGameUI.CapacityBar.UpdateCapacity(currentCapacityValue);
            capacityIsFullText.enabled = true;
            playerState.ChangeState(playerState.IdleState);
            bumpChecker.ChangeIsPlayerCapacityFull(true);
        }

        print("current capacity value" + currentCapacityValue + "/" + maxCapacity);
    }

    private void SetSandType(SandType type)
    {
        currentSandType = type;
    }

    private void UpdateMaxCapacity(float value)
    {
        maxCapacity = value;
    }
}