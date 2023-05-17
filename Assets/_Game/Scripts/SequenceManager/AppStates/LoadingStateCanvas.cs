using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.SequenceManager.AppStates
{
    public class LoadingStateCanvas : AbstractAppStateCanvas
    {
        [SerializeField] private float loadingTime;
        [SerializeField] private LevelLoader levelLoader;
        
        private LoadingUI loadingUI;

        public override void Initialize()
        {
            loadingUI = UIManager.Instance.GetCanvas(CanvasTypes.Loading) as LoadingUI;
        }

        public override void Enter()
        {
            UIManager.Instance.EnableCanvas(CanvasTypes.Loading);
            loadingUI.LoadingTime = loadingTime;
            levelLoader.LoadLevel();
            //TODO load level and exit loading
            Invoke("ExitLoading", loadingTime);
        }

        public override void Exit()
        {
            UIManager.Instance.DisableCanvas(CanvasTypes.Loading);
        }

        private void ExitLoading()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.InGame);
        }
    }
}