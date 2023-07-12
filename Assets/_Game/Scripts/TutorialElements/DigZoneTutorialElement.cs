using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Utils.LineRendererGPS;
using UnityEngine;

public class DigZoneTutorialElement : TutorialElement
{
    private PlayerSandAccumulator playerSandAccumulator;

    //[SerializeField] private Line lineRenderer;
    private bool isEntered;

    public void Initialize(PlayerSandAccumulator playerSandAccumulator)
    {
        this.playerSandAccumulator = playerSandAccumulator;
        SubscribeToAction();
    }

    private void SubscribeToAction()
    {
        playerSandAccumulator.OnCapacityFull += TutorialCompleted;
    }

    private void TutorialCompleted()
    {
        ConditionComplete();
        //    lineRenderer.StartCoroutine();
        playerSandAccumulator.OnCapacityFull -= TutorialCompleted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEntered) return;
        if (other.CompareTag("Player"))
        {
            isEntered = true;
            ReachedToDestination();
            // lineRenderer.StopCoroutine();
        }
    }
}