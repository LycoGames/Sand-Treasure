using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Observer;
using _Game.Scripts.Player;
using _Game.Scripts.UI;
using DG.Tweening;
using UnityEngine;

public class CollectTutorialElement : TutorialElement, IObserver
{
    private Inventory playerInventory;
    [SerializeField] private int moneyCondition;

    public void Initialize(Inventory playerInventory)
    {
        this.playerInventory = playerInventory;
        playerInventory.AddObserver(this);
    }

    public void OnNotify(int value, ItemType type)
    {
        if (type != ItemType.Money) return;
        if (value < moneyCondition) return;
        ConditionComplete();
    }
}