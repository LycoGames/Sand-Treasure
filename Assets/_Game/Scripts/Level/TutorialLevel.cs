using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using _Game.Scripts.UI;
using _Game.Scripts.Utils.LineRendererGPS;
using UnityEngine;

public class TutorialLevel : Level
{
    [SerializeField] private Line lineRenderer;
    [SerializeField] private DigZoneTutorialElement digZoneTutorialElement;
    [SerializeField] private UpgradeZoneTutorialElement upgradeZoneTutorialElement;
    [SerializeField] private CollectTutorialElement collectTutorialElement;

    public void Initialize(PlayerSandAccumulator playerSandAccumulator, Inventory playerInventory,PlayerUpgradesUI playerUpgradesUI)
    {
        if (lineRenderer.IsTutorialComplete)
        {
            return;
        }

        lineRenderer.SetPlayer(playerSandAccumulator.transform);
        digZoneTutorialElement.Initialize(playerSandAccumulator);
        collectTutorialElement.Initialize(playerInventory);
        upgradeZoneTutorialElement.Initialize(playerUpgradesUI);
    }
}