using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using DG.Tweening;
using UnityEngine;

public class UpgradeZoneTutorialElement : TutorialElement
{
    private RectTransform fingerRect;
    private readonly Vector3 scaledSize = new Vector3(1.2f, 1.2f, 1.2f);
    private readonly Vector3 stockSize = new Vector3(0.85f, 0.85f, 0.85f);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SetActiveFinger();
    }

    public void Initialize(PlayerUpgradesUI playerUpgradesUI)
    {
        playerUpgradesUI.OnStackCapacityUpgradeRequest += CompleteCondition;
        fingerRect = playerUpgradesUI.Finger;
    }

    private void SetActiveFinger()
    {
        fingerRect.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(fingerRect.DOScale(scaledSize, 0.5f))
            .Append(fingerRect.DOScale(stockSize, 0.5f)).SetLoops(-1);
    }
    
    private void CompleteCondition(Stat obj)
    {
        fingerRect.gameObject.SetActive(false);
        ConditionComplete();
    }
}