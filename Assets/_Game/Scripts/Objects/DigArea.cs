using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control.Items;
using _Game.Scripts.Enums;
using _Game.Scripts.MeshTools;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using _Game.Scripts.StatSystem;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    [SerializeField] private LootArea lootArea;
    [SerializeField] private DigZone digZone;

    private StateController stateController;
    private Coroutine diggingCoroutine;
    private WaitForSeconds diggingCoroutineWaitForSeconds;
    private StackManager playerStackManager;
    private Transform playerDigPos;
    public float LootingCooldown { get; set; }

    private void Start()
    {
        LootingCooldown = 1f;
        diggingCoroutineWaitForSeconds = new WaitForSeconds(LootingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (stateController == null || playerStackManager == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
            playerDigPos = other.gameObject.transform.Find("Digger").gameObject.transform;
            Stats stats = other.GetComponent<Stats>();
            stats.OnItemDropChanceChange += UpdateLootingCooldown;
            UpdateLootingCooldown(stats.GetStat(Stat.ItemDropChance));
        }

        diggingCoroutine = StartCoroutine(DiggingCoroutine());
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private void UpdateLootingCooldown(float value)
    {
        LootingCooldown = value;
        diggingCoroutineWaitForSeconds = new WaitForSeconds(LootingCooldown);
    }

    private IEnumerator DiggingCoroutine()
    {
        int lastPercentage = digZone.GetPercentOfDig();

        while (true)
        {
            if (stateController.CurrentState == stateController.DigState)
            {
                if (digZone.GetPercentOfDig() != lastPercentage)
                {
                    lastPercentage = digZone.GetPercentOfDig();

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
                        playerStackManager.Add(obj, 0.25f);
                    }
                }
            }

            yield return diggingCoroutineWaitForSeconds;
        }
    }
}