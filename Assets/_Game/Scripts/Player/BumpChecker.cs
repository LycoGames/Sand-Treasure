using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using DG.Tweening;
using UnityEngine;

public class BumpChecker : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float checkDelay;
    [SerializeField] private float maxDistance;

    private WaitForSeconds waitForSeconds;
    public bool higherLevelArea;
    public bool isPlayerCapacityFull;

    void Start()
    {
        waitForSeconds = new WaitForSeconds(checkDelay);
    }

    public void StartCheckBumpCoroutine()
    {
        StartCoroutine(CheckBumpCoroutine());
    }

    public void StopCheckBumpCoroutine()
    {
        StopAllCoroutines();
    }

    public void ChangeIsPlayerCapacityFull(bool isPlayerFull)
    {
        isPlayerCapacityFull = isPlayerFull;
        ChangeMaxDistance(isPlayerFull);
    }

    private void ChangeMaxDistance(bool isPlayerFull)
    {
        maxDistance = isPlayerFull ? 4 : 7;
    }

    private IEnumerator CheckBumpCoroutine()
    {
        while (true)
        {
            yield return waitForSeconds;
            CheckBump();
        }
    }

    private void CheckBump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance,
                layerMask))
        {
            if (isPlayerCapacityFull)
            {
                playerController.StopPlayer(isPlayerCapacityFull);
            }
            else
            {
                playerController.IncreaseMovementSpeed(false, higherLevelArea);
            }
        }
        else
        {
            playerController.IncreaseMovementSpeed(true, false);
        }
    }
}