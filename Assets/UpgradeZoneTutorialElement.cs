using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control;
using UnityEngine;

public class UpgradeZoneTutorialElement : TutorialElement
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ConditionComplete();
    }
}