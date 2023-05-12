using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Subject,ISaveable
{
    private int money;
    
    public void AddMoney(int value)
    {
        money += value;
        base.NotifyObservers(money);
    }
    public void SpendMoney(int value)
    {
        money -= value;
        base.NotifyObservers(money);
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
        base.NotifyObservers(money);
    }
}