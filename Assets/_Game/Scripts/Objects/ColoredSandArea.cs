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
            UpdatePlayerLevel(stats.GetStatLevel(Stat.Strength));
            bumpChecker = other.GetComponent<BumpChecker>();
            playerMatColorChanger = other.GetComponent<PlayerMatColorChanger>();
            notEnoughPower = other.GetComponent<NotEnoughPower>();
            print("entered to" + sandType);
            if (level > playerStrengthLevel)
            {
                print("if entered to" + sandType);
                bumpChecker.isPlayerOnHigherLevelArea = true;
                playerMatColorChanger.ChangeColor(true);
                notEnoughPower.InstantiateText();
            }
        }
    }

    private void UpdatePlayerLevel(float value)
    {
        playerStrengthLevel = stats.GetStatLevel(Stat.Strength);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        print("exited from" + sandType);
        if (level > playerStrengthLevel)
        {
            print("if exited from" + sandType);
            bumpChecker.isPlayerOnHigherLevelArea = false;
            playerMatColorChanger.ChangeColor(false);
        }
    }
}