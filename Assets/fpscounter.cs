using System.Collections;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Base.FPS
{
    public class fpscounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] float refreshTime = 1f;
        [SerializeField] private int fpsLimit = 60;
        private float fps;
        private WaitForSeconds wfsForRefreshFPS;
        void Start()
        {
            Application.targetFrameRate = fpsLimit;
            wfsForRefreshFPS = new WaitForSeconds(refreshTime);
            StartCoroutine(GetFps());
        }

        IEnumerator GetFps()
        {
            while (true)
            {
                fps = (int)(1f / Time.unscaledDeltaTime);
                fpsText.text = fps.ToString();
                yield return wfsForRefreshFPS;
            }
        }
    }
}