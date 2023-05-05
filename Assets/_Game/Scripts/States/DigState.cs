using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;

public class DigState : IState
{
    private PlayerMeshHandler playerMeshHandler;
    public void OnEnter(StateController controller)
    {
        if (playerMeshHandler==null)
        {
            playerMeshHandler = controller.GetComponent<PlayerMeshHandler>();
        }
        playerMeshHandler.StartTimerCoroutine();
        Debug.Log("entered dig state");
    }

    public void UpdateState(StateController controller)
    {
        Debug.Log("update dig state");
        playerMeshHandler.Dig();
    }

    public void OnHurt(StateController controller)
    {
        Debug.Log("dig hurt");
    }

    public void OnExit(StateController controller)
    {
        playerMeshHandler.StopTimerCoroutine();
        Debug.Log("exited dig state");
    }
}