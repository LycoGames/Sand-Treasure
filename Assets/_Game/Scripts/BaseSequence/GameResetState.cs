using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;

namespace _Game.Scripts.BaseSequence
{
    public class GameResetState : AbstractAppState
    {
        public override void Enter()
        {
            UIManager.Instance.EnableCanvas(CanvasTypes.Loading);
            SequenceManager.Instance.ChangeState(AppStateTypes.Loading);
        }

        public override void Exit()
        {
            UIManager.Instance.DisableCanvas(CanvasTypes.Loading);
        }
    }
}