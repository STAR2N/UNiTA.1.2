using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Game;

public class Letter : NetworkBehaviour
{
    public bool isLocalObject = false;
    private void Start()
    {
        LetterMaster.Instance.LetterCards.Add(gameObject);
    }

    [SyncVar(hook = nameof(Hook_Email)), HideInInspector] public string Email;
    void Hook_Email(string _, string @new)
    {
        if(GameNetworkManager.Instance.userEmail == @new) {
            ShowVisual();
            LetterMaster.Instance.IsDone = true;
        }
    }
    [SyncVar(hook = nameof(Hook_Content)), HideInInspector] public string Content;
    void Hook_Content(string _, string @new)
    {
    }
    [SyncVar(hook = nameof(Hook_ImgIdx)), HideInInspector] public int ImgIdx;
    void Hook_ImgIdx(int _, int @new)
    {
        GetComponent<SpriteRenderer>().sprite = LetterMaster.Instance.LetterBack_BgImg[@new];
    }

    [SyncVar(hook = nameof(Hook_SecurityLevel)), HideInInspector] public LetterSecurityLevelType SecurityLevel;
    void Hook_SecurityLevel(LetterSecurityLevelType _, LetterSecurityLevelType @new)
    {
        if(isLocalObject || isServer || @new == LetterSecurityLevelType.Public)
        {
            ShowVisual();
        }
    }

    public void ShowVisual()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
public enum LetterSecurityLevelType
{
    None,
    Public,
    Private
}