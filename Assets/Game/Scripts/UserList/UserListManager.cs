namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Mirror;
    using TMPro;
    using UnityEngine.UI;
    public class UserListManager : NetworkBehaviour
    {
        [SerializeField] private GameObject ListItemPrefab;
        private GameObject ListItemIns;
        private GameObject UserList;
        private GameObject ProfileImage;
        private GameObject ProfileName;
        private GameObject RoomOwnerTag;
        private GameObject RoomName;
        private GameObject[] JoinCounts;
        private Transform[] ChildList;
        private bool _IsServer;
        void Awake()
        {
            UserList = GameObject.FindWithTag("UserListContent");
            RoomName = GameObject.FindWithTag("RoomName");
            JoinCounts = GameObject.FindGameObjectsWithTag("JoinCount");
            if (!UserList || !RoomName || JoinCounts != null)
                return;
        }

        #region Client
        public override void OnStartClient()
        {
            base.OnStartClient();

            GameNetworkManager.OnNewUserList += OnNewUserList;
        }
        public override void OnStopClient()
        {
            base.OnStopClient();

            GameNetworkManager.OnNewUserList -= OnNewUserList;
        }
        #endregion

        #region Receiver
        private void OnNewUserList(GameNetworkManager.UpdateUserListMessage message)
        {
            // 리스트 아이템 전체 삭제
            ChildList = UserList.GetComponentsInChildren<Transform>();
            if (ChildList != null)
            {
                for (int j = 1; j < ChildList.Length; ++j)
                {
                    if (ChildList[j] != null)
                        Destroy(ChildList[j].gameObject);
                }
            }

            GameNetworkManager manager = NetworkManager.singleton as GameNetworkManager;
            for (int i = 0; i < JoinCounts.Length; ++i)
            {
                if (JoinCounts[i] != null)
                {
                    JoinCounts[i].GetComponent<TextMeshProUGUI>().text = message.names.Count + "/" + manager.maxConnections;
                }
            }

            for (int i = 0; i < message.names.Count; ++i)
            {
                // Debug.Log("OnNewUserList name = " + message.names[i] + " VFX = " + message.VFXs[i]);
                if (!UserList || !ListItemPrefab)
                    return;

                _IsServer = message.IsServers[i];


                // 리스트 아이템 인스턴싱
                ListItemIns = Instantiate(ListItemPrefab);

                // 리스트 아이템 자식 오브젝트 세팅
                SetListItemVar();
                ProfileImage.GetComponent<Image>().sprite = GameNetworkManager.Instance.CharacterImages.ImageList[message.VFXs[i]];
                ProfileName.GetComponent<TextMeshProUGUI>().text = message.names[i];
                if (_IsServer)
                {
                    RoomOwnerTag.SetActive(true);
                }
                else
                {
                    RoomOwnerTag.SetActive(false);
                }


                if (!ListItemIns)
                    return;

                ListItemIns.transform.SetParent(UserList.transform, false);
            }
        }
        #endregion

        private void SetListItemVar()
        {
            ProfileImage = ListItemIns.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            ProfileName = ListItemIns.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject;
            RoomOwnerTag = ListItemIns.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject;
        }

        // ServerName
        private Network.ServerData m_DataContainer;
        private void OnEnable() => DataContainer_Init();
        private void Start() => DataContainer_Init();
        private void OnDisable() => DataContainer_Dispose();

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
            // 방 제목 설정
            RoomName.GetComponent<TextMeshProUGUI>().text = m_DataContainer.ServerName;
            Debug.Log(m_DataContainer.ServerName);
        }
    }
}


