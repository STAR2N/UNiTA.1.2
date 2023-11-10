
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Game.Media;
using Game.UI;
using Game.TransformExtension;
using Game.Controller.Player;
using System;
using Game.Controller;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Game.Player {
    public class SeatCharacter : NetworkBehaviour {
        [DllImport("__Internal")]
        private static extern void JoinRoom_React(string roomCode, string roomName, string uid);

        private Texture2D tex;
        public bool isRoomConnected = false;

        [SyncVar(hook = nameof(Hook_RelatedSeat)), HideInInspector] public NetworkIdentity RelatedSeat;
        private void Hook_RelatedSeat(NetworkIdentity @old, NetworkIdentity @new) {
            Copy.Source = null;
            if (@new != null) {
                Copy.Source = @new.transform;
                var seat = @new.GetComponent<Seatable>();
                if (seat != null) {
                    var copy = VideoVFX.GetComponent<CopyTransform>();
                    if (copy == null) {
                        copy = VideoVFX.AddComponent<CopyTransform>();
                    }

                    copy.CopyEvent = CopyTransform.Event.Update;
                    copy.CopyMode = CopyTransform.Mode.Position;
                    copy.CopyTarget = CopyTransform.Target.FromSource;

                    copy.Source = seat.WebcamPosition;
                    VideoVFX.transform.position = seat.WebcamPosition.position;
                }
            }
        }
        [HideInInspector] public bool IsAssigned { get; private set; }
        [HideInInspector] public int UID { get; private set; }
        [SyncVar, HideInInspector] public string Name;

        [Header("Components")]
        [SerializeField] CopyTransform Copy;

        [Header("VFX")]
        public GameObject CharacterVFX;
        public GameObject VideoVFX;

        [Header("Webcam view")]
        public RectTransform WebcamCanvasRect;
        public RawImage WebcamView;

        private RenderTexture m_ReceivedTexture;

        private string m_CurrentRoom = string.Empty;
        public string CurrentRoom {
            get => m_CurrentRoom;
            private set {
                m_CurrentRoom = value;
                OnRoomChange.Invoke(m_CurrentRoom);
            }
        }
        public UnityEvent<string> OnRoomChange = new UnityEvent<string>();

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

        #region Unity events
        private void Awake() {
            ShowCharacter();

            tex = new Texture2D(2, 2);
            VideoProcessor.OnNewTexture += OnNewTexture;
        }

        private void OnDestroy() {
            VideoProcessor.OnNewTexture -= OnNewTexture;

            if (m_ReceivedTexture != null)
                Destroy(m_ReceivedTexture);
        }
        #endregion

        #region Client
        public override void OnStartLocalPlayer() {
            base.OnStartLocalPlayer();

            ShowWebcam();
            WebcamView.texture = VideoProcessor.Instance.Webcam;
        }
        #endregion

        #region RPC
        [TargetRpc(channel = Channels.Reliable)]
        public void RpcNotifyRoom(string room) {
            CurrentRoom = room;
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcSetVisible(NetworkConnection target, int uid) {
            IsAssigned = true;
            UID = uid;
            ShowWebcam();
            WebcamView.color = Color.white;
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcSetInvisible(NetworkConnection target) {
            IsAssigned = false;
            ShowCharacter();
            WebcamView.color = Color.black;
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcJoinRoom(NetworkConnection target, string roomCode, string roomName, int uid) {
            // Debug.Log("RpcJoinRoom " + uid);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            JoinRoom_React(roomCode, roomName, uid.ToString());
#endif
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcSetName(NetworkConnection target, string name) {
            gameObject.name = name;
        }

        #endregion

        #region VFX management
        public void ShowCharacter() {
            CharacterVFX.SetActive(true);
            VideoVFX.SetActive(false);

            if (m_ReceivedTexture != null) {
                Destroy(m_ReceivedTexture);
                m_ReceivedTexture = null;
                WebcamView.texture = null;
            }
        }
        public void ShowWebcam() {
            CharacterVFX.SetActive(false);
            VideoVFX.SetActive(true);
        }
        #endregion

        #region Webcam management
        private void OnNewTexture(int uid, Vector2Int size, RenderTexture rt) {
            if (!IsAssigned)
                return;

            if (UID != uid)
                return;

            var canvasSize = WebcamCanvasRect.sizeDelta;
            canvasSize.x = canvasSize.y * size.x / size.y;
            WebcamCanvasRect.sizeDelta = canvasSize;

            if (m_ReceivedTexture == null) {
                m_ReceivedTexture = new RenderTexture(rt);
                WebcamView.texture = m_ReceivedTexture;
            } else {
                Graphics.Blit(rt, m_ReceivedTexture);
            }
        }
        #endregion

        public void comm_setImage (string param){
            JObject o = JObject.Parse(param);
            string id = o["id"].ToString();
            string base64str = o["data"].ToString();

            byte[]  imageBytes = Convert.FromBase64String(base64str);
            tex.LoadImage( imageBytes );
            
            WebcamView.texture = tex;
        }

        public void comm_onRoomConnected (){
            isRoomConnected = true;
        }

        // public void comm_onSetParticipants (){
        //     Invoke("onSetParticipants", 1);
        // }

        // public void onSetParticipants () {
        //     // Debug.Log("comm_onSetParticipants");
        //     GameObject.Find("SeatCharacterController(Clone)").gameObject.GetComponent<SeatedCharacterController>().CheckVisibleCharacters();
        // }
    }
}