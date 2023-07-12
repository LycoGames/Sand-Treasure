using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class MoneyPool :GenericObjectPool<Money>
{
    public static MoneyPool Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }
}
