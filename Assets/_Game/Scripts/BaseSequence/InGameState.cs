using System;
using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.BaseSequence
{
    public class InGameState : AbstractAppState
    {
        // [SerializeField] private PlayerController playerController;
        private InGameUI inGameUI;
        [SerializeField] private Stats playerStats;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private PlayerSandAccumulator playerSandAccumulator;

        public Action OnReachedFinish;
        [HideInInspector] public bool isFinished;
        private int isVibrationOn;

        public override void Initialize()
        {
            inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            inGameUI.AudioToggle = AudioOnOff;
            GetVibrationSettings();
            inGameUI.VibrationToggle = VibrationOnOff;
            inGameUI.CapacityBar.Initialize(playerStats);
            //inGameUI.Pause = PauseGame;
            //  playerController.OnPositionChange += inGameUI.SetDistanceText;
        }

        private void GetVibrationSettings()
        {
            isVibrationOn = PlayerPrefs.HasKey("Vibration") ? PlayerPrefs.GetInt("Vibration") : 1;
            inGameUI.VibrationToggleButton.isOn = isVibrationOn == 1;
            GameManager.Instance.ToggleVibration(isVibrationOn == 1);
        }

        public override void Enter()
        {
            //   playerController.canControl = true;
            playerSandAccumulator.Initialize();
            UpdateProgressBar(levelLoader.GetCompletionPercentage());
            UIManager.Instance.EnableCanvas(CanvasTypes.InGame);
            inGameUI.FingerAnim.EnableFingerAnim();
            inGameUI.FingerTransform.DOScale(1.25f, 0.7f).OnComplete((() => inGameUI.FingerTransform.DOScale(1f,0.7f)));
            inGameUI.LevelBar.DOScale(1.25f, 0.7f).OnComplete((() => inGameUI.LevelBar.DOScale(1f,0.7f)));
            inGameUI.ProgressBarTransform.DOScale(1.25f, 0.7f).OnComplete((() => inGameUI.ProgressBarTransform.DOScale(1f,0.7f)));
            Actions.OnInGameStateBegin?.Invoke();
            SoundManager.Instance.PlayLoopEngine();
        }

        public override void Exit()
        {
            // playerController.canControl = false;
            //UIManager.Instance.DisableCanvas(CanvasTypes.InGame);
            SoundManager.Instance.StopEngine();
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

        private void VibrationOnOff(bool isOn)
        {
            GameManager.Instance.ToggleVibration(isOn);
            switch (isOn)
            {
                case true:
                    PlayerPrefs.SetInt("Vibration", 1);
                    break;
                case false:
                    PlayerPrefs.SetInt("Vibration", 0);
                    break;
            }
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