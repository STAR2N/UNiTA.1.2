namespace Game.UI {
    using Game.Network;
    using LightReflectiveMirror;
    using Mirror;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenuUI_CreateRoomView : MonoBehaviour
    {
        [SerializeField] TMP_InputField m_RoomNameInput;
        [SerializeField] Toggle m_SearchToggle;
        [SerializeField] Button m_ShowSpaceButton;
        [SerializeField] Button m_HostButton;
        [SerializeField] GameObject m_spacePopup;

        LightReflectiveMirrorTransport Transport => Mirror.Transport.activeTransport as LightReflectiveMirrorTransport;

        private void Awake()
        {
//            m_RoomNameInput.onSubmit.AddListener(name => StartHost(name, m_SearchToggle.isOn, m_NightToggle.isOn));
            m_HostButton.onClick.AddListener(OnPressHost);
            m_ShowSpaceButton.onClick.AddListener(OnShowSpaceButtonClicked);
        }

        public void OnPressHost() => StartHost(m_RoomNameInput.text, m_SearchToggle.isOn);
        public void OnShowSpaceButtonClicked() 
        { 
            m_spacePopup.SetActive(true);
        }

        public void StartHost(string serverName, bool isPublic)
        {
            if (string.IsNullOrWhiteSpace(GameNetworkManager.SelectedLevel))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(serverName))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(User.Name)) {
                return;
            }

            // var level = GameNetworkManager.Instance.Levels.Levels["Park"];
            // GameNetworkManager.SelectedLevel = (isNight) ? level.Night : level.Day;
            
            Transport.serverName = serverName;

            ExtraServerData extraServerData = new ExtraServerData();
            extraServerData.HostCharacterIdx = User.VFX;

            Transport.extraServerData = JsonUtility.ToJson(extraServerData);

            Transport.isPublicServer = isPublic;

             NetworkManager.singleton.StartHost();
        }

    }
}