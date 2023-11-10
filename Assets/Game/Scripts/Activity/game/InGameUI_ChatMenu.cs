using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI_ChatMenu : MonoBehaviour
{
    [SerializeField] Button m_ShowChatButton;
    [SerializeField] Button m_HideChatButton;
    [SerializeField] CanvasGroup m_ChatWindowCanvas;
    [SerializeField] GameObject m_Dot;

    bool isVisibleChatWindow = false;

    void Start()
    {
        m_ShowChatButton.onClick.AddListener(ShowChat);
        m_HideChatButton.onClick.AddListener(HideChat);
    }

    void ShowChat() 
    {
        m_ShowChatButton.gameObject.SetActive(false);
        m_ChatWindowCanvas.alpha = 1f;
        m_ChatWindowCanvas.interactable = true;
        m_ChatWindowCanvas.blocksRaycasts = true;
        HideDot();
        isVisibleChatWindow = true;
    }

    void HideChat() 
    {
        m_ShowChatButton.gameObject.SetActive(true);
        m_ChatWindowCanvas.alpha = 0f;
        m_ChatWindowCanvas.interactable = false;
        m_ChatWindowCanvas.blocksRaycasts = false;
        isVisibleChatWindow = false;
    }

    public void UpdateDotVisible() 
    {
        if(!isVisibleChatWindow) {
            ShowDot();
        }
    }

    void ShowDot()
    {
        m_Dot.SetActive(true);
    }

    void HideDot()
    {
        m_Dot.SetActive(false);
    }
}
