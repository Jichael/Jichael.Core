using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Jichaels.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] private GameObject canvas;

        [SerializeField] private Image fadeImage;

        [SerializeField] private GameObject progressBar;
        [SerializeField] private Image progressImage;
        [SerializeField] private TextMeshProUGUI progressText;

        [SerializeField, Range(0.01f, 4)] private float fadeSpeed = 2;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadScene(int buildIndex, bool fadeIn, bool fadeOut, bool showProgressBar)
        {
            StartCoroutine(LoadSceneCo(buildIndex, fadeIn, fadeOut, showProgressBar));
        }

        public void LoadScene(string sceneName, bool fadeIn, bool fadeOut, bool showProgressBar)
        {
            LoadScene(SceneUtility.GetBuildIndexByScenePath(sceneName), fadeIn, fadeOut, showProgressBar);
        }

        private IEnumerator LoadSceneCo(int buildIndex, bool fadeIn, bool fadeOut, bool showProgressBar)
        {

            Color fadeColor = fadeImage.color;
            fadeColor.a = 0;
            fadeImage.color = fadeColor;
            
            canvas.SetActive(true);
            
            if (fadeIn) yield return StartCoroutine(FadeImageCo(0, 1));
            
            if (showProgressBar) progressBar.SetActive(true);

            AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);

            while (!op.isDone)
            {
                if (showProgressBar)
                {
                    float loadProgress = op.progress / 0.9f;
                    progressText.text = $"{((int) (loadProgress * 100f)).ToString()} %";
                    progressImage.fillAmount = loadProgress;
                }

                yield return Yielders.EndOfFrame;
            }

            progressBar.SetActive(false);

            if (fadeOut) yield return StartCoroutine(FadeImageCo(1, 0));

            canvas.SetActive(false);
        }

        private IEnumerator FadeImageCo(float start, float end)
        {
            Color fadeColor = fadeImage.color;
            fadeColor.a = start;

            float delta = 0;

            while (!Mathf.Approximately(fadeColor.a, end))
            {
                delta += Time.deltaTime * fadeSpeed;
                fadeColor.a = Mathf.Lerp(start, end, delta);
                fadeImage.color = fadeColor;
                yield return Yielders.EndOfFrame;
            }

            fadeColor.a = end;
            fadeImage.color = fadeColor;
        }
    }
}