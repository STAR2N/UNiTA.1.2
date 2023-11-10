using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class InGameUI_GuideView : MonoBehaviour
    {
        [Header("Panel animation")]
        bool m_HideContents = false;
        [SerializeField] Button m_TogglePanel;
        [SerializeField] RectTransform m_ContentsPanel;
        [SerializeField] AnimationParams m_Positions;
        [SerializeField] float m_Duration = 1;
        [SerializeField] Ease m_Easing = Ease.OutExpo;

        [System.Serializable]
        private class AnimationParams
        {
            [SerializeField] public RectTransform ShowPosition;
            [SerializeField] public RectTransform HidePosition;
        }

        private void Awake()
        {
            m_TogglePanel.onClick.AddListener(OnPressToggle);
            HideContents(true);
        }

        public void OnPressToggle()
        {
            if (m_HideContents)
            {
                HideContents();
            } else
            {
                ShowContents();
            }
        }

        public void HideContents(bool noAnimation = false) {
            m_HideContents = false;

            m_ContentsPanel.DOKill();
            m_ContentsPanel.DOMove(m_Positions.HidePosition.position, m_Duration).SetEase(m_Easing).Play();

            if (noAnimation) {
                m_ContentsPanel.DOKill(true);
            }
        }
        public void ShowContents(bool noAnimation = false) {
            m_HideContents = true;

            m_ContentsPanel.DOKill();
            m_ContentsPanel.DOMove(m_Positions.ShowPosition.position, m_Duration).SetEase(m_Easing).Play();

            if (noAnimation) {
                m_ContentsPanel.DOKill(true);
            }
        }
    }
}