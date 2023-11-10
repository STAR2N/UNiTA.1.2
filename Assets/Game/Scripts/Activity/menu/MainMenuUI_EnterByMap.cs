namespace Game.UI
{
    using Game.Network;
    using LightReflectiveMirror;
    using Mirror;
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenuUI_EnterByMap : MonoBehaviour
    {
        // [SerializeField] Image m_Image;
        // [SerializeField] TextMeshProUGUI m_RoomName;
        [SerializeField] Button m_EnterButton;
        [SerializeField] Button m_ExitButton;
        [SerializeField] Button m_NoRoomExitButton;
        [SerializeField] GameObject m_MainView;
        [SerializeField] GameObject m_NoRoom;

        LightReflectiveMirrorTransport Transport => Mirror.Transport.activeTransport as LightReflectiveMirrorTransport;

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

        public void OnPressEnter() {
            if (string.IsNullOrWhiteSpace(User.Name)) {
                return;
            }

            StartHost();
        }

        public void OnExitClicked() {
            // gameObject.SetActive(false);
            m_MainView.SetActive(false);
            m_NoRoom.SetActive(false);
            Destroy(gameObject);
        }

        public void StartHost()
        {
            var network = NetworkManager.singleton;
            if (network != null)
            {
                Transport.serverName = $"Guest_{Random.Range(0, 10000)}";

                ExtraServerData extraServerData = new ExtraServerData();
                extraServerData.HostCharacterIdx = User.VFX;

                Transport.extraServerData = JsonUtility.ToJson(extraServerData);

                Transport.isPublicServer = true;

                NetworkManager.singleton.StartHost();
            }
        }
    }
}