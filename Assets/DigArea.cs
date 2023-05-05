using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    [SerializeField] private float diggingCooldown = .25f;
    private StateController stateController;
    private Coroutine diggingCoroutine;
    private WaitForSeconds diggingCoroutineWaitForSeconds;
    private StackManager playerStackManager;
    private ItemsObjectPool pool;

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(diggingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateController == null || playerStackManager == null || pool == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
            pool = other.GetComponent<ItemsObjectPool>();
        }

        stateController.ChangeState(stateController.DigState);
        diggingCoroutine = StartCoroutine(DiggingCoroutine());
    }

    private void OnTriggerExit(Collider other)
    {
        stateController.ChangeState(stateController.IdleState);
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private IEnumerator DiggingCoroutine()
    {
        while (playerStackManager.CanAddToStack(pool.GetPrefab().Type))
        {
            var obj = pool.GetFromPool();
            obj.gameObject.SetActive(true);
            playerStackManager.Add(obj, diggingCooldown);
            yield return diggingCoroutineWaitForSeconds;
        }
    }
}