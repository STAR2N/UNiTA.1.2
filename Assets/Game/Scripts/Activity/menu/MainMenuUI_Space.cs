using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI_Space : MonoBehaviour
{
    [SerializeField] Image m_spaceImage;
    [SerializeField] TextMeshProUGUI m_spaceName;
    [SerializeField] public Button m_spaceButton;

    private string m_name;

    private void Awake()
    {
        m_spaceButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick() 
    {
        GameNetworkManager.SelectedLevel = GameNetworkManager.Instance.Levels.Levels[m_name].Scene;
        GameObject.Find("spacepopup").SetActive(false);
        GameObject.Find("UI Space Text").GetComponent<TextMeshProUGUI>().text = m_name;
    }

    public void SetName(string name) 
    {
        m_name = name;
        m_spaceName.text = name;
    }

    public void SetImage(Sprite sprite) 
    {
        m_spaceImage.sprite = sprite;
    }
}
