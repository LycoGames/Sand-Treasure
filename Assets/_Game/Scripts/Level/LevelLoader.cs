using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Saving;
using _Game.Scripts.UI;
using Cinemachine;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private List<Level> levels = new List<Level>();
    [SerializeField] private PlayerUpgradesUI playerUpgradesMenuUI;
    [SerializeField] private SavingSystem savingSystem;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public int currentLevel;
    private Level loadedLevel;
    private int levelCounter;
    public bool IsReset { get; set; }

    public void LoadLevel()
    {
        if (loadedLevel) DestroyLoadedLevel();
        currentLevel = PlayerPrefs.HasKey("LevelIndex") ? PlayerPrefs.GetInt("LevelIndex") : 0;
        levelCounter = PlayerPrefs.HasKey("LevelCounter") ? PlayerPrefs.GetInt("LevelCounter") : 0;
        if (currentLevel > levels.Count - 1)
        {
            loadedLevel = LoadFromFirstLevel();
        }

        else
        {
            loadedLevel = Instantiate(levels[currentLevel]);
        }

        loadedLevel.Initialize(playerUpgradesMenuUI);
        savingSystem.Load();
        loadedLevel.FossilManager.InstantiateBody();
        if (currentLevel == 0)
        {
            TutorialLevel tutorialLevel = (TutorialLevel)loadedLevel;
            GameManager.Instance.SetupTutorialLevel(tutorialLevel);
        }

        GameManager.Instance.ResetTreasureCount();
        GameManager.Instance.SetTotalTreasureCount(levels[currentLevel].TotalTreasureCount);
        GameManager.Instance.SetPlayerPos();
        GameManager.Instance.ResetPlayerState();
        GameManager.Instance.UpdateLevelText(levelCounter + 1);
        GameManager.Instance.ResetPlayerSpeed();
        GameManager.Instance.ResetPlayerColor();
    }

    public void OnLevelComplete()
    {
        currentLevel++;
        levelCounter++;
        PlayerPrefs.SetInt("LevelIndex", currentLevel);
        PlayerPrefs.SetInt("LevelCounter", levelCounter);
        PlayerPrefs.Save();
        savingSystem.Save();
        MoonSDK.TrackLevelEvents(MoonSDK.LevelEvents.Complete, levelCounter);
        GameManager.Instance.ResetProgressBar();
        GameManager.Instance.ResetFinishCondition();
    }

    private void DestroyLoadedLevel()
    {
        loadedLevel.DestroyLevel();
        loadedLevel = null;
    }

    private Level LoadFromFirstLevel()
    {
        currentLevel = 0;
        return Instantiate(levels[currentLevel]);
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

    public float GetCompletionPercentage()
    {
        return loadedLevel.MyDigZone.GetPercentOfDig();
    }
}