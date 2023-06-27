using System;
using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.BaseSequence
{
    public class InGameState : AbstractAppState
    {
        // [SerializeField] private PlayerController playerController;
        private InGameUI inGameUI;
        [SerializeField] private Stats playerStats;
        [SerializeField] private LevelLoader levelLoader;
        
        public Action OnReachedFinish;
        [HideInInspector] public bool isFinished;

        public override void Initialize()
        {
            inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            inGameUI.AudioToggle = AudioOnOff;
            inGameUI.CapacityBar.Initialize(playerStats);
            //inGameUI.Pause = PauseGame;
            //  playerController.OnPositionChange += inGameUI.SetDistanceText;
        }

        public override void Enter()
        {
            //   playerController.canControl = true;
            UpdateProgressBar(levelLoader.GetCompletionPercentage());
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

        public void UpdateProgressBar(float value)
        {
            value = value / 100;
            inGameUI.UpdateProgressBar(value);
            if (value >= 0.5f && isFinished == false)
            {
                isFinished = true;
                GoEndGameState();
            }
        }
    }
}