using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using Cinemachine;
using DG.Tweening;
using RDG;
using UnityEngine;

public class FossilPart : MonoBehaviour
{
    [SerializeField] private BoneType boneType;
    [SerializeField] private BoxCollider myCollider;

    public BoneType BoneType => boneType;

    private Vector3 destination;

    public Action<FossilPart> OnCollected;

    public Action<FossilPart> OnSequenceComplete;
    
    public void Setup(Vector3 destination, Action<FossilPart> OnCollect)
    {
        OnCollected = OnCollect;
        this.destination = destination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerController playerController = other.GetComponent<PlayerController>();
        playerController.IsCanMove = false;
        OnCollected?.Invoke(this);
        GameManager.Instance.Vibrate(100,200,true);
        DOTween.Sequence()
            .Append(this.transform.DOMoveY(4, 0.5f))
            .Append(this.transform.DOMove(destination, 2f))
            .OnComplete(() =>
            {
                OnSequenceComplete?.Invoke(this);
                myCollider.isTrigger = false;
                Destroy(this.gameObject,0.5f);
                playerController.IsCanMove = true;
            });
    }
}