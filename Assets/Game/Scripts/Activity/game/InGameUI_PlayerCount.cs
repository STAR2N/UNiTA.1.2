

namespace Game.UI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using Game.Network;
    using Mirror;
    using Game.Player;

    public class InGameUI_PlayerCount : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;

        public void UpdatePlayerCount()
        {
            GameNetworkManager manager = NetworkManager.singleton as GameNetworkManager;

            var SeatCharacters = FindObjectsOfType<SeatCharacter>();
            var WalkingCharacters = FindObjectsOfType<WalkingCharacter>();

            int curPlayerNum = SeatCharacters.Length + WalkingCharacters.Length;

            Debug.Log("Seat : " + SeatCharacters.Length + "  Walk : " + WalkingCharacters.Length);

            m_Text.text = curPlayerNum + "/" + manager.maxConnections;
        }
    }

}