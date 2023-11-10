using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class InGameUI_Exit : MonoBehaviour
    {
        [SerializeField] Button m_DisconnectButton;

        private void Awake()
        {
            m_DisconnectButton.onClick.AddListener(OnClickDisconnect);
        }

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