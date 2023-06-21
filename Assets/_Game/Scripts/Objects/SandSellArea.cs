using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.StatSystem;
using DG.Tweening;
using UnityEngine;
using LiquidVolumeFX;
using Random = UnityEngine.Random;

public class SandSellArea : MonoBehaviour
{
    private PlayerSandAccumulator playerSandAccumulator;
    private LiquidVolume liquidVolume;
    private Stats stats;
    private Transform playerBowlTransform;
    private Stack<ParticleSystem> vfxStack = new Stack<ParticleSystem>();
    [SerializeField] private ParticleSystem sandVFX;
    private const int Blue = 0;
    private const int Pink = 1;
    private const int Yellow = 2;
    private const int Green = 3;
    private const int Purple = 4;
    [SerializeField] private List<SandCubes> cubesToThrow = new List<SandCubes>();

    [Tooltip("Decrease for more cubes")] [SerializeField]
    private int amountDivider;

    private float capacity;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
    private WaitForSeconds waitForSecondsVFX = new WaitForSeconds(0.25f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

        for (int i = 0; i < liquidVolume.liquidLayers.Length; i++)
        {
            if (liquidVolume.liquidLayers[i].amount > 0)
            {
                StartCoroutine(EmptyPlayerBowlCoroutine(liquidVolume.liquidLayers[i].amount, 0, 0.25f, i));
                InstantiateEffect(i);
                InstantiateSandCubes(i);
            }
        }

        StartCoroutine(PlayParticles());
        StartCoroutine(ThrowSandCubes());
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

    private void InstantiateCubes(int count, SandType sandType)
    {
        for (int j = 0; j < count; j++)
        {
            var instance = PoolManager.Instance.GetFromPool(sandType);
            cubesToThrow.Add(instance);
        }
    }

    private Vector3 GetRandomPos()
    {
        int randomX = Random.Range(-16, 0);
        int randomZ = Random.Range(-40, -26);
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

    private void OnTriggerExit(Collider other)
    {
        //StopAllCoroutines();
    }
}