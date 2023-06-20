using System;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Stack;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class PoolManager : MonoBehaviour
    {
        // [SerializeField] private List<StackableItem> stackableItemsList = new();
        // [SerializeField] private ItemsObjectPool poolPrefab;
        [SerializeField] private List<SandCubes> sandCubesList = new();
        [SerializeField] private SandCubesObjectPool poolPrefab;

        private Dictionary<SandType, SandCubesObjectPool> poolsDictionary = new();

        // private Dictionary<ItemType, ItemsObjectPool> poolsDictionary = new();
        public static PoolManager Instance;
        [SerializeField] private InGameUI inGameUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            InitializePools();
        }


        private void InitializePools()
        {
            poolsDictionary = new Dictionary<SandType, SandCubesObjectPool>();
            foreach (var item in sandCubesList)
            {
                var pool = Instantiate(poolPrefab, this.transform);
                pool.SetSandType(item, inGameUI);
                poolsDictionary[item.SandType] = pool;
            }
        }

        public void ReturnItemToItsPool(SandCubes item)
        {
            item.Rigidbody.isKinematic = true;
            var pool = poolsDictionary[item.SandType];
            pool.ReturnToPool(item);
        }

        public SandCubes GetFromPool(SandType type)
        {
            var pool = poolsDictionary[type];
            var item = pool.GetFromPool();
            return item;
        }
        // private void InitializePools()
        // {
        //     poolsDictionary = new Dictionary<ItemType, ItemsObjectPool>();
        //     foreach (var item in stackableItemsList)
        //     {
        //         var pool = Instantiate(poolPrefab, this.transform);
        //         pool.SetObjectPoolType(item);
        //         itemsObjectPools.Add(pool);
        //         poolsDictionary[item.Type] = pool;
        //     }
        // }
        //
        // public void ReturnItemToItsPool(StackableItem item)
        // {
        //     var pool = poolsDictionary[item.Type];
        //     pool.ReturnToPool(item);
        // }
        //
        // public StackableItem GetFromPool(ItemType type)
        // {
        //     var pool = poolsDictionary[type];
        //     var item = pool.GetFromPool();
        //     return item;
        // }
    }
}