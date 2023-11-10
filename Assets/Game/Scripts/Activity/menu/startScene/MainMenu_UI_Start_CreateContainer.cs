using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI_Start_CreateContainer : MonoBehaviour
{
    [SerializeField] Button m_CreateRoomButton;
    [SerializeField] GameObject m_MainView;
    [SerializeField] GameObject m_StartView;

    private void Awake()
    {
        m_CreateRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
    }
  
  
    public void OnCreateRoomButtonClicked() 
    { 
        // m_StartView.gameObject.SetActive(false);
        m_MainView.gameObject.SetActive(true);
    }
}
