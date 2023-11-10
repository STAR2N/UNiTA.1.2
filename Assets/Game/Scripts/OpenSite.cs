using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSite : MonoBehaviour
{
    public string url;

    public void OpenWebsite()
    {
        Application.OpenURL("https://cogo.co.kr/");
    }

}
