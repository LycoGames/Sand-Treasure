using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    private StateController stateController;

    private void OnTriggerEnter(Collider other)
    {
        stateController = other.GetComponent<StateController>();
        stateController.ChangeState(stateController.DigState);
    }

    private void OnTriggerExit(Collider other)
    {
        stateController.ChangeState(stateController.IdleState);
    }
}