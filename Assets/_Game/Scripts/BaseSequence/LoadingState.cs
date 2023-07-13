using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.BaseSequence
{
    public class LoadingState : AbstractAppState
    {
        [SerializeField] private float loadingTime;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private PlayerController playerController;
        
        private LoadingUI loadingUI;

        public override void Initialize()
        {
            loadingUI = UIManager.Instance.GetCanvas(CanvasTypes.Loading) as LoadingUI;
        }

        public override void Enter()
        {
            UIManager.Instance.EnableCanvas(CanvasTypes.Loading);
            playerController.IsCanMove = false;
            loadingUI.LoadingTime = loadingTime;
            levelLoader.LoadLevel();
            Invoke("StartGame", loadingTime);
            AudioListener.pause = true;
        }

        public override void Exit()
        {
            UIManager.Instance.DisableCanvas(CanvasTypes.Loading);
            playerController.IsCanMove = true;
            AudioListener.pause = false;
        }

        private void StartGame()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.InGame);
        }
    }
}