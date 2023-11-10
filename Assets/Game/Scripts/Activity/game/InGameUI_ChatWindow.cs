using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using Game.Network;
using Game.Player;
using UnityEngine.UI;

public class InGameUI_ChatWindow : MonoBehaviour
{
    public static InGameUI_ChatWindow Instance { get; private set; }

    [SerializeField] TMP_InputField m_chatInput;
    [SerializeField] GameObject m_Content;
    [SerializeField] GameObject m_ChatPreFab;
    [SerializeField] ScrollRect scroll_rect;
    [SerializeField] Button m_UpButton;
    [SerializeField] Button m_DownButton;
    [SerializeField] InGameUI_ChatMenu m_ChatMenu; 

    public bool isSelected = false;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
    }
    private void OnDisable()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Awake()
    {
        m_chatInput.onSubmit.AddListener(EmitChat);
        m_UpButton.onClick.AddListener(ScrollUp);
        m_DownButton.onClick.AddListener(ScrollDown);
        m_chatInput.onSelect.AddListener((string arg) => {
            isSelected = true;
        });

        m_chatInput.onDeselect.AddListener((string arg) => {
            isSelected = false;
        });
    }

    public void EmitChat(string message)
    {
        m_chatInput.ActivateInputField();

        if(string.IsNullOrWhiteSpace(message)) {
            return;
        }

        m_chatInput.text = "";
        string name = User.Name;

        ChatManager chatManager = null;
        if(NetworkClient.connection.identity.GetComponent<WalkingCharacter>()) {
            chatManager = NetworkClient.connection.identity.GetComponent<WalkingCharacter>().GetComponent<ChatManager>();
        } else if (NetworkClient.connection.identity.GetComponent<SeatCharacter>()) {
            chatManager = NetworkClient.connection.identity.GetComponent<SeatCharacter>().GetComponent<ChatManager>();
        }

        int characterIdx = User.VFX;

        if(chatManager) {
            chatManager.CmdEmitChat(name, message, characterIdx);
        }
    }

    public void OnPlayerMessage(string name, string message, int characterIdx) 
    {
        string prettyMessage = $"<color=white><size=11>{name}: {message}</size></color>";
        AppendMessage(prettyMessage, characterIdx);
    }

    void AppendMessage(string message, int characterIdx) 
    {
        var chat = Instantiate(m_ChatPreFab);
        InGameUI_Chat ui_chat = chat.GetComponent<InGameUI_Chat>();
        ui_chat.text = message;
        ui_chat.characterIdx = characterIdx;

        chat.transform.SetParent(m_Content.transform, false);

        ScrollDown_Delay();
        m_ChatMenu.UpdateDotVisible();
    }

    void ScrollUp() 
    {
        scroll_rect.verticalNormalizedPosition = 1.0f;
    }

    void ScrollDown_Delay() 
    {
        Invoke("ScrollDown", 0.1f);
    }

     void ScrollDown() 
    {
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
