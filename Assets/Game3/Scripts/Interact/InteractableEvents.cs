using UnityEngine;
using UnityEngine.Events;

namespace iLLi
{
    /// <summary>
    /// Handle events with inspector
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    [DisallowMultipleComponent]
    public sealed class InteractableEvents : MonoBehaviour
    {
        Interactable interactable;

        [SerializeField] UnityEvent<InteractionManager, InteractionManager.Params> FocusEvent = new UnityEvent<InteractionManager, InteractionManager.Params>();
        [SerializeField] UnityEvent<InteractionManager> DefocusEvent = new UnityEvent<InteractionManager>();
        [SerializeField] UnityEvent<InteractionManager> TriggerEvent = new UnityEvent<InteractionManager>();
        [SerializeField] UnityEvent<InteractionManager> UntriggerEvent = new UnityEvent<InteractionManager>();

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            if (interactable == null)
                return;

            interactable.FocusEvent += FocusEvent.Invoke;
            interactable.DefocusEvent += DefocusEvent.Invoke;
            interactable.TriggerEvent += TriggerEvent.Invoke;
            interactable.UntriggerEvent += UntriggerEvent.Invoke;
        }

        private void OnDestroy()
        {
            if (interactable == null)
                return;

            interactable.FocusEvent -= FocusEvent.Invoke;
            interactable.DefocusEvent -= DefocusEvent.Invoke;
            interactable.TriggerEvent -= TriggerEvent.Invoke;
            interactable.UntriggerEvent -= UntriggerEvent.Invoke;
        }
    }
}