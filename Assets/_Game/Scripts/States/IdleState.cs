using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts.States
{
    public class IdleState : IState
    {
        public void OnEnter(StateController controller)
        {
            Debug.Log("entered idle state");
        }

        public void UpdateState(StateController controller)
        {
            Debug.Log("update idle state");
        }

        public void OnExit(StateController controller)
        {
            Debug.Log("exited idle");
        }
    }
}