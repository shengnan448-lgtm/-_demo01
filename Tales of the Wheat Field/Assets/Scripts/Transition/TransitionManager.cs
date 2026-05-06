using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;
        bool isFade;
        CanvasGroup fadeCanvasGroup;
        private void Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            StartCoroutine(LoadSceneSetActive(startSceneName));
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnTransitionEvent(string sceneName, Vector3 pos)
        {
            if(!isFade)
            StartCoroutine(Transition(sceneName, pos));
        }

        /// <summary>
        /// 加载并激活场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }
        /// <summary>
        /// 卸载场景  激活场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);
            EventHandler.CallMoveToPosition(targetPosition);
            EventHandler.CallAfterSceneloadEvent();
            yield return Fade(0);
        }
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;//拦截用户在隐身时 点击下面的UI内容
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration_A;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha,targetAlpha ))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}

