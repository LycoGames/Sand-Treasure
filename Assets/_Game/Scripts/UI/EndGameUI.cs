using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Base.UserInterface;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : AbstractBaseCanvas
{
    [SerializeField] private Button nextLevelButton;

    public OnUIButtonClickEvent NextLevel;
    
    public override void OnStart()
    {
        Debug.Log("EndGameUI Enter");
        nextLevelButton.onClick.AddListener(GoNextLevel);
    }

    public override void OnQuit()
    {
        Debug.Log("EndGameUI Exit");
    }

    private void GoNextLevel()
    {
        NextLevel?.Invoke();
    }
}
