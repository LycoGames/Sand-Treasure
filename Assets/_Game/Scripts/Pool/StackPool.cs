using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Stack;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class StackPool : MonoBehaviour
    {
        [SerializeField] private List<StackData> stackPool;
        [SerializeField] private StackData prefab;


        public void InitializeStackPool(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                StackData instanceStack = Instantiate(prefab, transform);
                instanceStack.gameObject.SetActive(false);
                stackPool.Add(instanceStack);
            }
        }

        public StackData GetStackFromPool()
        {
            foreach (StackData stackInstance in stackPool.Where(stackInstance =>
                         stackInstance.gameObject.activeInHierarchy == false))
            {
                stackInstance.gameObject.SetActive(true);
                return stackInstance;
            }

            return null;
        }
    }
}