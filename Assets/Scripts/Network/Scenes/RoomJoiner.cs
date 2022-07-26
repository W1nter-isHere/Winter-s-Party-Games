using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class RoomJoiner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject backButton;

        private void Start()
        {
            text.text = "Joining...";
            backButton.SetActive(false);
            PhotonNetwork.JoinRoom(GlobalData.Read<string>(GameConstants.GlobalData.RoomIDToJoin));
        }
        
        public override void OnJoinedRoom()
        {
            SceneManager.TransitionToScene(RoomData.Read<GameConstants.SceneIndices>(GameConstants.CustomRoomProperties.Scene));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            backButton.SetActive(true);
            text.text = "Failed to join room.\nReason: " + message;
        }
    }
}