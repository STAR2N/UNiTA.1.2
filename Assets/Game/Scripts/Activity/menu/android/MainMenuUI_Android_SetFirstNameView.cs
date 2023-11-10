namespace Game.UI {
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using TMPro;

    public class MainMenuUI_Android_SetFirstNameView : MonoBehaviour {
        [SerializeField] TMP_InputField m_NameInput;
        [SerializeField] Button m_AcceptButton;

        public UnityEvent OnSuccess = new UnityEvent();

        private void Awake() {
            m_AcceptButton.onClick.AddListener(() => {
                if (string.IsNullOrWhiteSpace(m_NameInput.text))
                    return;

                User.Name = m_NameInput.text;
                OnSuccess.Invoke();
            });
        }
    }
}