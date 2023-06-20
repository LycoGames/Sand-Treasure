using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.Pool;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

public class SandCubes : MonoBehaviour
{
    [SerializeField] private Rigidbody myRb;
    public Rigidbody Rigidbody => myRb;
    [SerializeField] private SphereCollider sphereCollider;
    public SphereCollider SphereCollider => sphereCollider;
    [SerializeField] private UIRewardVisualizer rewardVisualizer;
    public UIRewardVisualizer RewardVisualizer => rewardVisualizer;
    [SerializeField] private SandType sandType;
    public SandType SandType => sandType;
    [SerializeField] private int value;
    public int Value => value;
    [SerializeField] private Effects effect;
    public Effects Effect =>effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory playerInventory = other.GetComponent<Inventory>();
            rewardVisualizer.VisualiseReward(this.transform.position, (() => playerInventory.AddMoney(value)));
            this.transform.DOMove(other.transform.position, 0.2f)
                .OnComplete((() =>
                {
                    myRb.isKinematic = true;
                    sphereCollider.isTrigger = false;
                    PoolManager.Instance.ReturnItemToItsPool(this);
                }));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}