using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct BodyPart
{
    public BoneType type;
    public GameObject part;
}

public class FossilBodyController : MonoBehaviour
{
    [SerializeField] private List<FossilPart> fossilPartPrefabsToInstantiate;
    [SerializeField] private List<BodyPart> bodyParts;
    private FossilManager fossilManager;
    private Action<BoneType> OnCollect;
    private int collectCount;
    public Action OnSkeletonComplete;

    public void SetupBody(List<BoneType> collectedBones, bool hasCollected, Action<BoneType> OnCollect)
    {
        this.OnCollect = OnCollect;
        collectCount = collectedBones.Count;
        foreach (var bodyPart in bodyParts)
        {
            if (!collectedBones.Contains(bodyPart.type))
            {
                bodyPart.part.SetActive(false);
            }
        }

        if (!hasCollected)
            InstantiateFossilPart(collectedBones);
    }

    private void PartCollect(FossilPart part)
    {
        OnCollect?.Invoke(part.BoneType);
        EnableBodyPart(part.BoneType);
        collectCount++;
        if (collectCount >= bodyParts.Count)
        {
            OnSkeletonComplete?.Invoke();
        }
        else
        {
            GameManager.Instance.ChangeCamFollowTargetToPlayer();
        }
    }

    private void EnableBodyPart(BoneType type)
    {
        foreach (var bodyPart in bodyParts)
        {
            if (bodyPart.type == type)
            {
                bodyPart.part.SetActive(true);
            }
        }
    }

    private void SwitchFollowTarget(FossilPart part)
    {
        GameManager.Instance.ChangeCamFollowTarget(part.transform);
    }

    private void InstantiateFossilPart(List<BoneType> collectedBones)
    {
        foreach (var prefab in fossilPartPrefabsToInstantiate)
        {
            if (!collectedBones.Contains(prefab.BoneType))
            {
                var instance = Instantiate(prefab, GetRandomPos(), Quaternion.identity,this.transform);
                foreach (var bodyPart in bodyParts)
                {
                    if (bodyPart.type == instance.BoneType)
                    {
                        instance.Setup(bodyPart.part.transform.position, SwitchFollowTarget);
                        instance.OnSequenceComplete += PartCollect;
                    }
                }

                break;
            }
        }
    }

    private Vector3 GetRandomPos()
    {
        float randomX;
        float randomZ;
        randomX = Random.Range(-25, 25);
        randomZ = Random.Range(-10, 15);
        return new Vector3(randomX, 1, randomZ);
    }
}