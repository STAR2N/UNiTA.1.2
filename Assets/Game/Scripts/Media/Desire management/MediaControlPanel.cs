namespace Game.Media {
    using FrostweepGames.Plugins.Native;
    using FrostweepGames.VoicePro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MediaControlPanel : MonoBehaviour {
        [SerializeField] Toggle m_InputAudio;
        [SerializeField] Toggle m_OutputAudio;
        [SerializeField] Toggle m_InputVideo;

        #region Unity events
        private void Awake() {
            m_InputAudio.isOn = !MediaControl.DesireInputAudio;
            m_OutputAudio.isOn = !MediaControl.DesireOutputAudio;
            m_InputVideo.isOn = !MediaControl.DesireInputVideo;

            m_InputAudio.onValueChanged.AddListener(value => MediaControl.DesireInputAudio = !value);
            m_OutputAudio.onValueChanged.AddListener(value => MediaControl.DesireOutputAudio = !value);
            m_InputVideo.onValueChanged.AddListener(value => MediaControl.DesireInputVideo = !value);
        }
        #endregion
    }

    public static class MediaControl {

        public static event System.Action<bool> InputAudioChangeEvent = delegate { };
        private static bool m_EnableInputAudio = false;
        public static bool DesireInputAudio {
            get => m_EnableInputAudio;
            set {
                m_EnableInputAudio = value;
                if (value) {
                    CustomMicrophone.RequestMicrophonePermission();
                }
                InputAudioChangeEvent(value);
            }
        }
        public static event System.Action<bool> OutputAudioChangeEvent = delegate { };
        private static bool m_EnableOutputAudio = true; // 2021-09-30 : Set default to enable
        public static bool DesireOutputAudio {
            get => m_EnableOutputAudio;
            set {
                m_EnableOutputAudio = value;
                OutputAudioChangeEvent(value);
            }
        }
        public static event System.Action<bool> InputVideoChangeEvent = delegate { };
        private static bool m_EnableInputVideo = true; // 2021-09-30 : There's no interface to enable it, force to do
        public static bool DesireInputVideo {
            get => m_EnableInputVideo;
            set {
                m_EnableInputVideo = value;
                InputVideoChangeEvent(value);
            }
        }
    }
}