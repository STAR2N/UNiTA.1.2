using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using System.Runtime.InteropServices;
using Game;

public class LetterButton : NetworkBehaviour
{ 
    [DllImport("__Internal")]
    private static extern void RequestAddLetter_React(string email, string content);

    [SerializeField] public TMP_InputField LetterInputField;
    private Raycast MRaycast;
    public bool OnlyCanShowHost;

    private void Start()
    {
        OnlyCanShowHost = false;
        MRaycast = GameObject.FindObjectOfType<Raycast>();
    }
    private void Update()
    {
        
    }
    public void OnClickCompleteButton()
    {
        var content = LetterInputField.GetComponent<TMP_InputField>().text;
        var imageIdx = LetterMaster.Instance.RandomImage;
        LetterSecurityLevelType securityLevelType;
        if (OnlyCanShowHost)
            securityLevelType = LetterSecurityLevelType.Private;
        else
            securityLevelType = LetterSecurityLevelType.Public;
        LetterMaster.Instance.LetterCards[LetterMaster.Instance.CompleteIndex].GetComponent<Letter>().isLocalObject = true;
        
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        var email = GameNetworkManager.Instance.userEmail;
        LetterMaster.Instance.CmdSetLetter(content, imageIdx, securityLevelType, email);
        RequestAddLetter_React(email, content);
#endif

#if UNITY_EDITOR == true
        LetterMaster.Instance.CmdSetLetter(content, imageIdx, securityLevelType, imageIdx.ToString());
#endif

        LetterMaster.Instance.IsDone = true;

        ResetWriteLetterUI();
        MRaycast.IsLetterActive = false;
    }

    public void OnClickStopShowLetter()
    {
        
        GameObject CompleteLetterUI = LetterMaster.Instance.CompleteLetterUI;

        CompleteLetterUI.GetComponentInChildren<CanvasGroup>().alpha = 0.0f;
        CompleteLetterUI.GetComponentInChildren<CanvasGroup>().interactable = false;
        CompleteLetterUI.GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;

        MRaycast.IsLetterActive = false;
    }

    public void OnClickChangeToggle()
    {
        OnlyCanShowHost = !OnlyCanShowHost;
    }

    public void ResetWriteLetterUI()
    {
        LetterMaster.Instance.LetterUI.GetComponentInChildren<CanvasGroup>().alpha = 0.0f;
        LetterMaster.Instance.LetterUI.GetComponentInChildren<CanvasGroup>().interactable = false;
        LetterMaster.Instance.LetterUI.GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
        LetterInputField.GetComponent<TMP_InputField>().text = "";
        LetterMaster.Instance.LetterUI.GetComponentInChildren<Toggle>().isOn = true;

        LetterMaster.Instance.IsWriting = false;
    }
    public void OnClickDeleteButton()
    {
        ResetWriteLetterUI();

        LetterMaster.Instance.CmdBackupLetterVar(LetterMaster.Instance.RandomImage);
        MRaycast.IsLetterActive = false;
    }
}
