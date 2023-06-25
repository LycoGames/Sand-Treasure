using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Saving;
using _Game.Scripts.UI;
using UnityEngine;

public class LevelLoader : MonoBehaviour, ISaveable
{
    [SerializeField] private List<Level> levels = new List<Level>();
    [SerializeField] private PlayerUpgradesUI playerUpgradesMenuUI;

    public int currentLevel;
    private Level loadedLevel;

    public void LoadLevel()
    {
        if (loadedLevel) DestroyLoadedLevel();
        if (currentLevel > levels.Count - 1)
        {
            loadedLevel = LoadRandomLevel();
        }

        else
        {
            loadedLevel = Instantiate(levels[currentLevel]);
        }

        loadedLevel.Initialize(playerUpgradesMenuUI);
        if (currentLevel==0)
        {
            TutorialLevel tutorialLevel = (TutorialLevel)loadedLevel;
            GameManager.Instance.SetupTutorialLevel(tutorialLevel);
        }
        GameManager.Instance.ResetTreasureCount();
        GameManager.Instance.SetTotalTreasureCount(levels[currentLevel].TotalTreasureCount);
        GameManager.Instance.SetPlayerPos();
        GameManager.Instance.ResetPlayerState();
        GameManager.Instance.UpdateLevelText(currentLevel + 1);
        GameManager.Instance.ResetPlayerSpeed();
        GameManager.Instance.ResetProgressBar();
        GameManager.Instance.ResetFinishCondition();
        GameManager.Instance.ResetPlayerColor();
    }

    public void OnLevelComplete()
    {
        currentLevel++;
        MoonSDK.TrackLevelEvents(MoonSDK.LevelEvents.Complete, currentLevel);
    }

    private void DestroyLoadedLevel()
    {
        loadedLevel.DestroyLevel();
        loadedLevel = null;
    }

    private Level LoadRandomLevel()
    {
        currentLevel = GetRandomLevelIndex();
        return Instantiate(levels[currentLevel]);
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