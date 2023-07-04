using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Saving;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct FossilManagerData
{
    public int currentBodyIndex;
    public List<BoneType> collectedBoneTypes;

    public FossilManagerData(int index, List<BoneType> boneTypes)
    {
        currentBodyIndex = index;
        collectedBoneTypes = boneTypes;
    }
}

public class FossilManager : MonoBehaviour, ISaveable
{
    [SerializeField] private List<FossilBodyController> fossilBodyPrefabList;
    [SerializeField] private UIRewardVisualizer rewardVisualizer;
    [SerializeField] private AudioClip clip;
    
    private int currentBodyIndex;
    private CinemachineVirtualCamera virtualCamera;
    private List<BoneType> collectedBoneTypes = new();
    private FossilBodyController fossilBodyController;
    
    public void Initialize()
    {
        InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
        rewardVisualizer.SetDestination(inGameUI.MoneyPanel);
    }

    public void InstantiateBody()
    {
        fossilBodyController = Instantiate(fossilBodyPrefabList[currentBodyIndex],
            this.transform.position + fossilBodyPrefabList[currentBodyIndex].transform.localPosition,
            Quaternion.identity * fossilBodyPrefabList[currentBodyIndex].transform.localRotation, this.transform);
        fossilBodyController.SetupBody(collectedBoneTypes, BodyPartCollected);
        fossilBodyController.OnSkeletonComplete += GoNextFossil;
    }

    private void GoNextFossil()
    {
        currentBodyIndex++;
        StartCoroutine(RewardCoroutine());
        //TODO instantiate effect,money add money to  player.
    }

    private IEnumerator RewardCoroutine()
    {
        GameManager.Instance.PlayConfetti();
        SoundManager.Instance.PlayOneShot(clip);
        for (int i = 0; i < 4; i++)
        {
            rewardVisualizer.VisualiseReward(this.transform.position,(() => GameManager.Instance.AddMoneyToPlayer(20)));
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.ChangeCamFollowTargetToPlayer();
    }


    private void BodyPartCollected(BoneType type)
    {
        collectedBoneTypes.Add(type);
    }

    public object CaptureState()
    {
        return new FossilManagerData(currentBodyIndex, collectedBoneTypes);
    }

    public void RestoreState(object state)
    {
        var data = (FossilManagerData)state;
        currentBodyIndex = data.currentBodyIndex;
        collectedBoneTypes = data.collectedBoneTypes;
    }
}