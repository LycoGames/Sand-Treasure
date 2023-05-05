using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    [SerializeField] private ObjectPool pool;
    [SerializeField] private float diggingCooldown = .25f;
    private StateController stateController;
    private Coroutine diggingCoroutine;
    private WaitForSeconds diggingCoroutineWaitForSeconds;

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(diggingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        stateController = other.GetComponent<StateController>();
        stateController.ChangeState(stateController.DigState);
        if (TryGetComponent<StackManager>(out var stackManager))
            diggingCoroutine = StartCoroutine(DiggingCoroutine(stackManager));
    }

    private void OnTriggerExit(Collider other)
    {
        stateController.ChangeState(stateController.IdleState);
        StopCoroutine(diggingCoroutine);
    }

    private IEnumerator DiggingCoroutine(StackManager sourceStack)
    {
        while (sourceStack.CanAddToStack(pool.GetPrefab().Type))
        {
            sourceStack.Add(pool.Get(), diggingCooldown);
            yield return diggingCoroutineWaitForSeconds;
        }
    }
}