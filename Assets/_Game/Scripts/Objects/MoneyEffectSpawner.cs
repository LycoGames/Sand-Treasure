using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEffectSpawner : MonoBehaviour
{
    [SerializeField] private DollarSignText dollarSignTextPrefab;
    
    public void Spawn(int moneyValue,Vector3 pos)
    {
        var instance = Instantiate(dollarSignTextPrefab,pos,Quaternion.identity);
        instance.SetMoneyValueText(moneyValue);
    }
}