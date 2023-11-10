using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainMenuUI_JoinRoomView : MonoBehaviour
    {
        [SerializeField] TMP_InputField m_RoomNameInput;
        [SerializeField] Button m_JoinButton;

        private void Awake()
        {
            m_RoomNameInput.onSubmit.AddListener(code => StartClient(code));
            m_JoinButton.onClick.AddListener(OnPressJoin);
        }

        public void OnPressJoin() {
            if (string.IsNullOrWhiteSpace(User.Name)) {
                return;
            }

            StartClient(m_RoomNameInput.text);
        }

        public void StartClient(string code)
        {
            var network = NetworkManager.singleton;
            if (network != null)
            {
                network.networkAddress = code;
                network.StartClient();
            }
        }
    }
}
