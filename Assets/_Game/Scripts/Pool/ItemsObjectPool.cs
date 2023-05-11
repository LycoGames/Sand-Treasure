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

        public override StackableItem GetFromPool()
        {
            if (pooledObjects.Count>0)
            {
                var itemToReturn = pooledObjects.Dequeue();
                objectsInUse.Add(itemToReturn);
                //itemToReturn.gameObject.SetActive(true);
                return itemToReturn;
            }

            return CreateNewPooledItem();
        }

        public override void ReturnToPool(StackableItem item)
        {
            base.ReturnToPool(item);
            item.transform.parent = this.transform;
        }
    }
}