using UnityEngine;

namespace iLLi
{
    [RequireComponent(typeof(Interactable))]
    public class Inspectable : MonoBehaviour
    {
        Interactable interactable;

        [SerializeField] GameObject graphicsPrefab;

        #region Unity events
        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.TriggerEvent += TriggerInternal;
                interactable.UntriggerEvent += UntriggerInternal;
            }
        }
        private void OnDestroy()
        {
            if (interactable != null)
            {
                interactable.TriggerEvent -= TriggerInternal;
                interactable.UntriggerEvent -= UntriggerInternal;
            }
        }
        #endregion

        #region Input
        private void TriggerInternal(InteractionManager obj)
        {
            if (!obj.TryGetComponent(out InspectManager manager))
                return;
            manager.Select(this);
        }
        private void UntriggerInternal(InteractionManager obj)
        {
            if (!obj.TryGetComponent(out InspectManager manager))
                return;
            manager.Deselect(this, obj.CurrentInteractable != interactable);
        }
        #endregion

        public void Show()
        {
            if (graphicsPrefab == null)
                return;

            var ui = FindObjectOfType<InspectUI>(true);
            if (ui == null)
                return;

            ui.Show(graphicsPrefab);
        }
    }
}