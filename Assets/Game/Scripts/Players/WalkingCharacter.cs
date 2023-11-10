namespace Game.Player {
    using global::CMF;
    using Mirror;
    using TMPro;
    using UnityEngine;
    using Game.Controller;
    using Game.UI;
    using System.Runtime.InteropServices;
    using UnityEngine.Sprites;

    public class WalkingCharacter : NetworkBehaviour {
        [DllImport("__Internal")]
        private static extern void ExitRoom_React(string roomCode, string roomName);

        [SyncVar(hook = nameof(Hook_PlayerName)), HideInInspector] public string PlayerName;
        void Hook_PlayerName(string _, string @new) {
            NameTextPanel.text = @new;
        }
        [SyncVar(hook = nameof(Hook_PlayerVFX)), HideInInspector] public int PlayerVFX;
        void Hook_PlayerVFX(int _, int @new) {
            VFXManager.Index = @new;
        }


        [Header("UI")]
        public TMP_Text NameTextPanel;

        [Header("Controllers")]
        public Rigidbody Rigidbody;
        public Mover Mover;
        public Transform HeadOrigin;
        [SerializeField] public VFXSelector VFXManager;

        [SerializeField]
        public Sprite MiniMapIcon_Host;
        [SerializeField]
        public Sprite MiniMapIcon_Guest;
        [SerializeField]
        public SpriteRenderer MiniMapIcon_Renderer;

        void Start()
        {
            if (isLocalPlayer)
            {
                MiniMapIcon_Renderer.sprite = MiniMapIcon_Host;
            }
            else
            {
                MiniMapIcon_Renderer.sprite = MiniMapIcon_Guest;
            }
        }

        private void Update() {
            if (transform.position.y < -50f) {
                transform.position = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;
            }
        }

        public override void OnStartAuthority() {
            base.OnStartAuthority();
            CmdSetName(User.Name);
            CmdSetVFX(User.VFX);

            var message = new GameNetworkManager.AddUserListMessage{name = User.Name, VFX = User.VFX, isServer = isServer};
            NetworkClient.Send(message);
            NameTextPanel.enabled = false;
        }

        public override void OnStopAuthority() {
            base.OnStopAuthority();

            NameTextPanel.enabled = true;
        }

        [Command]
        private void CmdSetName(string name) {
            PlayerName = name;
        }

        [Command]
        private void CmdSetVFX(int i) {
            PlayerVFX = i;
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcExitRoom(NetworkConnection target, string roomCode, string roomName) {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            ExitRoom_React(roomCode, roomName);
#endif
        }
    }
}