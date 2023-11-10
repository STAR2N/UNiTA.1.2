using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI_Chat : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] TMP_Text m_Text;
    
    public string text
    {
        set
        {
            m_Text.text = value;
        }
    }

    public int characterIdx
    {
        set
        {
            Sprite image = GameNetworkManager.Instance.CharacterImages.ImageList[value];
            m_Image.sprite = image;
        }
    }
}
