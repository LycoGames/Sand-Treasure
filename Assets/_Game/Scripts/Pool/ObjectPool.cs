using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Stack;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private StackableItem prefab;

        private readonly Stack<StackableItem> pool = new();

        public StackableItem Get()
        {
            if (!pool.Any())
                Add();

            return pool.Pop();
        }

        public void Add(StackableItem item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(transform);

            pool.Push(item);
        }

        public StackableItem GetPrefab()
        {
            return prefab;
        }

        private void Add()
        {
            var item = Instantiate(prefab, transform.position, Quaternion.identity);
            item.Setup(this);
            Add(item);
        }
    }
}