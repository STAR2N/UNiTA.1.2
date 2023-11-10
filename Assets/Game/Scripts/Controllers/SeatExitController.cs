namespace Game.Controller {
    using UnityEngine;
    using Game.Controller.Player;
    using System.Runtime.InteropServices;

    public class SeatExitController : MonoBehaviour {
        [DllImport("__Internal")]
        private static extern void RequestShowIngameReview_React();

        [SerializeField] SeatedCharacterController Controller;

        [Header("Input")]
        [SerializeField] KeyCode m_ExitKey = KeyCode.E;
        [SerializeField] string m_ExitButtonName = "character interact button";

        private UI.ButtonEvent.EventListener m_ExitListener;
        private void Awake() {
            m_ExitListener = UI.ButtonEvent.Regist(m_ExitButtonName, () => {
                PerformStand();
            });
        }
        private void OnDestroy() {
            m_ExitListener?.Dispose();
            m_ExitListener = null;
        }

        private void Update() {
            if (Input.GetKeyDown(m_ExitKey)) {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                if(Controller.Character.isRoomConnected) {
                    PerformStand();
                }
#else
                PerformStand();
#endif
            }
        }

        public void PerformStand() {
            var character = Controller.Character;
            if (character.RelatedSeat != null) {
                var seat = character.RelatedSeat.GetComponent<Seatable>();
                if (seat != null) {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                    RequestShowIngameReview_React();
#endif
                    seat.CmdTryStand();
                }
            }
        }
    }
}