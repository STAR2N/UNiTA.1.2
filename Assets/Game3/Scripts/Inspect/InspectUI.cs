using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace iLLi
{
    public class InspectUI : MonoBehaviour
    {
        [SerializeField] Transform modelContainer;
        [SerializeField]
#if UNITY_EDITOR
        new
#endif
        Camera camera;
        public Camera Camera => camera;

        [SerializeField] UnityEvent openEvent = new UnityEvent();
        [SerializeField] UnityEvent closeEvent = new UnityEvent();

        float fov = 60;
        float zoom = 0;

        [SerializeField] AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 1, 1, 8);
        [SerializeField] float zoomSensitivity = 0.1f;

        private void OnEnable()
        {
            fov = camera.fieldOfView;
        }
        private void OnDisable()
        {
            camera.fieldOfView = fov;
            zoom = 0;
        }
        private void Update()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            zoom = Mathf.Clamp01(zoom + scroll * zoomSensitivity);
            var calculatedZoom = zoomCurve.Evaluate(zoom);
            if (calculatedZoom == 0)
            {
                camera.fieldOfView = fov;
            } else
            {
                camera.fieldOfView = fov / calculatedZoom;
            }
        }

        public void Show(GameObject prefab)
        {
            ClearObjectsInternal();
            var model = Instantiate(prefab, modelContainer);
            if (model != null)
            {
                var canvases = new List<Canvas>();
                model.GetComponentsInChildren(true, canvases);
                foreach(var canvas in canvases)
                {
                    canvas.worldCamera = camera;
                }
            }
            gameObject.SetActive(true);
            openEvent.Invoke();
        }
        public void Close()
        {
            ClearObjectsInternal();
            gameObject.SetActive(false);
            closeEvent.Invoke();
        }

        private void ClearObjectsInternal()
        {
            foreach (Transform child in modelContainer.transform)
            {
                Destroy(child.gameObject);
            }
            modelContainer.DetachChildren();
        }
    }
}