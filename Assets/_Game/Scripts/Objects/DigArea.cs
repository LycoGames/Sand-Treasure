using System.Collections;
using _Game.Scripts.Enums;
using _Game.Scripts.MeshTools;
using _Game.Scripts.Player;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using _Game.Scripts.States;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Objects
{
    public class DigArea : MonoBehaviour
    {
        [SerializeField] private LootArea lootArea;
        [SerializeField] private DigZone digZone;
        private InGameUI inGameUI;
        private StateController stateController;
        private Coroutine diggingCoroutine;
        private WaitForSeconds diggingCoroutineWaitForSeconds;
        private StackManager playerStackManager;
        private Transform playerDigPos;
        private PlayerSandAccumulator playerSandAccumulator;
        public float LootingCooldown { get; set; }

        private void Start()
        {
            inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
            LootingCooldown = 1f;
            diggingCoroutineWaitForSeconds = new WaitForSeconds(LootingCooldown);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (stateController == null || playerStackManager == null)
            {
                stateController = other.GetComponent<StateController>();
                playerSandAccumulator = other.GetComponent<PlayerSandAccumulator>();
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
            float lastPercentage = digZone.GetPercentOfDig();

            while (true)
            {
                if (stateController.CurrentState == stateController.DigState)
                {
                    if (digZone.GetPercentOfDig() != lastPercentage)
                    {
                        inGameUI.UpdateProgressBar(digZone.GetPercentOfDig());
                        lastPercentage = digZone.GetPercentOfDig();

                        if (playerSandAccumulator.CanAccumulateSand())
                        {
                            playerSandAccumulator.AccumulateSand();
                        }
                    }
                }

                yield return diggingCoroutineWaitForSeconds;
            }
        }
        // FOR LOOTING ITEMS, ITS CANCELLED ON ITERATION
        // private IEnumerator DiggingCoroutine()
        // {
        //     int lastPercentage = digZone.GetPercentOfDig();
        //
        //     while (true)
        //     {
        //         if (stateController.CurrentState == stateController.DigState)
        //         {
        //             if (digZone.GetPercentOfDig() != lastPercentage)
        //             {
        //                 lastPercentage = digZone.GetPercentOfDig();
        //                 
        //                 var item = lootArea.GetDroppedItem();
        //                 if (item == null)
        //                 {
        //                     continue;
        //                 }
        //
        //                 if (playerStackManager.CanAddToStack(item.Type))
        //                 {
        //                     var obj = PoolManager.Instance.GetFromPool(item.Type);
        //                     obj.transform.position = playerDigPos.position;
        //                     obj.gameObject.SetActive(true);
        //                     playerStackManager.Add(obj, 0.25f);
        //                 }
        //             }
        //         }
        //
        //         yield return diggingCoroutineWaitForSeconds;
        //     }
        // }
    }
}