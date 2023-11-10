using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChatManager : NetworkBehaviour
{
    public static event Action<string, string> OnMessage;

    [Command]
    public void CmdEmitChat(string name, string message, int characterIdx)
    {
        RpcOnEmitChat(name, message, characterIdx);
    }

    [ClientRpc]
    public void RpcOnEmitChat(string name, string message, int characterIdx)
    {
        InGameUI_ChatWindow chatWindow = GameObject.Find("chatwindow").GetComponent<InGameUI_ChatWindow>();
        chatWindow.OnPlayerMessage(name, message, characterIdx);
    }
}


