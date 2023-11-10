namespace Game.Media {
    using FrostweepGames.VoicePro;
    using Mirror;
    using UnityEngine;

    public class MediaNetworkManager : NetworkBehaviour {
        public static MediaNetworkManager Instance { get; private set; }
        public int UID { get; private set; } = -1;

        [SerializeField] VideoManager m_Video;
        public VideoManager Video => m_Video;
        [SerializeField] VoiceManager m_Microphone;
        public VoiceManager Microphone => m_Microphone;
        [SerializeField] SpeakerManager m_Speaker;
        public SpeakerManager Speaker => m_Speaker;

        #region Unity events
        private void Awake() {
            if (Instance == null)
                Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }
        #endregion

        #region Client
        public override void OnStartClient() {
            base.OnStartClient();
            CmdRequestUID();
        }

        public override void OnStopClient() {
            base.OnStopClient();
            NetworkRouter.Instance.Unregister();
        }
        #endregion

        #region Initializing process
        [Command(channel = Channels.Reliable, requiresAuthority = false)]
        public void CmdRequestUID(NetworkConnectionToClient sender = null) {
            RpcReceiveUID(sender, sender.connectionId);
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcReceiveUID(NetworkConnection target, int uid) {
            UID = uid;
            InitializeMediaSubsystems();
        }

        private void InitializeMediaSubsystems() {
            if (UID < 0)
                return;

            switch (NetworkRouter.Instance.GetNetworkState()) {
                case "Unknown":
                    {
                        var uid = UID;
                        NetworkRouter.Instance.Register(uid, $"net{uid}", Enumerators.NetworkType.Mirror);
                    } break;
            }

            Microphone.Initialize();
            Speaker.Initialize();
            Video.Initialize();
        }
        #endregion
    }
}
