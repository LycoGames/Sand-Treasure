using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private UIRewardVisualizer rewardVisualizer;
    [SerializeField] private BoxCollider boxCollider;

    private static readonly int OpenChest = Animator.StringToHash("OpenChest");


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxCollider.enabled = false;
            animator.SetTrigger(OpenChest);
            Inventory playerInventory = other.GetComponent<Inventory>();
            this.gameObject.transform.DOJump(other.transform.position, 5f, 1, 0.3f).SetAutoKill(true).OnComplete(() =>
            {
                rewardVisualizer.VisualiseReward(this.gameObject.transform.position, (() => playerInventory.AddTreasure()));
                Destroy(gameObject);
            });
        }
    }
}