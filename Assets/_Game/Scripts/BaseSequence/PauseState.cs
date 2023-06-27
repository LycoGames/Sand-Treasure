using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;

namespace _Game.Scripts.BaseSequence
{
    public class PauseState : AbstractAppState
    {
        private PauseUI pauseUI;
        public override void Initialize()
        {
            pauseUI=UIManager.Instance.GetCanvas(CanvasTypes.Pause) as PauseUI;
        }

        public override void Enter()
        {
            UIManager.Instance.EnableCanvas(CanvasTypes.Pause);
        }

        public override void Exit()
        {
            UIManager.Instance.DisableCanvas(CanvasTypes.Pause);
        }

        private void ResetGame()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.Reset);
        }

        private void ResumeGame()
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.Counter);
        }
    }
}
