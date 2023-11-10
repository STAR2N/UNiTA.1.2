namespace Game.UI {
    using DG.Tweening;
    using UnityEngine;

    public class MainMenuUI_Android : MonoBehaviour {
        [Header("Views")]
        [SerializeField] RectTransform m_MainLogoObject;
        [SerializeField] RectTransform m_ServerListObject;

        [Header("Components")]
        [SerializeField] RectTransform m_LogoTransform;
        [SerializeField] MainMenuUI_Android_SetFirstNameView m_SetNameView;
        [SerializeField] MainMenuUI_ServerListView m_ServerListView;

        private void Start() {
            m_MainLogoObject.gameObject.SetActive(true);
            m_ServerListObject.gameObject.SetActive(false);

            m_SetNameView.gameObject.SetActive(false);

            var seq = DOTween.Sequence()
                .Append(m_LogoTransform.DOPunchScale(-Vector3.one * 0.1f, 1f).From())
                .AppendInterval(2)
                .AppendCallback(() => {
                    if (string.IsNullOrWhiteSpace(User.Name)) {
                        m_SetNameView.gameObject.SetActive(true);
                    } else {
                        m_SetNameView.OnSuccess.Invoke();
                    }
                })
                .Play();
        }
    }
}