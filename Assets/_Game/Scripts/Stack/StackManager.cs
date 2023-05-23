using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.StatSystem;
using _Game.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Stack
{
    [RequireComponent(typeof(Stats))]
    public class StackManager : MonoBehaviour
    {
        public Action<ItemType> OnItemCollected;

        private readonly List<StackData> stackList = new List<StackData>();
        [SerializeField] private StackPool stackPool;
        [SerializeField] private int rowCount = 1;
        [SerializeField] private int columnCount = 1;
        [SerializeField] private bool reverseAlign;
        [SerializeField] private float offsetX = 0.5f;
        [SerializeField] private float offsetZ = 0.5f;
        [SerializeField] private TextMeshProUGUI maxText;

        private Stats stats;

        private void Awake()
        {
            SetStats();
            maxText.enabled = false;
        }

        private void OnEnable()
        {
            InitializeStackPool(stats.GetStat(Stat.StackLimit));
            stats.OnStackLimitChange += InitializeStackPool;
        }

        private void OnDisable()
        {
            stats.OnStackLimitChange -= InitializeStackPool;
        }

        public void Add(StackableItem stackableItem, float duration)
        {
            StackData stackData = GetStackByType(stackableItem.Type);
            if (stackData == null || stackData.Stack.Count == GetStackCapacity())
                stackData = InitializeStack(stackableItem);
            GetStackByType(stackableItem.Type).Stack.Push(stackableItem);
            OnItemCollected?.Invoke(stackableItem.Type);
            stackableItem.PickUp(stackData);


            stackableItem.MoveCoroutine = stackableItem.StartCoroutine(ParabolicMover.Instance.Coroutine(
                stackableItem.transform,
                stackableItem.transform.localPosition, 1f,
                duration,
                GetStackPosition(stackableItem.Offset, stackData.Stack.Count)));

            if (IsStackFull() == true)
            {
                maxText.enabled = true;
            }
        }

        public StackableItem Get(ItemType type)
        {
            StackData stackData = GetStackByType(type);
            StackableItem stackableItem = stackData.Stack.Pop();

            if (stackData.Stack.Count <= 0)
                DeActivateStack(stackData);
            if (IsStackFull()==false)
            {
                maxText.enabled = false;
            }
            return stackableItem;
        }

        public bool CanAddToStack(ItemType type)
        {
            StackData stackData = GetStackByType(type);
            if (stackData != null && stackData.Stack.Count < GetStackCapacity())
                return true;

            return stackList.Count < GetStackLimit();
        }

        public bool IsStackFull()
        {
            if (stackList.Count<GetStackLimit())
            {
                return false;
            }
            foreach (var stack in stackList)
            {
                if (stack.Stack.Count < GetStackCapacity())
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanGetFromStack(ItemType type)
        {
            StackData stackData = GetStackByType(type);
            return stackData != null;
        }

        public Vector3 GetStackPeakPosition(ItemType type)
        {
            return GetStackByType(type).Stack.Peek().transform.position;
        }

        public Transform GetStackHolder(ItemType type)
        {
            return GetStackByType(type).transform;
        }

        private void InitializeStackPool(float limit)
        {
            stackPool.InitializeStackPool((int)limit);
        }

        private StackData InitializeStack(StackableItem stackableItem)
        {
            StackData stackData = stackPool.GetStackFromPool();
            if (stackData == null)
            {
                print("Stackdata is null");
            }

            if (stackableItem == null)
            {
                print("Stackableitem is null");
            }

            stackData.StackType = stackableItem.Type;
            stackList.Add(stackData);
            UpdateStackPositions();
            return stackData;
        }

        private void DeActivateStack(StackData stackData)
        {
            stackList.Remove(stackData);
            stackData.gameObject.SetActive(false);
            UpdateStackPositions();
        }

        private StackData GetStackByType(ItemType type)
        {
            StackData stackData = null;
            foreach (var stackDataInstance in stackList.Where(stackDataInstance => stackDataInstance.StackType == type))
            {
                if (stackData == null)
                    stackData = stackDataInstance;

                else if (stackData.Stack.Count > stackDataInstance.Stack.Count)
                {
                    stackData = stackDataInstance;
                }
            }

            return stackData;
        }

        private void UpdateStackPositions()
        {
            if (stackList.Count == 0)
                return;

            if (stackList.Count % 2 == 1)
            {
                foreach (StackData stack in stackList)
                {
                    if (stackList.IndexOf(stack) == stackList.Count - 1)
                    {
                        stack.transform.localPosition =
                            new Vector3(0, 0, stackList.IndexOf(stack) / 2 * offsetZ);
                        return;
                    }

                    stack.transform.localPosition = GetStackHolderPosition(stack);
                }
            }
            else
            {
                foreach (StackData stack in stackList.ToArray())
                {
                    stack.transform.localPosition = GetStackHolderPosition(stack);
                }
            }
        }

        private Vector3 GetStackHolderPosition(StackData stack)
        {
            return new Vector3((stackList.IndexOf(stack) % 2 == 0 ? -1 : 1) * offsetX, 0,
                stackList.IndexOf(stack) / 2 * offsetZ);
        }

        private Vector3 GetStackPosition(Vector3 itemBounds, int stackItemCount)
        {
            return new Vector3(itemBounds.x * ((stackItemCount - 1) % columnCount),
                itemBounds.y * ((stackItemCount - 1) / (rowCount * columnCount)),
                (reverseAlign ? -1 : 1) * -itemBounds.z * ((stackItemCount - 1) / columnCount % rowCount));
        }

        private int GetStackCapacity()
        {
            return (int)stats.GetStat(Stat.StackCapacity);
        }

        private int GetStackLimit()
        {
            int stackLimit = (int)stats.GetStat(Stat.StackLimit);
            return stackLimit;
        }

        private void SetStats()
        {
            stats = GetComponent<Stats>();
        }
    }
}