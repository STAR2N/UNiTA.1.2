namespace Game.UI {
    using UnityEngine;

    public class InGameUI : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] InGameUI_CodeView m_CodeView;
        [SerializeField] InGameUI_GuideView m_GuideView;
        [SerializeField] public GameObject m_SitMenu;
        [SerializeField] public GameObject m_Header;
    }
}