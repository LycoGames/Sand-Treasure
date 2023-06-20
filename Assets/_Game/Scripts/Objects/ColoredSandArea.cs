using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using UnityEngine;

public class ColoredSandArea : MonoBehaviour
{
    [SerializeField] private SandType sandType;
    [SerializeField] private int level;
    
    private PlayerSandAccumulator playerSandAccumulator;
    private Stats stats;
    private BumpChecker bumpChecker;
    private PlayerMatColorChanger playerMatColorChanger;
    private int playerStrengthLevel;
    private NotEnoughPower notEnoughPower;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerSandAccumulator = other.GetComponent<PlayerSandAccumulator>();
            playerSandAccumulator.ChangeDustColor(sandType);
            stats = other.GetComponent<Stats>();
            stats.OnStrengthChange += UpdatePlayerLevel;
            bumpChecker = other.GetComponent<BumpChecker>();
            playerMatColorChanger = other.GetComponent<PlayerMatColorChanger>();
            UpdatePlayerLevel(stats.GetStatLevel(Stat.Strength));
            notEnoughPower = other.GetComponent<NotEnoughPower>();
            if (level>playerStrengthLevel)
            {
                bumpChecker.higherLevelArea = true;
                playerMatColorChanger.ChangeColor(true);
                notEnoughPower.InstantiateText();
                print(playerStrengthLevel);
                print("not enough power");
            }
        }
    }

    private void UpdatePlayerLevel(float value)
    {
        playerStrengthLevel = stats.GetStatLevel(Stat.Strength);
    }

    private void OnTriggerExit(Collider other)
    {
        if (level>playerStrengthLevel)
        {
            bumpChecker.higherLevelArea = false;
            playerMatColorChanger.ChangeColor(false);
        }
    }
    
}
