using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace Game.UI
{
    public class InGameUI_CopyLink : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void RequestCopyLink_React(string code);

        public static InGameUI_CopyLink Instance { get; private set; }

        [SerializeField] Button m_CopyButton;

        public string CodeText;

        #region Unity events
        private void Awake()
        {
            Instance = this;

            m_CopyButton.onClick.AddListener(OnClickCopy);
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
            CodeText = m_DataContainer.ServerId;
        }
        #endregion

        public void OnClickCopy()
        {
            Debug.Log(CodeText);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            RequestCopyLink_React(CodeText);
#endif
        }
    }
}