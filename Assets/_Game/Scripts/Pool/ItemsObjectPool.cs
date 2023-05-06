using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Stack;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class ItemsObjectPool : GenericObjectPool<StackableItem>
    {
        [SerializeField] private Transform spawnPos;
        [SerializeField] private ItemType poolType;
        public ItemType PoolType => poolType;

        public StackableItem GetPrefab()
        {
            return base.objectToPool;
        }

        public void SetObjectPoolType(StackableItem stackableItem)
        {
            base.objectToPool = stackableItem;
            poolType = stackableItem.Type;
        }
    }
}