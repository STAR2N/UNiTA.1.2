using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShootRenderTexture : MonoBehaviour
{
    [SerializeField]
    public RenderTexture DrawTexture;   //PNG저장할 타겟 렌더 텍스쳐

    void RenderTextureSave()
    {
        RenderTexture.active = DrawTexture;
        var texture2D = new Texture2D(DrawTexture.width, DrawTexture.height, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, DrawTexture.width, DrawTexture.height), 0, 0);
        texture2D.Apply();
        var data = texture2D.EncodeToPNG();
        File.WriteAllBytes("D:/Download/MiniMapImg.png", data);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RenderTextureSave();
        }
    }
}
