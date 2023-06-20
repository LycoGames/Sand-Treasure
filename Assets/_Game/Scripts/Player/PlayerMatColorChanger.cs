using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMatColorChanger : MonoBehaviour
{
    [SerializeField] private Material playerMat;
    [SerializeField] private Material playerStockMat;
    
    private Color playerStockColor;
    private Coroutine coroutine;
    private WaitForSeconds waitForSeconds;
    private Vector3 scaleSize = new Vector3(0.1f, 0.1f, 0.1f);

    void Start()
    {
        waitForSeconds = new WaitForSeconds(1f);
        playerStockColor = playerMat.color;
        ResetPlayerColor();
    }

    public void ChangeColor(bool isChange)
    {
        if (isChange)
        {
            print("not enough power");
            coroutine = StartCoroutine(ChangeColorCoroutine());
        }
        else
        {
            playerMat.DOColor(playerStockColor, 1f);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private IEnumerator ChangeColorCoroutine()
    {
        while (true)
        {
            playerMat.DOColor(Color.red, 1f);
            this.transform.DOPunchScale(scaleSize, 0.5f, 10, 0);
            yield return waitForSeconds;
            playerMat.DOColor(playerStockColor, 1f);
            yield return waitForSeconds;
        }
    }

    public void ResetPlayerColor()
    {
        playerMat.color = playerStockMat.color;
    }
}