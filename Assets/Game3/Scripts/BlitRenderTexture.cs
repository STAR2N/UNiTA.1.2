using UnityEngine;

public class BlitRenderTexture : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Texture texture;

    void Awake()
    {
        Graphics.Blit(texture, renderTexture);
    }
}
