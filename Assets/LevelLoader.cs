using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    //TODO load new level, reset player's stack, set player's pos, Reset TreasureCount, Update TotalTreasureCount
    [SerializeField] private List<Level> levels = new List<Level>();

    private int index;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadLevel()
    {
        if (index > levels.Count) LoadRandomLevel();
        Instantiate(levels[index]);
        if (levels[index-1])
        {
            Destroy(levels[index-1]);
        }
        GameManager.Instance.ResetTreasureCount();
        GameManager.Instance.SetTotalTreasureCount(levels[index].TotalTreasureCount);
    }

    private void LoadRandomLevel()
    {
        Instantiate(levels[GetRandomLevelIndex()]);
    }

    private int GetRandomLevelIndex()
    {
        return Random.Range(0, levels.Count);
    }
    
}