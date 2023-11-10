namespace Game.UI {
    using MessageSystem;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class ButtonEvent : MonoBehaviour {
        [Tooltip("If true, regist callback on the onClick event in the same object.")]
        [SerializeField] bool m_RegistOnEnable = false;
        [SerializeField, FormerlySerializedAs("ButtonName")] string m_ButtonName = string.Empty;

        #region Auto regist
        private Button m_RegistedButton = null;
        private void OnEnable() {
            if (m_RegistOnEnable) {
                m_RegistedButton = GetComponent<Button>();
                if (m_RegistedButton != null) {
                    m_RegistedButton.onClick.AddListener(OnPressed);
                }
            }
        }
        private void OnDisable() {
            if (m_RegistedButton != null) {
                m_RegistedButton.onClick.RemoveListener(OnPressed);
                m_RegistedButton = null;
            }
        }
        #endregion

        public void OnPressed() {
            if (string.IsNullOrEmpty(m_ButtonName)) {
                Debug.LogError($"Button name cannot be null or empty.", this);
                return;
            }
            ButtonMessage.Invoke(new EventArgs(m_ButtonName));
        }

        #region Event class
        public class EventArgs : System.EventArgs {
            public string Button;

            public EventArgs(string name) {
                Button = name;
            }
        }
        public class ButtonMessage : GenericMessage<EventArgs> { }
        #endregion

        #region Listener manager
        public static EventListener Regist(string name, System.Action callback) {
            return new EventListener(name, callback);
        }

        public class EventListener : System.IDisposable {
            public readonly string ButtonName;
            public readonly System.Action Callback;
            private readonly Object m_DebugObject;

            internal EventListener(string button, System.Action onClick, Object debugObject = null) {
                if (string.IsNullOrEmpty(button))
                    throw new System.Exception("Button name cannot be null.");

                ButtonName = button;
                Callback = onClick;
                m_DebugObject = debugObject;
                ButtonMessage.OnMessage += OnPressed;
            }

            private void OnPressed(EventArgs args) {
                if (args.Button != ButtonName)
                    return;

                try {
                    Callback();
                } catch (System.Exception e) {
                    if (m_DebugObject == null)
                        Debug.LogError(e);
                    else
                        Debug.LogError(e, m_DebugObject);
                }
            }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing) {
                if (!disposedValue) {
                    if (disposing) {
                        // TODO: dispose managed state (managed objects)
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    ButtonMessage.OnMessage -= OnPressed;
                    disposedValue = true;
                }
            }

            // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            ~EventListener()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: false);
            }

            public void Dispose() {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                System.GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}