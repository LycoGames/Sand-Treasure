using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using _Game.Scripts.States;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Objects
{
    public class SellArea : MonoBehaviour
    {
        [SerializeField] private List<ItemType> itemsToGet = new();
        [SerializeField] private UIRewardVisualizer uiRewardVisualizer;
        [SerializeField] private AudioClip sellSFX;
        
        private StateController playerStateController;
        private StackManager playerStackManager;
        private Inventory playerInventory;
        private Dictionary<ItemType, Coroutine> coroutineDictionary = new();
        private WaitForSeconds waitForSeconds;

        void Start()
        {
            waitForSeconds = new WaitForSeconds(0.2f);
            InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            uiRewardVisualizer.SetDestination(inGameUI.MoneyPanel);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (playerStackManager == null)
                {
                    playerStackManager = other.GetComponent<StackManager>();
                    playerInventory = other.GetComponent<Inventory>();
                    playerStateController = other.GetComponent<StateController>();
                }

                Actions.onCollisionSellZone?.Invoke();
                //playerStateController.ChangeState(playerStateController.IdleState);
                SetCoroutineDictionary();
                SoundManager.Instance.Play(sellSFX);
            }
        }

        private void SetCoroutineDictionary()
        {
            coroutineDictionary = new Dictionary<ItemType, Coroutine>();
            foreach (var itemType in itemsToGet)
            {
                if (playerStackManager.CanGetFromStack(itemType))
                {
                    coroutineDictionary[itemType] = StartCoroutine(GetItemCoroutine(itemType));
                }
            }
        }

        private IEnumerator GetItemCoroutine(ItemType type)
        {
            while (playerStackManager.CanGetFromStack(type))
            {
                yield return waitForSeconds;
                GetItemFromPlayerStack(playerStackManager.Get(type));
            }

            StopTheCoroutine(type);
        }

        private void StopTheCoroutine(ItemType type)
        {
            Coroutine theCoroutine = coroutineDictionary[type];
            StopCoroutine(theCoroutine);
        }

        private void GetItemFromPlayerStack(StackableItem item)
        {
            item.transform.DOJump(transform.position, 5f, 1, 0.3f).SetAutoKill(true)
                .OnComplete(() =>
                {
                    uiRewardVisualizer.VisualiseReward(this.transform.position,
                        (() => playerInventory.AddMoney(item.Value)));
                    PoolManager.Instance.ReturnItemToItsPool(item);
                });
        }

        private void OnTriggerExit(Collider other)
        {
            StopAllCoroutines();
            Actions.onCollisionSellZone?.Invoke();
        }
    }
}