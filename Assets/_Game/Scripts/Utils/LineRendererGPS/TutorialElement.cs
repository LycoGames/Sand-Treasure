using System;
using UnityEngine;

public class TutorialElement : MonoBehaviour
{
    public Action OnConditionComplete;
    [SerializeField] private Transform destinationTransform;

    public void ConditionComplete()
    {
        OnConditionComplete?.Invoke();
    }

    public Transform GetDestinationTransform()
    {
        return destinationTransform != null ? destinationTransform : transform;
    }
}