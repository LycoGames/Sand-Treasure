using _Game.Scripts.Base.AppState;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.SequenceManager;
using _Game.Scripts.UI;
using UnityEngine;

public class EndGameState : AbstractAppState
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Effects effects;
    [SerializeField] private AudioClip levelCompleteSFX;
    
    private EndGameUI endGameUI;

    public override void Initialize()
    {
        endGameUI = UIManager.Instance.GetCanvas(CanvasTypes.EndGame) as EndGameUI;
        endGameUI.NextLevel = ChangeStateToLoading;
    }

    public override void Enter()
    {
        UIManager.Instance.EnableCanvas(CanvasTypes.EndGame);
        effects.effect.Play(true);
        SoundManager.Instance.PlayOneShot(levelCompleteSFX);
    }

    public override void Exit()
    {
        UIManager.Instance.DisableCanvas(CanvasTypes.EndGame);
        effects.effect.Stop(true);
    }

    private void ChangeStateToLoading()
    {
        levelLoader.OnLevelComplete();
        SequenceManager.Instance.ChangeState(AppStateTypes.Loading);
    }
}