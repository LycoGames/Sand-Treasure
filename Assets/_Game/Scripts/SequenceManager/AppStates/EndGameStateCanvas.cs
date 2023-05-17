using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.SequenceManager;
using _Game.Scripts.UI;

public class EndGameStateCanvas : AbstractAppStateCanvas
{
    private EndGameUI endGameUI;

    public override void Initialize()
    {
        endGameUI = UIManager.Instance.GetCanvas(CanvasTypes.EndGame) as EndGameUI;
        endGameUI.NextLevel = ChangeStateToLoading;
    }

    public override void Enter()
    {
        UIManager.Instance.EnableCanvas(CanvasTypes.EndGame);
    }

    public override void Exit()
    {
        UIManager.Instance.DisableCanvas(CanvasTypes.EndGame);
    }

    private void ChangeStateToLoading()
    {
        SequenceManager.Instance.ChangeState(AppStateTypes.Loading);
    }
}