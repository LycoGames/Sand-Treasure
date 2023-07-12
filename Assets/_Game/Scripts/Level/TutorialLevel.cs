using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control;
using _Game.Scripts.Player;
using _Game.Scripts.UI;
using _Game.Scripts.Utils.LineRendererGPS;
using UnityEngine;

public class TutorialLevel : Level
{
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private DigZoneTutorialElement digZoneTutorialElement;
    [SerializeField] private UpgradeZoneTutorialElement upgradeZoneTutorialElement;
    [SerializeField] private CollectTutorialElement collectTutorialElement;

    public void Initialize(PlayerSandAccumulator playerSandAccumulator, Inventory playerInventory,
        PlayerController playerController,
        PlayerUpgradesUI playerUpgradesUI)
    {
        if (tutorialManager.IsTutorialComplete)
            return;

        digZoneTutorialElement.Initialize(playerSandAccumulator);
        collectTutorialElement.Initialize(playerInventory);
        upgradeZoneTutorialElement.Initialize(playerUpgradesUI);
        cameraSwitcher.Initialize(playerController);
    }
}