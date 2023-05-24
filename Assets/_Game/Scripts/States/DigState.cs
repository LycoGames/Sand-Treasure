using _Game.Scripts.Interfaces;
using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts.States
{
    public class DigState : IState
    {
        private PlayerMeshHandler playerMeshHandler;
        private PlayerAnimator playerAnimator;
        private Effects effects;
        public void OnEnter(StateController controller)
        {
            if (playerMeshHandler == null|| playerAnimator==null)
            {
                playerMeshHandler = controller.GetComponent<PlayerMeshHandler>();
                playerAnimator = controller.GetComponent<PlayerAnimator>();
                effects = controller.GetComponent<Effects>();
            }
            effects.effect.Play(true);
            SoundManager.Instance.PlayLoop();
            playerAnimator.StartDigAnim();
            Debug.Log("entered dig state");
        }

        public void UpdateState(StateController controller)
        {
            playerMeshHandler.Dig();
        }

        public void OnExit(StateController controller)
        {
            playerAnimator.StopDigAnim();
            effects.effect.Stop(true);
            SoundManager.Instance.StopLoop();
            Debug.Log("exited dig state");
        }
    }
}