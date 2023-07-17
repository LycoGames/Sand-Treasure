using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using LiquidVolumeFX;
using Random = UnityEngine.Random;

public class SandSellArea : MonoBehaviour
{
    [SerializeField] private List<SandCubes> cubesToThrow = new List<SandCubes>();
    [SerializeField] private AudioClip sellSFX;

    [Tooltip("Decrease for more cubes")] [SerializeField]
    private int amountDivider;

    [SerializeField] private ParticleSystem sandVFX;
    [SerializeField] private UIRewardVisualizer rewardVisualizer;
    [SerializeField] private MoneyStackController moneyStackController;

    private PlayerSandAccumulator playerSandAccumulator;
    private LiquidVolume liquidVolume;
    private Stats stats;
    private Transform playerBowlTransform;
    private Stack<ParticleSystem> vfxStack = new Stack<ParticleSystem>();
    private const int Blue = 0;
    private const int Pink = 1;
    private const int Yellow = 2;
    private const int Green = 3;
    private const int Purple = 4;
    private const int Red = 5;
    private const int Snow = 6;
    private const int Lava = 7;
    private float capacity;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
    private WaitForSeconds waitForSecondsVFX = new WaitForSeconds(0.25f);
    public Action OnSell;
    private const int BlueValue = 5;
    private const int PinkValue = 3;
    private const int YellowValue = 2;
    private const int GreenValue = 7;
    private const int PurpleValue = 8;
    private const int RedValue = 10;
    private const int SnowValue = 2;
    private const int LavaValue = 2;

    private List<int> money = new();
    private Coroutine coroutine;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (playerBowlTransform == null)
        {
            playerSandAccumulator = other.GetComponent<PlayerSandAccumulator>();
            playerBowlTransform = other.gameObject.transform.Find("Bowl").gameObject.transform;
            liquidVolume = playerSandAccumulator.LiquidVolume;
            stats = other.GetComponent<Stats>();
            capacity = stats.GetStat(Stat.StackCapacity);
            stats.OnStackCapacityChange += UpdatePlayerCapacity;
        }

