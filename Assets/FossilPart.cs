using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class FossilPart : MonoBehaviour
{
    [SerializeField] private BoneType boneType;
    [SerializeField] private BoxCollider myCollider;
    
    public BoneType BoneType => boneType;

    private Vector3 destination;

    public Action<FossilPart> OnCollected;

    // private CinemachineVirtualCamera cinemachineVirtualCamera;
    // private Vector3 destination = new Vector3(-16f, 2.22f, -61f);
    //
    // private void Awake()
    // {
    //     cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    //     print(cinemachineVirtualCamera);
    // }
    public void Setup(Vector3 destination,Action<FossilPart> OnCollect)
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

        OnCollected?.Invoke(this);
        DOTween.Sequence()
            .Append(this.transform.DOMoveY(4, 0.5f))
            .Append(this.transform.DOMove(destination, 2f))
            .OnComplete(() =>
            {
                GameManager.Instance.ChangeCamFollowTarget(other.transform);
                myCollider.isTrigger = false;
            });
    }

}