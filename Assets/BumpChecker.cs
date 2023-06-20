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
    public bool isPlayerStackFull;

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
            if (isPlayerStackFull)
            {
                playerController.StopPlayer(isPlayerStackFull);
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