        SellSand();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopAllCoroutines();
        for (int i = 0; i < liquidVolume.liquidLayers.Length; i++)
        {
            if (liquidVolume.liquidLayers[i].amount > 0)
            {
                liquidVolume.liquidLayers[i].amount = 0;
            }

            liquidVolume.UpdateLayers();
            playerSandAccumulator.ResetCapacity();
        }
    }

    private void UpdatePlayerCapacity(float value)
    {
        capacity = value;
    }

    private void SellSand()
    {
        // if (playerSandAccumulator.IsCapacityEmpty())
        // {
        //     return;
        // }
        float sum = liquidVolume.liquidLayers.Sum(liquidLayerValue => liquidLayerValue.amount);
        if (sum <= 0)
        {
            return;
        }

        OnSell?.Invoke();
        SoundManager.Instance.Play(sellSFX);
        GameManager.Instance.Vibrate(100, 200, true);
        for (int i = 0; i < liquidVolume.liquidLayers.Length; i++)
        {
            if (liquidVolume.liquidLayers[i].amount > 0)
            {
                StartCoroutine(EmptyPlayerBowlCoroutine(liquidVolume.liquidLayers[i].amount, 0, 0.25f, i));
                InstantiateEffect(i);
                CalculateMoneyCount(i);
                //InstantiateSandCubes(i);
            }
        }

        StartCoroutine(PlayParticles());
        //StartCoroutine(ThrowSandCubes());
        moneyStackController.StartSpawningMoney(money);
        money.Clear();
    }


    private void CalculateMoneyCount(int i)
    {
        switch (i)
        {
            case Blue:
            {
                InstantiateMoney(CalculateCubeCount(Blue), BlueValue);
                break;
            }
            case Pink:
            {
                InstantiateMoney(CalculateCubeCount(Pink), PinkValue);
                break;
            }
            case Yellow:
            {
                InstantiateMoney(CalculateCubeCount(Yellow), YellowValue);
                break;
            }
            case Green:
            {
                InstantiateMoney(CalculateCubeCount(Green), GreenValue);
                break;
            }
            case Purple:
            {
                InstantiateMoney(CalculateCubeCount(Purple), PurpleValue);
                break;
            }
            case Red:
            {
                InstantiateMoney(CalculateCubeCount(Red), RedValue);
                break;
            }
            case Snow:
            {
                InstantiateMoney(CalculateCubeCount(Snow), SnowValue);
                break;
            }
            case Lava:
            {
                InstantiateMoney(CalculateCubeCount(Lava), LavaValue);
                break;
            }
        }
    }

    private void InstantiateMoney(int i, int value)
    {
        for (int j = 0; j < i; j++)
        {
            money.Add(value);
        }
    }


    private void InstantiateSandCubes(int i)
    {
        switch (i)
        {
            case Blue:
            {
                InstantiateCubes(CalculateCubeCount(Blue), SandType.Blue);
                //cubesToThrow.Add(instance);
                break;
            }
            case Pink:
            {
                InstantiateCubes(CalculateCubeCount(Pink), SandType.Pink);
                //cubesToThrow.Add(instance);
                break;
            }
            case Yellow:
            {
                InstantiateCubes(CalculateCubeCount(Yellow), SandType.Yellow);
                //cubesToThrow.Add(instance);
                break;
            }
            case Green:
            {
                InstantiateCubes(CalculateCubeCount(Green), SandType.Green);
                //cubesToThrow.Add(instance);
                break;
            }
            case Purple:
            {
                InstantiateCubes(CalculateCubeCount(Purple), SandType.Purple);
                //cubesToThrow.Add(instance);
                break;
            }
            case Red:
            {
                InstantiateCubes(CalculateCubeCount(Red), SandType.Red);
                //cubesToThrow.Add(instance);
                break;
            }
            case Snow:
            {
                InstantiateCubes(CalculateCubeCount(Snow), SandType.Snow);
                //cubesToThrow.Add(instance);
                break;
            }
            case Lava:
            {
                InstantiateCubes(CalculateCubeCount(Lava), SandType.Lava);
                //cubesToThrow.Add(instance);
                break;
            }
        }
    }

    private void InstantiateCubes(int count, SandType sandType)
    {
        for (int j = 0; j < count; j++)
        {
            var instance = PoolManager.Instance.GetFromPool(sandType);
            cubesToThrow.Add(instance);
        }
    }

    private IEnumerator ThrowSandCubes()
    {
        if (cubesToThrow.Count <= 0) yield break;

        cubesToThrow.Reverse();
        foreach (var cube in cubesToThrow)
        {
            cube.transform.position = transform.position;
            cube.gameObject.SetActive(true);
            cube.transform.DOJump(GetRandomPos(), 5f, 1, 0.5f).OnComplete((() =>
                    {
                        cube.Effect.effect.Play();
                        cube.Rigidbody.isKinematic = false;
                        cube.SphereCollider.isTrigger = true;
                    }
                ));
            ;
            // cube.Rigidbody.DOJump(GetRandomPos(), 5f, 1, 0.5f).OnComplete((() =>
            //         {
            //            // cube.Rigidbody.isKinematic = false;
            //             cube.SphereCollider.isTrigger = true;
            //         }
            //     ));
            // ;
            Vector3 scale = new Vector3(0.25f, 0.25f, 0.25f);
            cube.transform.DOShakeScale(0.25f, 1f, 1, 45f);
            cube.transform.DOPunchScale(scale, 1f, 1).SetLoops(3);
            yield return waitForSeconds;
        }

        cubesToThrow.Clear();
    }

    private int CalculateCubeCount(int i)
    {
        float sandAmount = liquidVolume.liquidLayers[i].amount;
        float count = sandAmount / (1 / (capacity / amountDivider));
        return (int)count;
    }


    private Vector3 GetRandomPos()
    {
        int randomX = Random.Range(-12, 0);
        int randomZ = Random.Range(-58, -50);
        Vector3 pos = new Vector3(randomX, 1, randomZ);
        return pos;
    }

    private void InstantiateEffect(int i)
    {
        var color = liquidVolume.liquidLayers[i].color;
        var instance = Instantiate(sandVFX, playerBowlTransform.position, Quaternion.identity);
        var instanceMain = instance.main;
        instanceMain.startColor = color;
        vfxStack.Push(instance);
    }

    private IEnumerator EmptyPlayerBowlCoroutine(float currentValue, float newValue, float duration, int layerIndex)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            liquidVolume.liquidLayers[layerIndex].amount = Mathf.Lerp(currentValue, newValue, t / duration);
            liquidVolume.UpdateLayers();
            yield return null;
        }

        liquidVolume.liquidLayers[layerIndex].amount = newValue;
        liquidVolume.UpdateLayers();
        playerSandAccumulator.ResetCapacity();
    }

    private IEnumerator PlayParticles()
    {
        while (vfxStack.Count > 0)
        {
            var vfx = vfxStack.Pop();
            vfx.Play();
            vfx.transform.DOJump(transform.position, 5f, 1, 1f).OnComplete((() => Destroy(vfx.gameObject)));
            yield return waitForSecondsVFX;
        }
        // (var vfx in vfxQueue)
        // {
        //     vfx.Play();
        //     vfxQueue.Dequeue();
        //     vfx.transform.DOJump(transform.position, 5f, 1, 1f);
        //     yield return new WaitForSeconds(1f);
        // }
    }
}