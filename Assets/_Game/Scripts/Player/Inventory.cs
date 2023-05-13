using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using UnityEngine;

public class Inventory : Subject,ISaveable
{
    private int money;
    private int treasure;
    public void AddMoney(int value)
    {
        money += value;
        base.NotifyObservers(money,ItemType.Money);
    }
    public void SpendMoney(int value)
    {
        money -= value;
        base.NotifyObservers(money,ItemType.Money);
    }

    public void AddTreasure()
    {
        treasure += 1;
        base.NotifyObservers(treasure,ItemType.Treasure);
    }
    public int GetMoney()
    {
        return money;
    }

    public bool HasEnoughMoneyToSpend(int value)
    {
        return money >= value;
    }

    public object CaptureState()
    {
        return money;
    }

    public void RestoreState(object state)
    {
        money = (int)state;
        base.NotifyObservers(money,ItemType.Money);
    }
}