namespace Game.CMF.Input {
    using global::CMF;
    using UnityEngine;

    public class CameraInputHandler : CameraInput {
        public CameraInput DefaultInput;

        public bool InvertVertical = false;
        public bool InvertVerticalOnJoystick = true;
        public bool FindJoystickOnUpdate = false;
        [SerializeField] string m_ProxyName;
        JoystickInputProxy m_Proxy = null;

        private void Start() {
            UpdateJoystick();
        }

        private void Update() {
            if (FindJoystickOnUpdate) {
                UpdateJoystick();
            }
        }

        public override float GetHorizontalCameraInput() {
            var input = DefaultInput.GetHorizontalCameraInput();
            if (m_Proxy != null) {
                input = m_Proxy.Input.Horizontal;
            }
            return input;
        }

        public override float GetVerticalCameraInput() {
            var input = DefaultInput.GetVerticalCameraInput();
            if (m_Proxy != null) {
                input = m_Proxy.Input.Vertical;
            }

            if (InvertVertical || (InvertVerticalOnJoystick && m_Proxy != null)) {
                return -input;
            }
            return input;
        }

        public void UpdateJoystick() {
            if (JoystickInputProxy.Proxies.TryGetValue(m_ProxyName, out var proxy)) {
                m_Proxy = proxy;
            }
        }
    }
}
