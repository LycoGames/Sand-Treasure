using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Player;
using _Game.Scripts.States;
using DG.Tweening;
using UnityEngine;

public struct RayData
{
    public Vector3 rayDir;
    public float maxDistance;

    public RayData(Vector3 rayDir, float maxDistance)
    {
        this.rayDir = rayDir;
        this.maxDistance = maxDistance;
    }
}

public class BumpChecker : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float checkDelay;
    [SerializeField] private float maxDistance;
    [SerializeField] private float sideMaxDistance;
    [SerializeField] private float sideRayAngle;
    [SerializeField] private StateController playerState;

    private WaitForSeconds waitForSeconds;
    public bool isPlayerOnHigherLevelArea;
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
        maxDistance = isPlayerFull ? 4 : 11;
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
        Vector3 leftDir = Quaternion.AngleAxis(-sideRayAngle, Vector3.up) * transform.forward;
        Vector3 rightDir = Quaternion.AngleAxis(sideRayAngle, Vector3.up) * transform.forward;
        List<RayData> rayDataList = new List<RayData>
        {
            new(transform.forward, maxDistance),
            new(leftDir, sideMaxDistance),
            new(rightDir, sideMaxDistance)
        };
        // Debug.DrawRay(transform.position, leftDir * sideMaxDistance, Color.blue, .1f); //todo silinecek
        // Debug.DrawRay(transform.position, rightDir * sideMaxDistance, Color.blue, .1f); //todo silinecek
        // Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.blue, .1f); //todo silinecek
        bool hasHit = rayDataList.Any(rayData =>
            Physics.Raycast(transform.position, rayData.rayDir, out hit, rayData.maxDistance, layerMask));
        if (hasHit)
        {
            if (isPlayerCapacityFull) //player fullse ve önümde kum varsa dur ilerleyeme
            {
                playerController.StopPlayer(isPlayerCapacityFull);
            }
            else //önümde kum varsa hızımı yavaslat, yüksek seviyeli yerdeysem daha da yavaslat
            {
                playerState.ChangeState(playerState.DigState);
                playerController.IncreaseMovementSpeed(false, isPlayerOnHigherLevelArea);
            }
        }
        else //önümde kum yoksa idle state geç ve hızımı arttır.
        {
            playerState.ChangeState(playerState.IdleState);
            playerController.IncreaseMovementSpeed(true, false);
        }
    }
}