using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using UnityEngine;

public class BumpChecker : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float checkDelay;
    [SerializeField] private float maxDistance;

    private WaitForSeconds waitForSeconds;

    void Start()
    {
        waitForSeconds = new WaitForSeconds(checkDelay);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxDistance, Color.yellow);
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxDistance, Color.yellow);
            print("hitted");
            playerController.IncreaseMovementSpeed(false);
        }
        else
        {
            playerController.IncreaseMovementSpeed(true);
            print("no hit");
        }
    }
}