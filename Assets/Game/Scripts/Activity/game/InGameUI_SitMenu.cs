using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Controller.Player;
using Game.Controller;
using System.Runtime.InteropServices;

public class InGameUI_SitMenu : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void onToggleVideo_React(bool isOn);
    [DllImport("__Internal")]
    private static extern void onToggleMic_React(bool isOn);
    [DllImport("__Internal")]
    private static extern void onToggleAudio_React(bool isOn);

    [SerializeField] Button m_ChangeSitButton;
    [SerializeField] Button m_ExitButton;
    [SerializeField] Toggle m_InputAudio;
    [SerializeField] Toggle m_OutputAudio;
    [SerializeField] Toggle m_InputVideo;

    private void Awake() {
        m_InputAudio.onValueChanged.AddListener(onToggleMic);
        m_OutputAudio.onValueChanged.AddListener(onToggleAudio);
        m_InputVideo.onValueChanged.AddListener(onToggleVideo);
    }

    void Start()
    {
        m_ChangeSitButton.onClick.AddListener(onChangeSitButtonClicked);
        m_ExitButton.onClick.AddListener(onExitButtonClicked);
    }

    void onExitButtonClicked() 
    {
        SeatExitController ExitController = FindObjectOfType<SeatExitController>();
        ExitController.PerformStand();
    }

    void onChangeSitButtonClicked() 
    {
        SeatedCharacterController SeatController = FindObjectOfType<SeatedCharacterController>();
        SeatController.Spectator.MoveNext();
    }
    
    void onToggleVideo(bool isOff)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        onToggleVideo_React(!isOff);
#endif
    }

    void onToggleMic(bool isOff)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        onToggleMic_React(!isOff);
#endif
    }

    void onToggleAudio(bool isOff)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        onToggleAudio_React(!isOff);
#endif
    }
}
