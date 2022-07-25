using System.Collections;
using System.Collections.Generic;
using Base;
using DG.Tweening;
using Games.Base;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using UnityEngine;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Games.ScrambledEggs.Procedure
{
    public class VoteResult : MonoBehaviour
    {
        [SerializeField] private Stage5SubmissionVote mostLiked;
        [SerializeField] private Transform row;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject tieMessage;
        
        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        private IEnumerator Start()
        {
            tieMessage.SetActive(false);
            backButton.SetActive(false);
            mostLiked.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            var rectTransform = mostLiked.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;
            mostLiked.gameObject.SetActive(true);
            mostLiked.GetComponent<Button>().interactable = false;
            var votes = GlobalData.Read<List<Vote>>(GameConstants.GlobalData.ScrambledEggsVoteResult);
            var index = votes.MostVoted(out var ties);
            var submissions = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).Stage5Submissions;
            var paintingSubmission = submissions[index];
            mostLiked.Init(index, paintingSubmission.SubmissionContent, paintingSubmission.Context);
            rectTransform.DOScale(new Vector3(1, 1, 1), 1f);

            var tied = false;
            
            foreach (var tie in ties)
            {
                tied = true;
                
                var tieShowcase = Instantiate(mostLiked, row);
                tieShowcase.gameObject.SetActive(false);
                var component = tieShowcase.GetComponent<RectTransform>();
                component.localScale = Vector3.zero;
                tieShowcase.gameObject.SetActive(true);
                tieShowcase.GetComponent<Button>().interactable = false;
                var tieSubmission = submissions[tie];
                tieShowcase.Init(index, tieSubmission.SubmissionContent, tieSubmission.Context);
                component.DOScale(new Vector3(1, 1, 1), 1f);
            }

            if (tied)
            {
                tieMessage.SetActive(true);
                tieMessage.GetComponent<CanvasGroup>().DOFade(1, 1f);
            }
            
            yield return new WaitForSeconds(2f);
            backButton.SetActive(true);
            backButton.GetComponent<CanvasGroup>().DOFade(1, 0.6f).SetEase(Ease.InOutCubic);
        }

        private void OnDestroy()
        {
            Destroy(LobbyData.Instance.gameObject);
        }
    }
}