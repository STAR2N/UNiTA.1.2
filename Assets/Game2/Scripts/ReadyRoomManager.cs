// using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.MiniGame.SnowMan
{
    // public class ReadyRoomManager : MonoBehaviourPunCallbacks
    public class ReadyRoomManager : MonoBehaviour
    {
        // [SerializeField]
        // string gameScene;
        // [SerializeField]
        // string menuScene;
        // [SerializeField]
        // TMP_Text infoText;
        // [SerializeField]
        // Button startButton;
        // [SerializeField]
        // Button exitButton;

        // private void Awake()
        // {
        //     if (!PhotonNetwork.InRoom)
        //         SceneManager.LoadScene(menuScene);
        // }

        // private void Start()
        // {
        //     startButton.onClick.AddListener(StartGame);
        //     exitButton.onClick.AddListener(Exit);
        // }

        // private void Update()
        // {
        //     if (PhotonNetwork.InRoom)
        //     {
        //         var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        //         infoText.text = $"시작을 기다리는 중{System.Environment.NewLine}({playerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers})";
        //     }
        // }

        // public void StartGame()
        // {
        //     if (PhotonNetwork.IsMasterClient && enabled)
        //     {
        //         enabled = false;
        //         PhotonNetwork.CurrentRoom.IsOpen = false;
        //         PhotonNetwork.LoadLevel(gameScene);
        //     }
        // }
        // public void Exit()
        // {
        //     if (PhotonNetwork.InRoom)
        //         PhotonNetwork.LeaveRoom();
        //     else
        //         OnLeftRoom();
        // }

        // public override void OnLeftRoom()
        // {
        //     SceneManager.LoadScene(menuScene);
        // }
    }
}