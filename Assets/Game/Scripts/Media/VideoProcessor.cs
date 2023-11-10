namespace Game.Media {
    using System.Collections;
    using Mirror;
    using UnityEngine;
    using Game.Utility.Extensions;

    public class VideoProcessor : NetworkBehaviour {
        public static VideoProcessor Instance { get; private set; }

        public static float FRAME_INTERVAL => GameNetworkManager.FrameInterval;
        public static int FRAME_RATE => GameNetworkManager.FrameRate;
        public const float NETWORK_TIMEOUT = 60f;

        public static bool ShouldProcess = false;

        /// <summary>
        /// Param0 : UID
        /// Param1 : Original size of texture
        /// Param2 : Temporal texture (will be deleted after calling event handlers)
        /// </summary>
        public static event System.Action<int, Vector2Int, RenderTexture> OnNewTexture = delegate { };

        public WebCamTexture Webcam { get; private set; }
        [SerializeField] Vector2Int m_TextureCompressSize = new Vector2Int(240, 240);
        [SerializeField, Range(1, 100)] int m_EncodeQuality = 75;

        #region Unity events
        private float m_NextAvailableFrameTime = 0;
        private void Awake() {
            if (Instance == null)
                Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }

        private void Update() {
            if (m_NextAvailableFrameTime < Time.time && !m_SendBuffer.HasValue && Webcam != null && Webcam.isPlaying && ShouldProcess) {
                SendTexture(Webcam);
            }
        }
        #endregion

        #region Client
        public override void OnStartClient() {
            base.OnStartClient();

            GameNetworkManager.OnNewVideoFrame += OnNewVideoFrame;
        }
        public override void OnStopClient() {
            base.OnStopClient();

            GameNetworkManager.OnNewVideoFrame -= OnNewVideoFrame;
        }
        #endregion

        #region Interface
        public void StartVideo() {
            try {
                if (Webcam == null) {
                    string deviceName = null;
                    foreach(var device in WebCamTexture.devices) {
                        // if (device.isFrontFacing) {
                        //     deviceName = device.name;
                        // }

                        deviceName = device.name;
                        break;

                        // if(device.name == "USB2.0 PC CAMERA") {
                        //     deviceName = device.name;
                        // }
                    }

                        Debug.Log(deviceName);

                    if (string.IsNullOrEmpty(deviceName)) {
                        Webcam = new WebCamTexture(m_TextureCompressSize.x, m_TextureCompressSize.y, GameNetworkManager.FrameRate);
                    Debug.Log("in");
                    } else {
                        Webcam = new WebCamTexture(deviceName, m_TextureCompressSize.x, m_TextureCompressSize.y, GameNetworkManager.FrameRate);
                    Debug.Log("out");
                    }

                    Webcam.Play();
                    Debug.Log("Play in");
                } else {
                    Webcam.Play();
                    Debug.Log("Play out");
                }
            } catch (System.Exception e) {
                Debug.LogError(e);
            }
        }

        public void StopVideo() {
            if (Webcam != null)
                Webcam.Stop();
        }
        #endregion

        #region Receiver
        private void OnNewVideoFrame(GameNetworkManager.OutVideoFrameMessage video) {
            var tex = new Texture2D(video.Size.x, video.Size.y, TextureFormat.RGBA32, false);

            try { // First try apply image from input data
                var result = tex.LoadImage(video.Data);
                tex.Apply();

                var rt = RenderTexture.GetTemporary(tex.width, tex.height);
                try { // Second try broadcast image to handlers
                    Graphics.Blit(tex, rt);
                    OnNewTexture(video.uid, video.OriginalSize, rt);
                } catch (System.Exception e) {
                    Debug.LogError(e, this);
                } finally {
                    RenderTexture.ReleaseTemporary(rt);
                }
            } catch (System.Exception e) {
                Debug.LogError(e, this);
            } finally {
                Destroy(tex);
            }
        }
        #endregion

        #region Sender
        private GameNetworkManager.InVideoFrameMessage? m_SendBuffer = null;
        public void SendTexture(RenderTexture rt) {
            if (m_SendBuffer.HasValue)
                return;

            var tex = rt.ToTexture2D();
            SendTexture(tex);
            Destroy(tex);
        }
        public void SendTexture(WebCamTexture wt) {
            if (m_SendBuffer.HasValue)
                return;

            var tex = wt.ToTexture2D();
            SendTexture(tex);
            Destroy(tex);
        }

        public void SendTexture(Texture2D tex) {
            if (m_SendBuffer.HasValue)
                return;

            var output = tex.Resize(m_TextureCompressSize);

            var size = new Vector2Int(output.width, output.height);
            var originalSize = new Vector2Int(tex.width, tex.height);
            var bytes = output.EncodeToJPG(m_EncodeQuality);
            
            Destroy(output);

            m_SendBuffer = new GameNetworkManager.InVideoFrameMessage { Size = size, OriginalSize = originalSize, Data = bytes };
            CmdPrepareNextSending();
            StartCoroutine(Timeout(new WaitForSecondsRealtime(NETWORK_TIMEOUT), () => {
                m_SendBuffer = null;
            }));
            m_NextAvailableFrameTime = Time.time + FRAME_INTERVAL;
        }
        #endregion

        #region Network
        [Command(channel = Channels.Reliable, requiresAuthority = false)]
        public void CmdPrepareNextSending(NetworkConnectionToClient sender = null) {
            var connId = sender.connectionId;
            if (!GameNetworkManager.NextAvailableFrameTime.TryGetValue(connId, out var next) || next > Time.realtimeSinceStartup) {
                RpcIsNextSendingAvailable(sender, false);
            } else {
                RpcIsNextSendingAvailable(sender, true);
            }
        }

        [TargetRpc(channel = Channels.Reliable)]
        public void RpcIsNextSendingAvailable(NetworkConnection target, bool avaliable) {
            if (avaliable && m_SendBuffer.HasValue) {
                NetworkClient.Send(m_SendBuffer.Value, Channels.Unreliable);
            }

            m_SendBuffer = null;
            StopAllCoroutines();
        }

        IEnumerator Timeout(WaitForSecondsRealtime waiter, System.Action onTimeout) {
            yield return waiter;
            onTimeout();
        }
        #endregion

#if UNITY_EDITOR
        private void Reset() {
            m_EncodeQuality = 75;
        }

        private void OnValidate() {
            m_EncodeQuality = Mathf.Clamp(m_EncodeQuality, 1, 100);
        }
#endif
    }
}