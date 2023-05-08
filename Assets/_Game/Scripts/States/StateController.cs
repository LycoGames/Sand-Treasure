using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;


public class StateController : MonoBehaviour
{
    IState currentState;
    public DigState DigState = new DigState();
    public MovementState MovementState = new MovementState();
    public IState CurrentState => currentState;

    private void Start()
    {
        currentState = MovementState;
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;
        currentState.OnEnter(this);
    }
}