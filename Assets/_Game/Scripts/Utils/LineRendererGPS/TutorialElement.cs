using System;
using UnityEngine;

public class TutorialElement : MonoBehaviour
{
    public Action OnConditionComplete;
    public Action OnDestinationReached;

    [SerializeField] private Transform destinationTransform;

    protected void ConditionComplete()
    {
        OnConditionComplete?.Invoke();
    }

    protected void ReachedToDestination()
    {
        OnDestinationReached?.Invoke();
    }

    public Transform GetDestinationTransform()
    {
        return destinationTransform != null ? destinationTransform : transform;
    }
}