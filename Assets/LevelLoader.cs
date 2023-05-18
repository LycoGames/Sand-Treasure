using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Saving;
using _Game.Scripts.UI;
using UnityEngine;

public class LevelLoader : MonoBehaviour, ISaveable
{
    //TODO load new level, reset player's stack, set player's pos, Reset TreasureCount, Update TotalTreasureCount
    [SerializeField] private List<Level> levels = new List<Level>();
    [SerializeField] private PlayerUpgradesUI playerUpgradesMenuUI;
    
    private int currentLevel;
    private Level loadedLevel;

    public void LoadLevel()
    {
        if (loadedLevel) DestroyLoadedLevel();
        if (currentLevel > levels.Count) LoadRandomLevel();
        loadedLevel = Instantiate(levels[currentLevel]);
        loadedLevel.Initialize(playerUpgradesMenuUI);
        GameManager.Instance.ResetTreasureCount();
        GameManager.Instance.SetTotalTreasureCount(levels[currentLevel].TotalTreasureCount);
    }

    private void DestroyLoadedLevel()
    {
        loadedLevel.DestroyLevel();
        loadedLevel = null;
    }

    public void OnLevelComplete()
    {
        currentLevel++;
    }

    private void LoadRandomLevel()
    {
        Instantiate(levels[GetRandomLevelIndex()]);
    }

    private int GetRandomLevelIndex()
    {
        return Random.Range(0, levels.Count);
    }

    public object CaptureState()
    {
        return currentLevel;
    }

    public void RestoreState(object state)
    {
        currentLevel = (int)state;
    }
}