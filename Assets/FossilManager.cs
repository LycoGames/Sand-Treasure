using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.Saving;
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
    private int currentBodyIndex;
    private CinemachineVirtualCamera virtualCamera;
   [SerializeField] private List<BoneType> collectedBoneTypes = new();
    private FossilBodyController fossilBodyController;

    public void Initialize(CinemachineVirtualCamera vCam)
    {
        virtualCamera = vCam;
    }

    public void InstantiateBody()
    {
        fossilBodyController = Instantiate(fossilBodyPrefabList[currentBodyIndex], this.transform.position+fossilBodyPrefabList[currentBodyIndex].transform.localPosition,
            Quaternion.identity * fossilBodyPrefabList[currentBodyIndex].transform.localRotation, this.transform);
        fossilBodyController.SetupBody(collectedBoneTypes, BodyPartCollected);
    }


    private void BodyPartCollected(BoneType type, Transform bodyPart)
    {
        collectedBoneTypes.Add(type);
        GameManager.Instance.ChangeCamFollowTarget(bodyPart);
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