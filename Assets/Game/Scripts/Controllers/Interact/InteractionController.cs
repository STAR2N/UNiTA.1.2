namespace Game.Controller {
    using System.Collections.Generic;
    using UnityEngine;

    public class InteractionController : MonoBehaviour {
        public KeyCode InteractionKey = KeyCode.E;
        [SerializeField] string m_InteractEventName;

        public Transform Camera;

        public readonly List<Interactable> Interactables = new List<Interactable>();

        private UI.ButtonEvent.EventListener m_InteractListener;
        private void Awake() {
            m_InteractListener = UI.ButtonEvent.Regist(m_InteractEventName, () => {
                PerformInteraction();
            });
        }
        private void OnDestroy() {
            m_InteractListener?.Dispose();
            m_InteractListener = null;
        }

        private void Update() {
            SortInteraction();
            if (Input.GetKeyDown(InteractionKey)) {
                PerformInteraction();
            }
        }

        public void SortInteraction() {
            Interactables.Sort((l, r) => {
                var ldot = Vector3.Dot(Camera.forward, l.transform.position - Camera.position);
                if (ldot < 0) {
                    return -1;
                }
                var rdot = Vector3.Dot(Camera.forward, r.transform.position - Camera.position);
                if (rdot < 0) {
                    return 1;
                }

                return (ldot >= rdot) ? 1 : -1;
            });
        }
        public void PerformInteraction() {
            if (Interactables.Count > 0) {
                Interactables[0].Interact(this);
            }
        }

        public abstract class Interactable : MonoBehaviour {
            public abstract void Interact(InteractionController controller);
        }
    }
}