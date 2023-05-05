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
    private StackManager playerStackManager;

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(diggingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateController == null || playerStackManager == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
        }

        stateController.ChangeState(stateController.DigState);
        diggingCoroutine = StartCoroutine(DiggingCoroutine(playerStackManager));
    }

    private void OnTriggerExit(Collider other)
    {
        stateController.ChangeState(stateController.IdleState);
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private IEnumerator DiggingCoroutine(StackManager playerStack)
    {
        while (playerStack.CanAddToStack(pool.GetPrefab().Type))
        {
            var obj = pool.Get();
            obj.gameObject.SetActive(true);
            playerStack.Add(obj, diggingCooldown);
            yield return diggingCoroutineWaitForSeconds;
        }
    }
}