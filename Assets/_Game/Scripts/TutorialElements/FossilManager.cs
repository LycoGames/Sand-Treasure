using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.BaseSequence;
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
    public bool hasCollectedPart;

    public FossilManagerData(int index, bool hasCollected, List<BoneType> boneTypes)
    {
        currentBodyIndex = index;
        collectedBoneTypes = boneTypes;
        hasCollectedPart = hasCollected;
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

    private WaitForSeconds moneyWaitForSeconds = new WaitForSeconds(0.3f);
    private WaitForSeconds camWaitForSeconds = new WaitForSeconds(1.5f);
    private EndGameState endGameState;
    public bool HasCollectedPart { get; set; }

    public void Initialize()
    {
        InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
        rewardVisualizer.SetDestination(inGameUI.MoneyPanel);
        endGameState = SequenceManager.Instance.GetAppState(AppStateTypes.EndGame) as EndGameState;
        endGameState.OnClickNextLevelButton += NextLevel;
    }


    public void InstantiateBody()
    {
        fossilBodyController = Instantiate(fossilBodyPrefabList[currentBodyIndex],
            this.transform.position + fossilBodyPrefabList[currentBodyIndex].transform.localPosition,
            Quaternion.identity * fossilBodyPrefabList[currentBodyIndex].transform.localRotation, this.transform);
        fossilBodyController.SetupBody(collectedBoneTypes, HasCollectedPart, BodyPartCollected);
        fossilBodyController.OnSkeletonComplete += OnFossilCompleted;
    }

    private void OnFossilCompleted()
    {
        StartCoroutine(RewardCoroutine());
    }

    private void NextLevel()
    {
        if (collectedBoneTypes.Count >= 4)
        {
            currentBodyIndex++;
            if (currentBodyIndex >= fossilBodyPrefabList.Count)
            {
                currentBodyIndex = 0;
            }

            collectedBoneTypes.Clear();
        }

        HasCollectedPart = false;
    }

    private IEnumerator RewardCoroutine()
    {
        GameManager.Instance.PlayConfetti();
        SoundManager.Instance.PlayOneShot(clip);
        for (int i = 0; i < 4; i++)
        {
            rewardVisualizer.VisualiseReward(this.transform.position,
                (() => GameManager.Instance.AddMoneyToPlayer(20)));
            yield return moneyWaitForSeconds;
        }

        yield return camWaitForSeconds;
        GameManager.Instance.ChangeCamFollowTargetToPlayer();
    }


    private void BodyPartCollected(BoneType type)
    {
        collectedBoneTypes.Add(type);
        HasCollectedPart = true;
    }

    public object CaptureState()
    {
        return new FossilManagerData(currentBodyIndex, HasCollectedPart, collectedBoneTypes);
    }

    public void RestoreState(object state)
    {
        var data = (FossilManagerData)state;
        currentBodyIndex = data.currentBodyIndex;
        collectedBoneTypes = data.collectedBoneTypes;
        HasCollectedPart = data.hasCollectedPart;
    }
}