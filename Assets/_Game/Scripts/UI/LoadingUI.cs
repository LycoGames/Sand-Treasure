using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image loadingFill;

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        //TODO on click next level run LoadLevel() method
        //TODO operation= LoadManager.LoadNextLevel();
        AsyncOperation operation = SceneManager.LoadSceneAsync(1); 
        while (!operation.isDone)
        {
            float operationValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingFill.fillAmount += operationValue;
            yield return null;
        }
    }
}