namespace Game.UI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Game.Network;
    using Mirror;

    public class MainMenuUI_EnterByLink : MonoBehaviour
    {
        // [SerializeField] Image m_Image;
        // [SerializeField] TextMeshProUGUI m_RoomName;
        [SerializeField] Button m_EnterButton;
        [SerializeField] Button m_ExitButton;
        [SerializeField] Button m_NoRoomExitButton;
        [SerializeField] GameObject m_MainView;
        [SerializeField] GameObject m_NoRoom;

        private string m_code;

        [SerializeField] string ServerListURI = "http://localhost/api/compressed/servers";

        // private void Start()
        // {
        //     CustomLRMRoomList.OnNewServerList += UpdateServerList_Link;
        // }

        // private void OnDestroy() {
        //     CustomLRMRoomList.OnNewServerList -= UpdateServerList_Link;
        // }

        void Awake() 
        {
            m_EnterButton.onClick.AddListener(OnPressEnter);
            m_ExitButton.onClick.AddListener(OnExitClicked);
            m_NoRoomExitButton.onClick.AddListener(OnExitClicked);
        }

        public void SetCode(string code)
        {
            m_code = code;
            // StartCoroutine(CustomLRMRoomList.GetServerList(ServerListURI));
        }

        // private void UpdateServerList_Link()
        // {   
        //     bool hasRoom = false;

        //     foreach (var server in CustomLRMRoomList.RoomList)
        //     {
        //         Debug.Log("ServerId:" + server.serverId);

        //         var data = JsonUtility.FromJson<ExtraServerData>(server.serverData);
        //         if (data.NetworkVersion != ExtraServerData.NETWORK_VERSION)
        //             continue;

        //         if(server.serverId != m_code) 
        //             continue;
                
        //         // m_RoomName.text = server.serverName;
        //         // m_Image.sprite = GameNetworkManager.Instance.CharacterImages.ImageList[data.HostCharacterIdx];
        //         hasRoom = true;
        //     }

        //     if(!hasRoom) {
        //         m_NoRoom.SetActive(true);
        //     }
        // }

        public void OnPressEnter() {
            if (string.IsNullOrWhiteSpace(User.Name)) {
                return;
            }

            StartClient(m_code);
        }

        public void OnExitClicked() {
            // gameObject.SetActive(false);
            m_MainView.SetActive(false);
            m_NoRoom.SetActive(false);
            Destroy(gameObject);
        }

        public void StartClient(string code)
        {
            Debug.Log(code);

            var network = NetworkManager.singleton;
            if (network != null)
            {
                network.networkAddress = code;
                network.StartClient();
            }
        }
    }
}