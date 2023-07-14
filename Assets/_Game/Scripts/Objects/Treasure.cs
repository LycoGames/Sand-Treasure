using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.Saving;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using RDG;
using UnityEngine;

namespace _Game.Scripts.Objects
{
    public class Treasure : MonoBehaviour, ISaveable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private UIRewardVisualizer rewardVisualizer;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private AudioClip treasureFoundSFX;
        [SerializeField] private int moneyValue;
        [SerializeField] private GameObject mGameObject;
        [SerializeField] private BoxCollider myBoxCollider;

        private bool hasCollected;
        private static readonly int OpenChest = Animator.StringToHash("OpenChest");

        private void Start()
        {
            InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            rewardVisualizer.SetDestination(inGameUI.MoneyPanel);
            Actions.OnInGameStateBegin += HandleSelf;
            Actions.OnGoNextLevel += () => hasCollected = false;
        }

        private void HandleSelf()
        {
            if (hasCollected)
            {
                DisableTreasure();
            }
        }

        private void DisableTreasure()
        {
            mGameObject.SetActive(false);
            myBoxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.PlayOneShot(treasureFoundSFX);
                GameManager.Instance.Vibrate(100, 200, true);
                hasCollected = true;
                boxCollider.enabled = false;
                animator.SetTrigger(OpenChest);
                Inventory playerInventory = other.GetComponent<Inventory>();
                this.gameObject.transform.DOJump(other.transform.position, 5f, 1, 0.3f).SetAutoKill(true).OnComplete(
                    () =>
                    {
                        rewardVisualizer.VisualiseReward(this.gameObject.transform.position,
                            (() => playerInventory.AddMoney(moneyValue)));
                        DisableTreasure();
                        //Destroy(gameObject);
                    });
            }
        }

        public object CaptureState()
        {
            return hasCollected;
        }

        public void RestoreState(object state)
        {
            hasCollected = (bool)state;
        }
    }
}