using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using _Game.Scripts.BaseSequence;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.States;
using _Game.Scripts.UI;
using Cinemachine;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    private int totalTreasureCount;
    private int foundedTreasureCount;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private InGameState inGameState;
    
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerStartingPos;
    [SerializeField] private StateController playerStateController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerMatColorChanger playerMatColorChanger;
    [SerializeField] private PlayerUpgradesUI playerUpgradesUI;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    protected override void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void IncreaseFoundedTreasureCount()
    {
        foundedTreasureCount++;
        if (foundedTreasureCount >= totalTreasureCount)
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.EndGame);
        }
    }

    public void ResetTreasureCount()
    {
        inventory.ResetTreasureCount();
        foundedTreasureCount = 0;
        inGameUI.SetFoundedTreasureCount(foundedTreasureCount);
    }

    public void SetTotalTreasureCount(int value)
    {
        totalTreasureCount = value;
        inGameUI.SetTotalTreasureCount(totalTreasureCount);
    }

    public void SetPlayerPos()
    {
        player.transform.position = playerStartingPos;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ResetPlayerState()
    {
        playerStateController.ChangeState(playerStateController.IdleState);
    }

    public void UpdateLevelText(int level)
    {
        inGameUI.SetLevelText(level);
    }

    public void ResetPlayerSpeed()
    {
        playerController.IncreaseMovementSpeed(true, false);
    }

    public void ResetProgressBar()
    {
        inGameUI.ResetProgressbar();
    }

    public void ResetFinishCondition()
    {
        inGameState.isFinished = false;
    }

    public void ResetPlayerColor()
    {
        playerMatColorChanger.ResetPlayerColor();
    }

    public void SetupTutorialLevel(TutorialLevel level)
    {
        level.Initialize(player.GetComponent<PlayerSandAccumulator>(), player.GetComponent<Inventory>(),playerUpgradesUI);
    }

    public void ChangeCamFollowTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }
}