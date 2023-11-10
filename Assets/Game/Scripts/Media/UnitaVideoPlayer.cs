using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Runtime.InteropServices;

public class UnitaVideoPlayer : MonoBehaviour
{
    VideoPlayer vp;

    [DllImport("__Internal")]
    private static extern void RequestPlayVideo_React(string objectName);

    [SerializeField] string m_VideoName;

    void Start()
    {
#if UNITY_EDITOR == true
        PlayVideo();
#endif
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        RequestPlayVideo_React(gameObject.name);
#endif
    }

    void comm_PlayVideo()
    {
        Invoke("PlayVideo", 1);
    }

    void PlayVideo()
    {
        vp = GetComponent<VideoPlayer>();
        if (vp == null) return;

        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, m_VideoName);
        vp.Play();
    }
}
