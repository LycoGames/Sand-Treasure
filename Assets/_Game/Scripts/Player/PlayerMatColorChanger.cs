using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using DG.Tweening;
using UnityEngine;

public class PlayerMatColorChanger : MonoBehaviour
{
    [SerializeField] private Material playerMat;
    [SerializeField] private Material playerStockMat;
    private SandType sandType;
    private Color playerStockColor;
    private Coroutine coroutine;
    private WaitForSeconds waitForSeconds;
    private Vector3 scaleSize = new Vector3(0.1f, 0.1f, 0.1f);
    private bool isRunning;

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
            StopColorAndCoroutine();
        }
    }


    private IEnumerator ChangeColorCoroutine()
    {
        isRunning = true;
        while (true)
        {
            DOTween.Kill(transform);
            playerMat.DOColor(Color.red, 1f);
            this.transform.localScale = Vector3.one;
            this.transform.DOPunchScale(scaleSize, 0.5f, 10, 0);
            yield return waitForSeconds;
            playerMat.DOColor(playerStockColor, 1f);
            yield return waitForSeconds;
        }

        isRunning = false;
    }

    private void StopColorAndCoroutine()
    {
        if (coroutine != null)
        {
            isRunning = false;
            DOTween.Kill(transform);
            this.transform.localScale = Vector3.one;
            StopCoroutine(coroutine);
        }
    }

    public void ResetPlayerColor()
    {
        playerMat.color = playerStockMat.color;
        StopColorAndCoroutine();
    }
}