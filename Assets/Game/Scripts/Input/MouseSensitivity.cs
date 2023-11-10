namespace Game.Input {
    using global::CMF;
    using UnityEngine;

    public class MouseSensitivity : MonoBehaviour {
        const string SENSITIVITY_KEY = "mouse_sensitivity";

        float m_Sensitivity = 0.01f;
        public float Sensitivity {
            get => m_Sensitivity;
            private set {
                m_Sensitivity = value;
                if (CameraInput != null)
                    CameraInput.mouseInputMultiplier = value;
                PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
            }
        }
        [SerializeField] CameraMouseInput m_CameraInput;
        public CameraMouseInput CameraInput {
            get => m_CameraInput; set {
                m_CameraInput = value;
                LoadSensitivity();
            }
        }

        [Header("Change")]
        public KeyCode RaiseKey = KeyCode.LeftBracket;
        public KeyCode LowerKey = KeyCode.RightBracket;
        public float MaxSensitivity = 0.02f;
        public float ChangeAmount = 0.00125f;

        private void Awake() {
            if (m_CameraInput != null)
                LoadSensitivity();
        }

        private void OnDestroy() {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void Update() {
            UpdateMouseSensitivity();
        }

        void LoadSensitivity() {
            m_Sensitivity = CameraInput.mouseInputMultiplier;

            if (PlayerPrefs.HasKey(SENSITIVITY_KEY)) {
                Sensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY);
            }
        }

        void UpdateMouseSensitivity() {
            if (CameraInput == null)
                return;

            var change = (Input.GetKeyDown(RaiseKey) ? 1f : 0f) - (Input.GetKeyDown(LowerKey) ? 1f : 0f);
            if (change != 0) {
                change *= ChangeAmount;
                Sensitivity = Mathf.Clamp(Sensitivity + change, 0, MaxSensitivity);
            }
        }
    }
}