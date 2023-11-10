namespace Game.Input {
    using Game.CMF;
    using UnityEngine;

    public class CameraZoom : MonoBehaviour {
        [Header("Components")]
        //[SerializeField] CameraController Controller;

        [Header("Option")]
        public float ZoomRatio = 0.5f; // x2
        public float ZoomSpeed = 120f;
        public bool Toggle = true;

        [Header("Key")]
        public KeyCode ZoomKey = KeyCode.Mouse1;

        private bool m_ToggleState = false;
        public float DesireFov { get; private set; } = 90f;
        [HideInInspector] public float NormalFov = 90f;

        private void Awake() {
            //NormalFov = Camera.VerticalToHorizontalFieldOfView(Controller.Camera.fieldOfView, Controller.Camera.aspect);
            DesireFov = NormalFov;
        }
        private void Update() {
            HandleInput();
            //UpdateFov(DesireFov);
        }
        
        private void HandleInput() {
            if (Toggle) {
                if (Input.GetKeyDown(ZoomKey)) {
                    DesireFov = (m_ToggleState) ? NormalFov * ZoomRatio : NormalFov;
                    m_ToggleState = !m_ToggleState;
                }
            } else {
                DesireFov = (Input.GetKeyDown(ZoomKey)) ? NormalFov * ZoomRatio : NormalFov;
            }
        }

        //private void UpdateFov(float desireFov) {
            //var vFov = Mathf.MoveTowards(Camera.VerticalToHorizontalFieldOfView(Controller.Camera.fieldOfView, Controller.Camera.aspect), desireFov, Time.deltaTime * ZoomSpeed);
            //Controller.SetFOV(
                //Camera.HorizontalToVerticalFieldOfView(vFov, Controller.Camera.aspect));
        }
    }