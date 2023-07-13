using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using _Game.Scripts.States;
using UnityEngine;

public class DigAreaEntrance : MonoBehaviour
{
    private StateController stateController;
    private PlayerController playerController;
    private BumpChecker bumpChecker;
    private PlayerSandAccumulator playerSandAccumulator;
    private PlayerMatColorChanger playerMatColorChanger;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (stateController == null)
        {
            stateController = other.GetComponent<StateController>();
            playerController = other.GetComponent<PlayerController>();
            bumpChecker = other.GetComponent<BumpChecker>();
            playerSandAccumulator = other.GetComponent<PlayerSandAccumulator>();
            playerMatColorChanger = other.GetComponent<PlayerMatColorChanger>();
        }

        // if (playerSandAccumulator.CanAccumulateSand())
        // {
        //     stateController.ChangeState(stateController.DigState);
        // }
        bumpChecker.StartCheckBumpCoroutine();
        //playerController.IncreaseMovementSpeed(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        //stateController.ChangeState(stateController.IdleState);
        bumpChecker.StopCheckBumpCoroutine();
        playerMatColorChanger.ChangeColor(false);
        playerController.IncreaseMovementSpeed(true,false);
    }
}