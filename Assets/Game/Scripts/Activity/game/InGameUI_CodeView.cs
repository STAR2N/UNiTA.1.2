using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class InGameUI_CodeView : MonoBehaviour
    {
        public static InGameUI_CodeView Instance { get; private set; }

        [SerializeField] Button m_DisconnectButton;
        [SerializeField] TMP_Text m_NameText;
        private string m_NameTextTemplate = null;
        public string NameText
        {
            get => m_NameText.text;
            set => m_NameText.text = m_NameTextTemplate.Replace("{server name}", value);
        }

        [SerializeField] TMP_Text m_CodeText;
        private string m_CodeTextTemplate = null;
        public string CodeText
        {
            get => m_CodeText.text;
            set { m_CodeText.text = m_CodeTextTemplate.Replace("{code}", value); }
        }

        #region Unity events
        private void Awake()
        {
            Instance = this;

            m_NameTextTemplate = NameText;
            m_CodeTextTemplate = CodeText;

            m_DisconnectButton.onClick.AddListener(OnClickDisconnect);
        }

        private Network.ServerData m_DataContainer;
        private void OnEnable() => DataContainer_Init();
        private void Start() => DataContainer_Init();
        private void OnDisable() => DataContainer_Dispose();

        private void Update() {
            if (m_DataContainer == null)
                DataContainer_Init();
        }

        private void DataContainer_Init() {
            if (m_DataContainer != null)
                return;

            m_DataContainer = Network.ServerData.Instance;
            if (m_DataContainer != null) {
                m_DataContainer.OnDataChange += DataContainer_OnDataChange;
                DataContainer_OnDataChange();
            }
        }
        private void DataContainer_Dispose() {
            if (m_DataContainer != null)
                m_DataContainer.OnDataChange -= DataContainer_OnDataChange;
            m_DataContainer = null;
        }
        private void DataContainer_OnDataChange() {
            NameText = m_DataContainer.ServerName;
            CodeText = m_DataContainer.ServerId;
        }
        #endregion

        public void OnClickDisconnect()
        {
            var network = NetworkManager.singleton;
            switch (network.mode)
            {
                case NetworkManagerMode.ClientOnly: network.StopClient(); break;
                case NetworkManagerMode.Host:       network.StopHost(); break;
                case NetworkManagerMode.ServerOnly: network.StopServer(); break;
                default: break;
            }
        }
    }
}