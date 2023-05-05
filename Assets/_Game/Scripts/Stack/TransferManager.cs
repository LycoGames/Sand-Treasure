using System;
using System.Collections;
using _Game.Scripts.Control;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Stack
{
    public class TransferManager : MonoBehaviour
    {
        public Action onGetTransferCompleted;
        public Action onGiveTransferCompleted;

        public delegate void TransferManagerWealthCollectDelegate(int value);

        public event TransferManagerWealthCollectDelegate OnMoneyCollected;
        public event TransferManagerWealthCollectDelegate OnDiamondCollected;

        [SerializeField] private StackManager stackManager;
       // [SerializeField] private EffectHandler effectHandler;


        [SerializeField] private float focusTime = 0.5f;
      //  [SerializeField] private Canvas maxTextPrefab;

        private float capacityToTransferSpeedDivider = 5f;

        private Stats stats;
        private Canvas maxCanvas;
        private Coroutine transferCoroutine;
        private float transferSpeed;
        private WaitForSeconds transferDelayWaitForSeconds;
        private const float PickupDelay = 0.2f;
        private WaitForSeconds pickupDelayWaitForSeconds;

        private void Awake()
        {
            SetStats();
            pickupDelayWaitForSeconds = new WaitForSeconds(PickupDelay);
        }

        private void OnEnable()
        {
            stats.OnStackCapacityChange += UpdateTransferSpeed;
        }


        private void OnDisable()
        {
            stats.OnStackCapacityChange -= UpdateTransferSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IStackItemCollector iStackItemCollector))
            {
                transferCoroutine = StartCoroutine(GiveItems(iStackItemCollector));
            }
            else if (other.TryGetComponent(out IStackItemGiver iStackItemGiver))
            {
                transferCoroutine = StartCoroutine(iStackItemGiver.GetItemType() == ItemType.Money
                    ? PickupItems(iStackItemGiver)
                    : GetItems(iStackItemGiver));
            }
            else if (other.TryGetComponent(out StackableItem stackableItem))
            {
                PickupItem(stackableItem);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IStackItemGiver _))
            {
                StopTransferCoroutine();
                onGetTransferCompleted?.Invoke();
            }
            else if (other.TryGetComponent(out IStackItemCollector _))
            {
                StopTransferCoroutine();
                onGiveTransferCompleted?.Invoke();
            }
        }

        private void StopTransferCoroutine()
        {
            if (transferCoroutine == null) return;
            StopAllCoroutines();
            transferCoroutine = null;
        }

        private IEnumerator PickupItems(IStackItemGiver iStackItemGiver)
        {
            while (iStackItemGiver.CanGetItem())
            {
                PickupItem(iStackItemGiver.Get());
                yield return pickupDelayWaitForSeconds;
            }
        }

        private IEnumerator GiveItems(IStackItemCollector iStackItemCollector)
        {
            ItemType otherType = iStackItemCollector.GetItemType();
            yield return new WaitForSeconds(focusTime);
            while (stackManager.CanGetFromStack(otherType))
            {
                if (iStackItemCollector.CanAddItem())
                {
                    iStackItemCollector.Add(stackManager.Get(otherType));
                }

                yield return transferDelayWaitForSeconds;
            }

            transferCoroutine = null;
            onGiveTransferCompleted?.Invoke();
        }

        private IEnumerator GetItems(IStackItemGiver iStackItemGiver)
        {
            ItemType otherType = iStackItemGiver.GetItemType();

            yield return new WaitForSeconds(focusTime);
            while (stackManager.CanAddToStack(otherType))
            {
                if (iStackItemGiver.CanGetItem())
                {
                    if (otherType == ItemType.RepairPart)
                    {
                     //   effectHandler.PlayThirdSoundEffect();
                    }

                    stackManager.Add(iStackItemGiver.Get(), transferSpeed);
                }

                yield return transferDelayWaitForSeconds;
            }
            //todo stack max ui
            // if (!stackManager.CanAddToStack(otherType))
            // {
            //     EnableMaxUI(stackManager.GetStackHolder(otherType), stackManager.GetStackPeakPosition(otherType));
            // }

            transferCoroutine = null;
            onGetTransferCompleted?.Invoke();
        }

        private void UpdateTransferSpeed(float stackCapacity)
        {
            transferSpeed = capacityToTransferSpeedDivider / stackCapacity;
            SetTransferDelayWaitForSeconds();
        }

        private void PickupItem(StackableItem stackableItem)
        {
            stackableItem.transform.DOMove(transform.position + new Vector3(0, 2f, 0), PickupDelay)
                .SetAutoKill(true)
                .OnComplete(() =>
                {
                    GetWealth(stackableItem);
                    stackableItem.ReSendToPool();
                });
        }

        private void GetWealth(StackableItem stackableItem)
        {
            switch (stackableItem.Type)
            {
                case ItemType.Money:
                    OnMoneyCollected?.Invoke(stackableItem.Value);
                   // effectHandler.PlaySoundEffect();
                   // inGameCanvas.DoPunchWealthPanel(ItemType.Money);
                    break;
                case ItemType.Diamond:
                    OnDiamondCollected?.Invoke(stackableItem.Value);
                   // effectHandler.PlaySecondSoundEffect();
                  //  inGameCanvas.DoPunchWealthPanel(ItemType.Diamond);
                    break;
                default:
                    break;
            }
        }

        private void SetStats()
        {
            stats = GetComponent<Stats>();
        }

        private void SetTransferDelayWaitForSeconds()
        {
            transferDelayWaitForSeconds = new WaitForSeconds(transferSpeed);
        }

        // private void EnableMaxUI(Transform parent, Vector3 position)
        // {
        //     if (maxCanvas == null)
        //         maxCanvas = Instantiate(maxTextPrefab, parent);
        //     else
        //     {
        //         maxCanvas.enabled = true;
        //     }
        //
        //     maxCanvas.transform.position = position + new Vector3(0, 1, 0);
        // }
        //
        // private void DisableMaxUI()
        // {
        //     if (maxCanvas.enabled)
        //         maxCanvas.enabled = false;
        // }
    }
}