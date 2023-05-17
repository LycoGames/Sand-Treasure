using System.Collections;
using _Game.Scripts.Base.UserInterface;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class LoadingUI : AbstractBaseCanvas
    {
        [SerializeField] private Image loadingFill;
       // public float loadingTime;
        public float LoadingTime { get; set; }
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

        public override void OnStart()
        {
            Debug.Log("LoadingUI OnStart");
            StartCoroutine(FillBar());
        }

        public override void OnQuit()
        {
            Debug.Log("LoadingUI OnExit");
            StopAllCoroutines();
        }

        private IEnumerator FillBar()
        {
            float elapsedTime = 0;
            while (elapsedTime <= LoadingTime)
            {
                loadingFill.fillAmount = elapsedTime/LoadingTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}