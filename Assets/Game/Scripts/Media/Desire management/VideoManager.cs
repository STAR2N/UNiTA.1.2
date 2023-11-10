namespace Game.Media {
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Send/Receive single video frame
    /// </summary>

    [DisallowMultipleComponent]
    public class VideoManager : NetworkBehaviour, IMediaInterface {
        private bool IsInitialized = false;

        [SerializeField] VideoProcessor m_Processor;
        public VideoProcessor Processor => m_Processor;

        #region Client
        public override void OnStopClient() {
            base.OnStopClient();

            if (IsInitialized)
                MediaControl.InputVideoChangeEvent -= OnDesireValueChanged;
        }
        #endregion


        #region Webcam
        public void StartMedia() {
            if (!MediaControl.DesireInputVideo)
                return;

            m_Processor.StartVideo();
        }

        public void StopMedia() { m_Processor.StopVideo(); }

        public void OnDesireValueChanged(bool enabled) {
            if (enabled) {
                StartMedia();
            } else {
                StopMedia();
            }
        }

        public void Initialize() {
            if (IsInitialized)
                return;

            IsInitialized = true;
            MediaControl.InputVideoChangeEvent += OnDesireValueChanged;
            OnDesireValueChanged(MediaControl.DesireInputVideo);
        }
        #endregion
    }
}