using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellZoneTutorialElement : TutorialElement
{
    [SerializeField] private SandSellArea sandSellArea;
    
    private void OnEnable()
    {
        sandSellArea.OnSell += TutorialCompleted;
    }

    private void TutorialCompleted()
    {
        ConditionComplete();
        sandSellArea.OnSell -= TutorialCompleted;
    }
}