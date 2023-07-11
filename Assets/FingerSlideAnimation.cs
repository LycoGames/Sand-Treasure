using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.UI;

public class FingerSlideAnimation : MonoBehaviour
{
    [SerializeField] private Transform fingerTransform;
    [SerializeField] private PathType pathType;
    [SerializeField] private List<Transform> pointTransforms;
    private readonly Vector3[] points=new Vector3[13];

    private void Start()
    {
        for (int i = 0; i < pointTransforms.Count; i++)
        {
            points[i] = pointTransforms[i].position;
        }

        StartFingerAnim();
    }

    private void StartFingerAnim()
    {
        fingerTransform.DOPath(points, 2f, pathType).SetEase(Ease.Linear).SetLoops(-1);
    }
}