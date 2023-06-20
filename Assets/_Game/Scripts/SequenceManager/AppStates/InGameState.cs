using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using UnityEngine;


namespace _Game.Scripts.SequenceManager.AppStates
{
    public class InGameState : AbstractAppState
    {
        // [SerializeField] private PlayerController playerController;
        private InGameUI inGameUI;
        [SerializeField] private Stats playerStats;
        
        public override void Initialize()
        {
            inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            inGameUI.AudioToggle = AudioOnOff;
            inGameUI.OnReachedFinish += GoEndGameState;
            inGameUI.CapacityBar.Initialize(playerStats);
            //inGameUI.Pause = PauseGame;
            //  playerController.OnPositionChange += inGameUI.SetDistanceText;
        }

        public override void Enter()
        {
            //   playerController.canControl = true;
            UIManager.Instance.EnableCanvas(CanvasTypes.InGame);
        }

        public override void Exit()
        {
            // playerController.canControl = false;
            //UIManager.Instance.DisableCanvas(CanvasTypes.InGame);
        }

        private void GoEndGameState()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.EndGame);
        }
        private void PauseGame()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.Pause);
        }

        private void AudioOnOff(bool isOn)
        {
            AudioListener.pause = !isOn;
        }
    }
}