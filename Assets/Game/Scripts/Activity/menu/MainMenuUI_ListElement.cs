using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Game.UI
{
    public class MainMenuUI_ListElement : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] Button m_ListButton;

        [Header("Display")]
        [SerializeField] TMPro.TMP_Text m_RoomNameText;
        [SerializeField] Image m_HostImage;
        private string m_RoomNameTextTemplate = null;
        public string RoomNameText
        {
            get => m_RoomNameText.text;
            set => m_RoomNameText.text = m_RoomNameTextTemplate.Replace("{server name}", value);
        }

        [SerializeField] TMPro.TMP_Text m_PlayerCountText;
        private string m_PlayerCountTextTemplate = null;
        public string PlayerCountText => m_PlayerCountText.text;
        public int PlayerCount
        {
            set
            {
                GameNetworkManager manager = NetworkManager.singleton as GameNetworkManager;
                int maxPlayerNum = manager.maxConnections;
                Debug.Log(manager.numPlayers);
                m_PlayerCountText.text = m_PlayerCountTextTemplate.Replace("{count}", $"{value}").Replace("{maxPlayer}", $"{maxPlayerNum}");
            }
        }

        public string ServerId { get; set; }

        public int HostCharacterIdx
        {
            set
            {
                Sprite image = GameNetworkManager.Instance.CharacterImages.ImageList[value];
                m_HostImage.sprite = image;
            }
        }


        public Button.ButtonClickedEvent onClick => m_ListButton.onClick;

        private void Awake()
        {
            m_PlayerCountTextTemplate = m_PlayerCountText.text;
            m_RoomNameTextTemplate = m_RoomNameText.text;
        }
    }
}