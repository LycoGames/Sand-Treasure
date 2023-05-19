using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.SequenceManager;
using _Game.Scripts.States;
using _Game.Scripts.UI;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    private int totalTreasureCount;
    private int foundedTreasureCount;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerStartingPos;
    [SerializeField] private StateController playerStateController;
    
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
}