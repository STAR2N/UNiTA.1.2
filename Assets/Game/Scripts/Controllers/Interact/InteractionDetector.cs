namespace Game.Physics {
    using System.Collections.Generic;
    using UnityEngine;
    using Game.Controller;

    public class InteractionDetector : MonoBehaviour {
        private readonly Dictionary<Collider, InteractionController.Interactable> InteractableCache = new Dictionary<Collider, InteractionController.Interactable>();

        [SerializeField] InteractionController Controller;

        private void OnTriggerEnter(Collider other) {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log($"{gameObject}: {nameof(InteractionDetector)}: {nameof(OnTriggerEnter)}: {other}");
#endif
            if (InteractableCache.ContainsKey(other))
                return;

            var interactable = other.GetComponent<InteractionController.Interactable>();
            if (interactable != null) {
                InteractableCache.Add(other, interactable);
                Controller.Interactables.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other) {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log($"{gameObject}: {nameof(InteractionDetector)}: {nameof(OnTriggerExit)}: {other}");
#endif
            if (InteractableCache.TryGetValue(other, out var interactable)) {
                Controller.Interactables.Remove(interactable);
                InteractableCache.Remove(other);
            }
        }
    }
}