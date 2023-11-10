using System;
using UnityEngine;

namespace iLLi
{
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public sealed class Interactable : MonoBehaviour
    {
        public event Action<InteractionManager, InteractionManager.Params> FocusEvent = delegate { };
        public event Action<InteractionManager> DefocusEvent = delegate { };

        public event Action<InteractionManager> TriggerEvent = delegate { };
        public event Action<InteractionManager> UntriggerEvent = delegate { };

        public void OnFocusInteraction(InteractionManager manager, InteractionManager.Params param) => FocusEvent(manager, param);
        public void OnDefocusInteraction(InteractionManager manager) => DefocusEvent(manager);

        public void OnTriggerInteraction(InteractionManager manager) => TriggerEvent(manager);
        public void OnUntriggerInteraction(InteractionManager manager) => UntriggerEvent(manager);
    }
}