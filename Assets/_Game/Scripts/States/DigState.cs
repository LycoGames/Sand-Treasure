using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;

public class DigState : IState
{
    private PlayerMeshHandler playerMeshHandler;
    private PlayerAnimator playerAnimator;
    public void OnEnter(StateController controller)
    {
        if (playerMeshHandler == null|| playerAnimator==null)
        {
            playerMeshHandler = controller.GetComponent<PlayerMeshHandler>();
            playerAnimator = controller.GetComponent<PlayerAnimator>();
        }

        playerMeshHandler.StartTimerCoroutine();
        playerAnimator.StartDigAnim();
        Debug.Log("entered dig state");
    }

    public void UpdateState(StateController controller)
    {
        Debug.Log("update dig state");
        playerMeshHandler.Dig();
    }

    public void OnExit(StateController controller)
    {
        playerMeshHandler.StopTimerCoroutine();
        playerAnimator.StopDigAnim();
        Debug.Log("exited dig state");
    }
}