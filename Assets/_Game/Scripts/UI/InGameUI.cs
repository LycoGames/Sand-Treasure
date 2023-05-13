using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Inventory inventory;
    [SerializeField] private RectTransform moneyPanel;
    [SerializeField] private RectTransform treasurePanel;
    [SerializeField] private TextMeshProUGUI treasureTotalText;
    [SerializeField] private TextMeshProUGUI foundTreasureText;


    private readonly Vector3 punchScale = new Vector3(0.3f, 0.3f, 0.3f);

    private readonly Vector3 stockScale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        inventory.AddObserver(this);
        moneyText.text = inventory.GetMoney().ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnNotify(int value, ItemType type)
    {
        switch (type)
        {
            case ItemType.Money:
                DOTween.Sequence()
                    .Append(moneyPanel.transform.DOPunchScale(punchScale, 0.1f, 2)).SetEase(Ease.InFlash)
                    .Append(moneyPanel.transform.DOScale(stockScale, 0.1f));
                moneyText.text = value.ToString();
                break;
            case ItemType.Treasure:
                DOTween.Sequence()
                    .Append(treasurePanel.transform.DOPunchScale(punchScale, 0.1f, 2)).SetEase(Ease.InFlash)
                    .Append(treasurePanel.transform.DOScale(stockScale, 0.1f));
                foundTreasureText.text = value.ToString();
                break;
        }
    }
}