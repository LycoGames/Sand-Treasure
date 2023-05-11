using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private List<StackableItem> stackableItemsList = new();
    [SerializeField] private ItemsObjectPool poolPrefab;
    private List<ItemsObjectPool> itemsObjectPools = new List<ItemsObjectPool>();
    public List<ItemsObjectPool> ItemsObjectPools => itemsObjectPools;
    private Dictionary<ItemType, ItemsObjectPool> poolsDictionary = new();
    public static PoolManager Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        InitializePools();
    }

    private void InitializePools()
    {
        poolsDictionary = new Dictionary<ItemType, ItemsObjectPool>();
        foreach (var item in stackableItemsList)
        {
            var pool = Instantiate(poolPrefab, this.transform);
            pool.SetObjectPoolType(item);
            itemsObjectPools.Add(pool);
            poolsDictionary[item.Type] = pool;
        }
    }

    public void ReturnItemToItsPool(StackableItem item)
    {
        var pool = poolsDictionary[item.Type];
        pool.ReturnToPool(item);
    }

    public StackableItem GetFromPool(ItemType type)
    {
        var pool = poolsDictionary[type];
        var item = pool.GetFromPool();
        return item;
    }
}