using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DollarSignText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetMoneyValueText(int value)
    {
        text.text += value.ToString();
        Destroy(this.gameObject,2f);
    }
}
