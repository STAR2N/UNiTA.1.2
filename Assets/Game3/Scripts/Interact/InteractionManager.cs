using System.Collections.Generic;
using UnityEngine;

namespace iLLi
{
    /// <summary>
    /// Raycast and handle interactable events
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        readonly List<Interactable> interactors = new List<Interactable>();

        #region Raycast params
        [SerializeField] LayerMask interactionLayers;
        [SerializeField] float distance = 10f;
        [SerializeField] Transform rayOrigin;
        #endregion

        public Interactable CurrentInteractable { get; private set; }
        public Interactable CurrentTriggeredInteractable { get; private set; }

        #region Unity evnets
        private void Start()
        {
            GetComponents(interactors);
        }
        private void OnEnable()
        {
            if (rayOrigin == null)
                rayOrigin = transform;
        }
        private void OnDisable()
        {
            Untrigger();
            DefocusInternal();
        }
        private void Update()
        {
            ScanInternal();
        }
        #endregion

        private void ScanInternal()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                DefocusInternal();
                return;
            }

            if (!Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, distance, interactionLayers.value, QueryTriggerInteraction.Collide))
            {
                DefocusInternal();
                return;
            }

            if (!hit.collider.TryGetComponent(out Interactable interactable))
            {
                DefocusInternal();
                return;
            }

            FocusInternal(interactable, new Params {
                Origin = rayOrigin.position,
                Distance = hit.distance,
                Point = hit.point,
                Normal = hit.normal,
            });
            return;
        }

        private void FocusInternal(Interactable interactable, Params param)
        {
            if (interactable == null)
                return;

            if (CurrentInteractable != interactable)
            {
                DefocusInternal();
                CurrentInteractable = interactable;
            }

            CurrentInteractable.OnFocusInteraction(this, param);
        }
        private void DefocusInternal()
        {
            if (CurrentInteractable == null)
                return;

            CurrentInteractable.OnDefocusInteraction(this);
            CurrentInteractable = null;
        }

        public void Trigger()
        {
            if (CurrentInteractable == null)
                return;

            CurrentTriggeredInteractable = CurrentInteractable;
            CurrentTriggeredInteractable.OnTriggerInteraction(this);
        }
        public void Untrigger()
        {
            if (CurrentTriggeredInteractable == null)
                return;

            CurrentTriggeredInteractable.OnUntriggerInteraction(this);
            CurrentTriggeredInteractable = null;
        }

        [System.Serializable]
        public struct Params
        {
            public Vector3 Origin;
            public float Distance;
            public Vector3 Point;
            public Vector3 Normal;
        }
    }
}