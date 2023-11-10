using UnityEngine;
using System.Runtime.InteropServices;

namespace Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void checkEnterByLink_React();

        [Header("Views")]
        [SerializeField] MainMenuUI_CreateRoomView m_CreateView; // This field does not need but for indicate relative scripts
        [SerializeField] MainMenuUI_JoinRoomView m_JoinView;
        [SerializeField] MainMenuUI_ServerListView m_ListView;
        [SerializeField] GameObject m_MainView;
        [SerializeField] GameObject m_EnterByLinkView;
        [SerializeField] GameObject m_EnterByLinkMap;

        [Header("UI")]
        [SerializeField] TMPro.TMP_InputField m_NameInput;

        private void Awake()
        {
            // Invoke("comm_onEnterByLink2", 1);

#if UNITY_WEBGL == true && UNITY_EDITOR == false
            checkEnterByLink_React();
#endif

            Debug.Log($"VERSION INFO\nNetwork: {Network.ExtraServerData.NETWORK_VERSION}\nApplication: {Network.ExtraServerData.APPLICATION_VERSION}");

            m_ListView.onClickJoin.AddListener(m_JoinView.StartClient);
            m_NameInput.onValueChanged.AddListener(name => User.Name = name);

            // Apply params
            var startup = StartupManager.Instance;
            if (startup != null)
            {
                var roomCode = startup.Params.Room;
                startup.Params.Room = null; // One shot
                if (!string.IsNullOrEmpty(roomCode))
                {
                    Debug.Log($"Found room code \"{roomCode}\"");
                    comm_onEnterByLink(roomCode);
                }
                var mapCode = startup.Params.Room;
                startup.Params.Room = null; // One shot
                if (!string.IsNullOrEmpty(mapCode))
                {
                    if (GameNetworkManager.Instance.Levels.Levels.TryGetValue(mapCode, out var level))
                    {
                        GameNetworkManager.SelectedLevel = level.Scene;
                        m_MainView.SetActive(true);
                        m_EnterByLinkMap.SetActive(true);
                        Debug.Log($"Found map code \"{mapCode}\"");
                    }
                    else
                    {
                        Debug.Log($"Found map code \"{mapCode}\" but not exist");
                    }
                }
            } else
            {
                Debug.LogError("Fail to find <StartupManager>");
            }
        }

        private void OnEnable() {
            var userName = User.Name;
            if (!string.IsNullOrWhiteSpace(userName)) {
                m_NameInput.text = userName;
            }
        }

        // private void comm_onEnterByLink2()
        // {
        //     comm_onEnterByLink("HUWYL");
        // }

        private void comm_onEnterByLink(string code)
        {
            m_MainView.SetActive(true);
            m_EnterByLinkView.SetActive(true);
            m_EnterByLinkView.GetComponent<MainMenuUI_EnterByLink>().SetCode(code);
        }
    }
}