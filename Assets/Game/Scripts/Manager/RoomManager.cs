namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Mirror;
    using System.Runtime.InteropServices;

    public class RoomManager : NetworkBehaviour
    {
        [DllImport("__Internal")]
        private static extern void EnterSpace_React(string spaceName);

        public static RoomManager Instance { get; private set; }
        // public string m_email;

        // ServerName
        private Network.ServerData m_DataContainer;
        private void Start() => DataContainer_Init();
        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            DataContainer_Init();
        }
        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            DataContainer_Dispose();
        }

        public bool isEnterSpace = false;

        private void Update()
        {
            if (m_DataContainer == null)
                DataContainer_Init();
        }

        private void DataContainer_Init()
        {
            if (m_DataContainer != null)
                return;

            m_DataContainer = Network.ServerData.Instance;
            if (m_DataContainer != null)
            {
                m_DataContainer.OnDataChange += DataContainer_OnDataChange;
                DataContainer_OnDataChange();
            }
        }
        private void DataContainer_Dispose()
        {
            if (m_DataContainer != null)
                m_DataContainer.OnDataChange -= DataContainer_OnDataChange;
            m_DataContainer = null;
        }
        private void DataContainer_OnDataChange()
        {
            if (string.IsNullOrWhiteSpace(m_DataContainer.ServerName))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(m_DataContainer.ServerId)) {
                return;
            }

            if(isEnterSpace) {
                return;
            }


            isEnterSpace = true;
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            EnterSpace_React(m_DataContainer.ServerId + m_DataContainer.ServerName);
#endif
        }
    }
}
