namespace Game.CMF.Input {
    using System.Collections.Generic;
    using UnityEngine;

    public class JoystickInputProxy : MonoBehaviour {
        public static readonly Dictionary<string, JoystickInputProxy> Proxies = new Dictionary<string, JoystickInputProxy>();

        [SerializeField] string m_ProxyName;
        public Joystick Input;

        private void Awake() {
            if (!Proxies.ContainsKey(m_ProxyName))
                Proxies[m_ProxyName] = this;
        }

        private void OnDestroy() {
            if (Proxies.TryGetValue(m_ProxyName, out var value) && value == this) {
                Proxies.Remove(m_ProxyName);
            }
        }
    }
}