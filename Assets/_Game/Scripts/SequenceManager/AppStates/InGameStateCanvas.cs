using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using InGameUI = _Game.Scripts.UI.InGameUI;

namespace _Game.Scripts.SequenceManager.AppStates
{
    public class InGameStateCanvas : AbstractAppStateCanvas
    {
       // [SerializeField] private PlayerController playerController;
        private InGameUI inGameUI;

        public override void Initialize()
        {
            inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
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
            UIManager.Instance.DisableCanvas(CanvasTypes.InGame);
        }

        private void PauseGame()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.Pause);
        }
    }
}