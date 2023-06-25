using System.Collections;
using System.Collections.Generic;
using _Game.Utils.LineRendererGPS;
using UnityEngine;

public class TutorialLevel : Level
{
    [SerializeField] private Line lineRenderer;
    [SerializeField] private DigZoneTutorialElement digZoneTutorialElement;
    [SerializeField] private UpgradeZoneTutorialElement upgradeZoneTutorialElement;

    public void Initialize(PlayerSandAccumulator playerSandAccumulator)
    {
        lineRenderer.SetPlayer(playerSandAccumulator.transform);
        digZoneTutorialElement.Initialize(playerSandAccumulator);
    }
}