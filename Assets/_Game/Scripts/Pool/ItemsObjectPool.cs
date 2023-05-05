using _Game.Scripts.Stack;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class ItemsObjectPool : GenericObjectPool<StackableItem>
    {
        [SerializeField] private Transform spawnPos;
        
        public StackableItem GetPrefab()
        {
            return base.objectToPool;
        }
        protected override void InitializePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(objectToPool, spawnPos);
                item.gameObject.SetActive(false);
                pooledObjects.Enqueue(item);
            }
        }
    }
}