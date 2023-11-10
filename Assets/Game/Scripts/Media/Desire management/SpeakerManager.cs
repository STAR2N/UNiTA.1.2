namespace Game.Media {
    using System.Collections.Generic;
    using FrostweepGames.VoicePro;
    using Mirror;
    using UnityEngine;
    using Game.TransformExtension;
    using System;

    public class SpeakerManager : NetworkBehaviour, IMediaInterface {
        private bool IsInitialized = false;

        [SerializeField] Listener m_Listener;

        #region Unity events
        private void Awake() {
            m_Listener.SpeakersUpdatedEvent += OnSpeakerUpdate;
        }

        private void OnDestroy() {
            m_Listener.SpeakersUpdatedEvent -= OnSpeakerUpdate;
        }
        #endregion

        #region Client
        public override void OnStopClient() {
            base.OnStopClient();

            if (IsInitialized)
                MediaControl.OutputAudioChangeEvent -= OnDesireValueChanged;
        }
        #endregion

        #region Speaker
        public void StartMedia() {
            if (!MediaControl.DesireOutputAudio)
                return;

            m_Listener.StartListen();
        }
        public void StopMedia() { m_Listener.StopListen(); }

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
            MediaControl.DesireOutputAudio = true; // Force enable
            MediaControl.OutputAudioChangeEvent += OnDesireValueChanged;
            OnDesireValueChanged(MediaControl.DesireOutputAudio);
        }
        #endregion

        // Update audio source options
        private void OnSpeakerUpdate(List<Speaker> speakers) {
            foreach(var speaker in speakers) {
                var source = speaker.transform.GetComponent<AudioSource>();
                source.spatialBlend = 0.1f;
            }
        }
        // Helper to control speaker position
        public void AttachSpeaker(GameObject source, int speakerId) {
            if (m_Listener.Speakers.TryGetValue(speakerId, out var speaker)) {
                var copy = source.AddComponent<CopyTransform>();

                copy.Source = speaker.transform;

                copy.CopyTarget = CopyTransform.Target.ToSource;
                copy.CopyMode = CopyTransform.Mode.PositionAndRotation;
                copy.CopyEvent = CopyTransform.Event.Update;
            }
        }
    }
}