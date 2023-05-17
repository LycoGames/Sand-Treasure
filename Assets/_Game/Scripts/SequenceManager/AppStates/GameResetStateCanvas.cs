using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;

namespace _Game.Scripts.SequenceManager.AppStates
{
    public class GameResetStateCanvas : AbstractAppStateCanvas
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
