using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class PoolCreator : MonoBehaviour
{
    [SerializeField] private List<StackableItem> stackableItemsList = new();
    [SerializeField] private ItemsObjectPool poolPrefab;
    private List<ItemsObjectPool> itemsObjectPools = new List<ItemsObjectPool>();
    public List<ItemsObjectPool> ItemsObjectPools => itemsObjectPools;

    private void Awake()
    {
        foreach (var item in stackableItemsList)
        {
            var pool = Instantiate(poolPrefab, this.transform);
            pool.SetObjectPoolType(item);
            itemsObjectPools.Add(pool);
        }
    }
}