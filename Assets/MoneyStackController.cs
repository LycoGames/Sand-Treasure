using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

public class MoneyStackController : MonoBehaviour
{
    [SerializeField] private int columnCount;
    [SerializeField] private int rowCount;
    [SerializeField] private bool reverseAlign;
    [SerializeField] private Vector3 offset;
    [SerializeField] private UIRewardVisualizer rewardVisualizer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private MoneyEffectSpawner moneyEffectSpawner;

    private int index = 0;

    public void Initialize()
    {
        InGameUI inGameUI = UIManager.Instance.GetCanvas(CanvasTypes.InGame) as InGameUI;
        rewardVisualizer.SetDestination(inGameUI.MoneyPanel);
    }

    public void StartSpawningMoney(List<int> moneyList)
    {
        StartCoroutine(SpawnCoroutine(new List<int>(moneyList)));
    }

    private IEnumerator SpawnCoroutine(List<int> list)
    {
        foreach (var money in list)
        {
            var instance = MoneyPool.Instance.GetFromPool();
            index++;
            instance.transform.position = this.transform.position;
            instance.transform.parent = this.spawnPoint;
            instance.transform.DOLocalMove(GetStackPosition(offset, index), 0.2f)
                .OnComplete((() => instance.EnableCollider()));
            instance.value = money;
            instance.OnCollect += VisualiseReward;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void VisualiseReward(Vector3 moneyPos, int value)
    {
        index = 0;
        moneyEffectSpawner.Spawn(value, moneyPos);
        GameManager.Instance.AddMoneyToPlayer(value);
    }

    private Vector3 GetStackPosition(Vector3 itemBounds, int stackItemCount)
    {
        return new Vector3(itemBounds.x * ((stackItemCount - 1) % columnCount),
            itemBounds.y * ((stackItemCount - 1) / (rowCount * columnCount)),
            (reverseAlign ? -1 : 1) * -itemBounds.z * ((stackItemCount - 1) / columnCount % rowCount));
    }
}