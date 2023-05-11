using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Subject
{
    private int money;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

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
}