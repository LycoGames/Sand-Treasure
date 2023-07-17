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
        private float cooldown = 0.25f;
        private float time=0;
        public void OnEnter(StateController controller)
        {
            if (playerMeshHandler == null || playerAnimator == null)
            {
                playerMeshHandler = controller.GetComponent<PlayerMeshHandler>();
                playerAnimator = controller.GetComponent<PlayerAnimator>();
                effects = controller.GetComponent<Effects>();
            }

            effects.effect.Play();
            SoundManager.Instance.PlayLoop();
            playerAnimator.StartDigAnim();
            Debug.Log("entered dig state");
        }

        public void UpdateState(StateController controller)
        {
            playerMeshHandler.Dig();
            time += Time.deltaTime;
            if (time>=cooldown)
            {
                GameManager.Instance.Vibrate(40,40,true);
                time = 0;
            }
        }

        public void OnExit(StateController controller)
        {
            playerAnimator.StopDigAnim();
            effects.effect.Stop();
            SoundManager.Instance.StopLoop();
            Debug.Log("exited dig state");
        }
    }
}