using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomPackages.Silicom.Core.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] private GameObject canvas;
        [SerializeField] private Image fadeImage;
        [SerializeField] private GameObject progressAnimation;
        [SerializeField] private Transform progressCircle;
        [SerializeField] private GameObject loadText;
        

        [SerializeField, Range(0.01f, 4)] private float fadeSpeed = 2;
        [SerializeField] private float circleSpeed = -400f;

        private bool _animate;

        private void Awake()
        {
            Instance = this;
            canvas.SetActive(false);
        }

        public void LoadScene(int buildIndex, bool fadeIn, bool fadeOut, bool showProgressCircle, bool showText, float minLoadTime)
        {
            StartCoroutine(LoadSceneCo(buildIndex, fadeIn, fadeOut, showProgressCircle, showText, minLoadTime));
        }

        public void LoadScene(string sceneName, bool fadeIn, bool fadeOut, bool showProgressCircle, bool showText, float minLoadTime)
        {
            LoadScene(SceneUtility.GetBuildIndexByScenePath(sceneName), fadeIn, fadeOut, showProgressCircle, showText, minLoadTime);
        }

        private IEnumerator LoadSceneCo(int buildIndex, bool fadeIn, bool fadeOut, bool showProgressCircle, bool showText, float minLoadTime)
        {
            Color fadeColor = fadeImage.color;
            fadeColor.a = fadeOut && !fadeIn ? 1 : 0;
            fadeImage.color = fadeColor;
            
            canvas.SetActive(true);
            
            
            if (fadeIn) yield return StartCoroutine(FadeImageCo(0, 1));

            if (showProgressCircle) StartCoroutine(CircleAnimationCo());
            loadText.SetActive(showText);
            
            AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);
            
            float fakeTimer = 0;
            while (!op.isDone || fakeTimer < minLoadTime)
            {
                fakeTimer += Time.deltaTime;
                yield return Yielders.EndOfFrame;
            }

            loadText.SetActive(false);
            if (showProgressCircle) _animate = false;

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

        private IEnumerator CircleAnimationCo()
        {
            progressAnimation.SetActive(true);
            _animate = true;
            while (_animate)
            {
                progressCircle.Rotate(0, 0, circleSpeed * Time.deltaTime);
                yield return Yielders.EndOfFrame;
            }
            progressAnimation.SetActive(false);
        }
    }
}