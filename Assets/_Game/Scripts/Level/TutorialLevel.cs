using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Utils.LineRendererGPS;
using UnityEngine;

public class TutorialLevel : Level
{
    [SerializeField] private Line lineRenderer;
    [SerializeField] private DigZoneTutorialElement digZoneTutorialElement;
    [SerializeField] private UpgradeZoneTutorialElement upgradeZoneTutorialElement;

    public void Initialize(PlayerSandAccumulator playerSandAccumulator)
    {
        if (lineRenderer.IsTutorialComplete)
        {
            return;
        }

        lineRenderer.SetPlayer(playerSandAccumulator.transform);
        digZoneTutorialElement.Initialize(playerSandAccumulator);
    }
}