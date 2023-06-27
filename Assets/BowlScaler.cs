using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Saving;
using _Game.Scripts.StatSystem;
using UnityEngine;

public class BowlScaler : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform bowl;
    [SerializeField] private float scaleX;
    [SerializeField] private Stats stats;
    private float scaleSizeX = 0.8f;

    private void Start()
    {
        stats.OnStackCapacityChange += ExpandBowl;
    }

    private void ExpandBowl(float obj)
    {
        bowl.localScale += new Vector3(scaleX, 0, 0);
        scaleSizeX = bowl.localScale.x;
    }

    public object CaptureState()
    {
        return scaleSizeX;
    }

    public void RestoreState(object state)
    {
        float value = (float)state;
        if (value < 0.8f) return;
        scaleSizeX = value;
        SetScaleSizeOnBowl(scaleSizeX);
    }

    private void SetScaleSizeOnBowl(float value)
    {
        bowl.localScale = new Vector3(value, 1, 1);
    }
}