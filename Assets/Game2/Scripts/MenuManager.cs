using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Photon.Pun;

namespace Game.MiniGame.SnowMan
{
    // public class MenuManager : MonoBehaviourPunCallbacks
    public class MenuManager : MonoBehaviour
    {
        // const int MAX_PLAYER = 8;

        // [SerializeField]
        // string readyRoom;
        // [SerializeField]
        // Button joinButton;

        // private void Awake()
        // {
        //     PhotonNetwork.AutomaticallySyncScene = true;
        // }

        // private void Start()
        // {
        //     joinButton.onClick.AddListener(JoinRandomRoom);
        //     joinButton.interactable = false;
        //     PhotonNetwork.ConnectUsingSettings();
        // }

        // public override void OnConnectedToMaster()
        // {
        //     if (!PhotonNetwork.InLobby)
        //         PhotonNetwork.JoinLobby();
        //     else
        //         OnJoinedLobby();
        // }

        // public override void OnJoinedLobby()
        // {
        //     joinButton.interactable = true;
        // }

        // public void JoinRandomRoom()
        // {
        //     if (!PhotonNetwork.InLobby)
        //         return;

        //     joinButton.interactable = false;
        //     PhotonNetwork.JoinRandomOrCreateRoom(expectedMaxPlayers: MAX_PLAYER, roomOptions: new Photon.Realtime.RoomOptions() { MaxPlayers = MAX_PLAYER });
        // }

        // public override void OnCreatedRoom()
        // {
        //     Debug.Log($"{nameof(OnCreatedRoom)}", this);
        //     PhotonNetwork.LoadLevel(readyRoom);
        // }
        // public override void OnJoinedRoom()
        // {
        //     Debug.Log($"{nameof(OnJoinedRoom)}", this);
        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         OnCreatedRoom();
        //     }
        // }
        // public override void OnCreateRoomFailed(short returnCode, string message)
        // {
        //     Debug.LogError($"{nameof(OnCreateRoomFailed)} => {returnCode}:{message}", this);
        //     joinButton.interactable = true;
        // }

        // public override void OnJoinRoomFailed(short returnCode, string message)
        // {
        //     Debug.LogError($"{nameof(OnJoinRoomFailed)} => {returnCode}:{message}", this);
        //     joinButton.interactable = true;
        // }

        // public override void OnJoinRandomFailed(short returnCode, string message)
        // {
        //     Debug.LogError($"{nameof(OnJoinRandomFailed)} => {returnCode}:{message}", this);
        //     joinButton.interactable = true;
        // }
    }
}