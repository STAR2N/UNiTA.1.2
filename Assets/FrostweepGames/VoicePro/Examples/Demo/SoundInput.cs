using FrostweepGames.VoicePro;
using UnityEngine;

namespace FrostweepGames.VoicePro.Examples
{
    public class SoundInput : MonoBehaviour
    {
        public Recorder recorder;

        public Listener listener;

        public bool isRecording;


        void Start()
        {
            int id = Random.Range(0, 100);
            NetworkRouter.Instance.Register(id, "Speaker" + id, Enumerators.NetworkType.PUN2);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && !isRecording)
            {
                StartRecord();
                isRecording = true;
            }
            else if (Input.GetKeyUp(KeyCode.R) && isRecording)
            {
                StopRecord();
                isRecording = false;
            }
        }

        public void StartRecord()
        {
            recorder.StartRecord();
        }

        public void StopRecord()
        {
            recorder.StopRecord();
        }
    }
}