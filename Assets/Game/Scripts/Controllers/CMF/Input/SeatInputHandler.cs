namespace Game.CMF.Input {
    using global::CMF;
    using UnityEngine;

    public class SeatInputHandler : MonoBehaviour {
        [Header("Keys")]
        public KeyCode PreviousViewKey = KeyCode.A;
        public KeyCode NextViewKey = KeyCode.D;
        [SerializeField] string m_ProxyName = "Movement joystick";

        JoystickInputProxy m_Proxy = null;
        public bool JoystickLeft { get; private set; } = false;
        public bool JoystickLeftDown { get; private set; } = false;
        public bool JoystickRight { get; private set; } = false;
        public bool JoystickRightDown { get; private set; } = false;

        private void Awake() {
            JoystickInputProxy.Proxies.TryGetValue(m_ProxyName, out m_Proxy);
        }
        private void Update() {
            if (m_Proxy != null) {
                HandleJoystic(m_Proxy.Input.Horizontal);
            }
        }
        private void HandleJoystic(float horizontal) {
            (bool, bool) Calc(bool prev, float input, float releaseDeadzone = 0.8f, float pressDeadzone = 0.9f) {
                if (prev) {
                    if (input < releaseDeadzone) {
                        return (false, false);
                    }
                } else {
                    if (input > pressDeadzone) {
                        return (true, true);
                    }
                }

                return (prev, false);
            }

            (JoystickLeft, JoystickLeftDown) = Calc(JoystickLeft, horizontal);
            (JoystickRight, JoystickRightDown) = Calc(JoystickRight, -horizontal);
        }

        public int GetChangeSeatInput() {
            return (
                -((Input.GetKeyDown(PreviousViewKey) || JoystickLeftDown ) ? 1 : 0)
                +((Input.GetKeyDown(NextViewKey)     || JoystickRightDown) ? 1 : 0)
                );
        }
    }
}