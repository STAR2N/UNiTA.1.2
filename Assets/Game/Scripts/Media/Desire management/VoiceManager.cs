namespace Game.Media {
    using FrostweepGames.Plugins.Native;
    using FrostweepGames.VoicePro;
    using Mirror;
    using UnityEngine;

    public class VoiceManager : NetworkBehaviour, IMediaInterface {
        private bool IsInitialized = false;

        [SerializeField] Recorder m_Recorder;

        #region Client
        public override void OnStopClient() {
            base.OnStopClient();

            if (IsInitialized)
                MediaControl.InputAudioChangeEvent -= OnDesireValueChanged;
        }
        #endregion

        #region Microphone
        public void StartMedia() {
            if (!MediaControl.DesireInputAudio)
                return;

            CustomMicrophone.RequestMicrophonePermission();
            if (CustomMicrophone.HasMicrophonePermission()) {
                m_Recorder.SetMicrophone(CustomMicrophone.devices[0]);
                m_Recorder.StartRecord();
            }
        }

        public void StopMedia() { m_Recorder.StopRecord(); }

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
            MediaControl.DesireInputAudio = true; // Force enable
            MediaControl.InputAudioChangeEvent += OnDesireValueChanged;
            OnDesireValueChanged(MediaControl.DesireInputAudio);
        }
        #endregion
    }
}