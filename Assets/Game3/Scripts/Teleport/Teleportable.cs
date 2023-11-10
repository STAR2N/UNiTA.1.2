using UnityEngine;

namespace iLLi
{
    [RequireComponent(typeof(Interactable))]
    public class Teleportable : MonoBehaviour
    {
        Interactable interactable;

        #region Unity events
        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.FocusEvent += FocusInternal;
                interactable.DefocusEvent += DefocusInternal;
                interactable.TriggerEvent += TriggerInternal;
                interactable.UntriggerEvent += UntriggerInternal;
            }
        }
        private void OnDestroy()
        {
            if (interactable != null)
            {
                interactable.FocusEvent -= FocusInternal;
                interactable.DefocusEvent -= DefocusInternal;
                interactable.TriggerEvent -= TriggerInternal;
                interactable.UntriggerEvent -= UntriggerInternal;
            }
        }
        #endregion

        #region Input
        private void FocusInternal(InteractionManager interactionManager, InteractionManager.Params param)
        {
            if (!interactionManager.TryGetComponent(out TeleportManager teleportManager))
                return;
            teleportManager.Focus(this, param);
        }
        private void DefocusInternal(InteractionManager interactionManager)
        {
            if (!interactionManager.TryGetComponent(out TeleportManager teleportManager))
                return;
            teleportManager.Defocus(this);
        }
        private void TriggerInternal(InteractionManager interactionManager)
        {
            if (!interactionManager.TryGetComponent(out TeleportManager teleportManager))
                return;
            teleportManager.Select(this);
        }
        private void UntriggerInternal(InteractionManager interactionManager)
        {
            if (!interactionManager.TryGetComponent(out TeleportManager teleportManager))
                return;
            teleportManager.Deselect(this, interactionManager.CurrentInteractable != interactable);
        }
        #endregion
    }
}