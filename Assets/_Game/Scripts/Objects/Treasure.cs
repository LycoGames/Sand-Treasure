using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using RDG;
using UnityEngine;

namespace _Game.Scripts.Objects
{
    public class Treasure : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private UIRewardVisualizer rewardVisualizer;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private AudioClip treasureFoundSFX;
        [SerializeField] private int moneyValue;
        
        private static readonly int OpenChest = Animator.StringToHash("OpenChest");

        private void Start()
        {
            InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            rewardVisualizer.SetDestination(inGameUI.MoneyPanel);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.PlayOneShot(treasureFoundSFX);
                GameManager.Instance.Vibrate(100,200,true);
                boxCollider.enabled = false;
                animator.SetTrigger(OpenChest);
                Inventory playerInventory = other.GetComponent<Inventory>();
                this.gameObject.transform.DOJump(other.transform.position, 5f, 1, 0.3f).SetAutoKill(true).OnComplete(() =>
                {
                    rewardVisualizer.VisualiseReward(this.gameObject.transform.position, (() => playerInventory.AddMoney(moneyValue)));
                    Destroy(gameObject);
                });
            }
        }
    }
}