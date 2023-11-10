namespace Game.UI {
    using Game.Network;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class MainMenuUI_ServerListView : MonoBehaviour
    {
        [SerializeField] TMP_InputField m_SearchInput;
        [SerializeField] ScrollRect m_List;
        [SerializeField] MainMenuUI_ListElement m_ListElement;
        [SerializeField] Direction m_ListDirection = Direction.Vertical;

        [SerializeField] string ServerListURI = "http://localhost/api/compressed/servers";
        public UnityEvent<string> onClickJoin = new UnityEvent<string>();

        private void Start()
        {
            CustomLRMRoomList.OnNewServerList += UpdateServerList;
            m_SearchInput.onValueChanged.AddListener(_ => { ClearList(); UpdateServerList(); });
        }

        private void OnDestroy() {
            CustomLRMRoomList.OnNewServerList -= UpdateServerList;
        }

        private void UpdateServerList()
        {
            var parent = m_List.content.transform;
            var searchKeyword = m_SearchInput.text.Trim();
            Debug.Log($"Received new server list {CustomLRMRoomList.RoomList.Count}");
            foreach (var server in CustomLRMRoomList.RoomList)
            {
                if (!string.IsNullOrWhiteSpace(searchKeyword) && !server.serverName.Contains(searchKeyword))
                    continue;

                var data = JsonUtility.FromJson<ExtraServerData>(server.serverData);
                if (data.NetworkVersion != ExtraServerData.NETWORK_VERSION)
                    continue;

                AddListElement(server.serverId, server.serverName, server.currentPlayers, data.HostCharacterIdx);
            }
        }

        public void RefreshList()
        {
            ClearList();
            StartCoroutine(CustomLRMRoomList.GetServerList(ServerListURI));
        }

        public void ClearList()
        {
            var parent = m_List.content.transform;
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
            parent.DetachChildren();
        }

        public void AddListElement(string id, string name, int currentPlayer, int hostCharacterIdx) {

            var element = Instantiate(m_ListElement.gameObject, m_List.content.transform).GetComponent<MainMenuUI_ListElement>();
            var elementTransform = element.transform as RectTransform;

            elementTransform.pivot = Vector2.one;
            elementTransform.anchorMin = Vector2.zero;
            elementTransform.anchorMax = Vector2.zero;

            switch (m_ListDirection) {
                case Direction.Vertical: {
                        var originRect = m_ListElement.transform as RectTransform;
                        var scale =  m_List.content.rect.width / originRect.rect.width;

                        elementTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_List.content.rect.width);
                        elementTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originRect.rect.height * scale);
                    } break;
                case Direction.Horizontal: {
                        var originRect = m_ListElement.transform as RectTransform;
                        var scale =  m_List.content.rect.height / originRect.rect.height;

                        elementTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originRect.rect.width * scale);
                        elementTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_List.content.rect.height);
                    }
                    break;
            }

            element.onClick.AddListener(() => { onClickJoin.Invoke(element.ServerId); });
            element.ServerId = id;
            element.RoomNameText = name;
            element.PlayerCount = currentPlayer;
            element.HostCharacterIdx = hostCharacterIdx;
        }

        public enum Direction {
            Horizontal,
            Vertical
        }
    }
}