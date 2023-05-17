using _Game.Scripts.Interfaces;
using UnityEngine;

namespace _Game.Scripts.States
{
    public class StateController : MonoBehaviour
    {
        IState currentState;
        public DigState DigState = new DigState();
        public IdleState IdleState = new IdleState();
        public IState CurrentState => currentState;

        private void Start()
        {
            currentState = IdleState;
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
}