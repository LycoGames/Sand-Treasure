using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.Singleton;
using _Game.Scripts.Enums;
using _Game.Scripts.SequenceManager;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    private int totalTreasureCount;
    private int foundedTreasureCount;

    public void IncreaseFoundedTreasureCount()
    {
        foundedTreasureCount++;
        if (foundedTreasureCount>=totalTreasureCount)
        {
            SequenceManager.Instance.ChangeState(AppStateTypes.EndGame);
        }
    }

    public void ResetTreasureCount()
    {
        foundedTreasureCount = 0;
    }

    public void SetTotalTreasureCount(int value)
    {
        totalTreasureCount = value;
    }
}
