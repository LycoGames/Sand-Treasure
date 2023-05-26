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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (stateController == null)
        {
            stateController = other.GetComponent<StateController>();
            playerController = other.GetComponent<PlayerController>();
        }

        stateController.ChangeState(stateController.DigState);
        playerController.IncreaseMovementSpeed(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        stateController.ChangeState(stateController.IdleState);
        playerController.IncreaseMovementSpeed(true);
    }
}