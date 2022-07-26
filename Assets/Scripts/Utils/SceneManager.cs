using System;
using System.Collections;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Audio;

namespace Utils
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance;

        [SerializeField] private float transitionTime = 1;
        [SerializeField] private Animator animator;

        private static readonly int Start = Animator.StringToHash("Start");
        private static readonly int End = Animator.StringToHash("End");

        public static event Action OnTransitionedToNewScene;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        private IEnumerator Transition(int scene, bool useTransition)
        {
            animator.ResetTrigger(Start);
            animator.ResetTrigger(End);

            SoundManager.Play(GameConstants.Sounds.SceneTransition);
            if (useTransition)
            {
                animator.SetTrigger(Start);
                yield return new WaitForSeconds(transitionTime);
            }

            PhotonNetwork.LoadLevel(scene);
            yield return new WaitUntil(() => PhotonNetwork.LevelLoadingProgress >= 1);

            OnTransitionedToNewScene?.Invoke();
            SoundManager.Play(GameConstants.Sounds.SceneTransitionFinish);

            if (useTransition)
            {
                animator.SetTrigger(End);
            }
        }

        public static void TransitionToScene(GameConstants.SceneIndices scene, bool useTransition = true)
        {
            _instance.StartCoroutine(_instance.Transition((int)scene, useTransition));
        }
    }
}