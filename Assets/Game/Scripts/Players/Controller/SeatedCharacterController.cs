namespace Game.Controller.Player {
    using System.Collections;
    using Game.CMF;
    using Game.PhysicsExtension;
    using Game.Player;
    using UnityEngine;
    using Game.UI;

    public class SeatedCharacterController : IPlayerController {
        public SeatCharacter Character { get; private set; }

        [Header("Controllers")]
        //[SerializeField] CameraController CameraController;
        [SerializeField] public SpectatePositionController Spectator;

        [Header("Camera")]
        [SerializeField] private SpringArm m_CamraArm;
        public SpringArm CameraArm => m_CamraArm;
        [SerializeField] private Camera m_Camera;
        public Camera Camera => m_Camera;

        [Header("Sound")]
        public AudioClip EnterClip;

        private GameObject m_SitMenu;
        private GameObject m_Header;

        public override void AssignObject(GameObject obj) {
            Character = obj.GetComponent<SeatCharacter>();

            Character.OnRoomChange.AddListener(OnRoomChanged);
            if (!string.IsNullOrEmpty(Character.CurrentRoom)) {
                OnRoomChanged(Character.CurrentRoom);
            }
            Spectator.OnPositionChanged.AddListener(() => {
                //CameraController.SetRotationAngles(0, 0);
                // CheckVisibleCharacters();
            });

            PlaySound(EnterClip);
        }

//         public void CheckVisibleCharacters() {
// #if UNITY_WEBGL == true && UNITY_EDITOR == false
//             // Debug.Log("comm_onSetParticipants Test");
//             if(Character) {
//                 if(Character.RelatedSeat) {
//                     if(Character.RelatedSeat.GetComponent<Seatable>()) {
//                         Character.RelatedSeat.GetComponent<Seatable>().CheckVisibleCharacters(Camera);
//                     } else {
//                         Debug.Log("No GetComponent");
//                     }
//                 } else {
//                     Debug.Log("No RelatedSeat");
//                 }
//             } else {
//                 Debug.Log("No Character");
//             }
// #endif
//         }
        
        private void Start()
        {
            //m_SitMenu = GameObject.Find("Game_Canvas(Clone)").GetComponent<InGameUI>().m_SitMenu;
            //m_Header = GameObject.Find("Game_Canvas(Clone)").GetComponent<InGameUI>().m_Header;
            //m_SitMenu.SetActive(true);
            // m_Header.SetActive(false);

            // CanvasGroup header_CanvasGroup = m_Header.GetComponent<CanvasGroup>();
            // header_CanvasGroup.alpha = 0f;
            // header_CanvasGroup.interactable = false;
            // header_CanvasGroup.blocksRaycasts = false;
        }

        private void OnEnable() {
            Media.VideoProcessor.ShouldProcess = true;
        }
        private void OnDisable() {
            Media.VideoProcessor.ShouldProcess = false;
        }

        private void OnDestroy() {
            if (Character != null) {
                Character.OnRoomChange.RemoveListener(OnRoomChanged);
            }
            PlaySound(EnterClip);
            //m_SitMenu.SetActive(false);
            // m_Header.SetActive(true);

            // CanvasGroup header_CanvasGroup = m_Header.GetComponent<CanvasGroup>();
            // header_CanvasGroup.alpha = 1f;
            // header_CanvasGroup.interactable = true;
            // header_CanvasGroup.blocksRaycasts = true;
        }

        private void OnRoomChanged(string room) {
            Spectator.LoadPositions(room);
//            Spectator.InsertPosition(0, Character.transform);
            Spectator.MoveToPosition(0);
        }

        public void PlaySound(AudioClip clip) {
            if (clip == null)
                return;

            var effector = new GameObject("Effector");
            effector.transform.position = Character.transform.position;
            effector.AddComponent<AudioClipPlayer>().Play(clip);
        }

        private class AudioClipPlayer : MonoBehaviour {
            public void Play(AudioClip clip) {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = false;
                source.Play();

                StartCoroutine(DestroyDelay(clip.length));
            }

            IEnumerator DestroyDelay(float wait) {
                yield return new WaitForSecondsRealtime(wait);
                Destroy(gameObject);
            }
        }
    }
}