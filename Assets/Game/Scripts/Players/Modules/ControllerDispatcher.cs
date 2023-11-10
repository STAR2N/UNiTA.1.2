namespace Game.Controller.Player {
    using UnityEngine;
    using Mirror;

    public class ControllerDispatcher : NetworkBehaviour {
        [SerializeField] GameObject m_ControllerPrefab;
        public IPlayerController CurrentController { get; private set; }

        private void OnDestroy() {
            if (CurrentController != null) {
                Destroy(CurrentController.gameObject);
            }
        }

        public override void OnStartAuthority() {
            base.OnStartAuthority();

            var obj = Instantiate(m_ControllerPrefab, transform);
            CurrentController = obj.GetComponent<IPlayerController>();
            if (CurrentController == null) {
                Destroy(obj);
                return;
            }
            CurrentController.AssignObject(gameObject);
        }

        public override void OnStopAuthority() {
            base.OnStopAuthority();

            if (CurrentController != null) {
                Destroy(CurrentController.gameObject);
                CurrentController = null;
            }
        }
    }
}