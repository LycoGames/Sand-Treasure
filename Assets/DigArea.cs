using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control.Items;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    [SerializeField] private float lootingCooldown = .25f;
    [SerializeField] private LootArea lootArea;

    private StateController stateController;
    private Coroutine diggingCoroutine;
    private WaitForSeconds diggingCoroutineWaitForSeconds;
    private StackManager playerStackManager;
    private Transform playerDigPos;

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(lootingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateController == null || playerStackManager == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
            playerDigPos = other.gameObject.transform.Find("Digger").gameObject.transform;
        }

        diggingCoroutine = StartCoroutine(DiggingCoroutine());
    }


    private void OnTriggerExit(Collider other)
    {
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private IEnumerator DiggingCoroutine()
    {
        while (true)
        {
            if (stateController.CurrentState == stateController.DigState)
            {
                var item = lootArea.GetDroppedItem();
                if (item == null)
                {
                    continue;
                }

                if (playerStackManager.CanAddToStack(item.Type))
                {
                    var obj = PoolManager.Instance.GetFromPool(item.Type);
                    obj.transform.position = playerDigPos.position;
                    obj.gameObject.SetActive(true);
                    playerStackManager.Add(obj, lootingCooldown);
                }
            }

            yield return diggingCoroutineWaitForSeconds;
        }
    }
}