namespace Game.Utility.Extensions {
    using UnityEngine;

    public static class TextureConversionExtension {
        public static Texture2D ToTexture2D(this RenderTexture rt, TextureFormat format = TextureFormat.RGBA32) {
            Texture2D ret = new Texture2D(rt.width, rt.height, format , false);

            RenderTexture.active = rt;
            ret.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            RenderTexture.active = null;

            ret.Apply();

            return ret;
        }
        public static Texture2D ToTexture2D(this WebCamTexture wt, TextureFormat format = TextureFormat.RGBA32) {
            var ret = new Texture2D(wt.width, wt.height, format, false);

            ret.SetPixels32(wt.GetPixels32());
            ret.Apply();

            return ret;
        }

        public static Texture2D Resize(this Texture2D tex, Vector2Int size) {
            var rt = RenderTexture.GetTemporary(size.x, size.y);
            Graphics.Blit(tex, rt);

            var ret = new Texture2D(size.x, size.y, tex.format, false);

            RenderTexture.active = rt;
            ret.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            RenderTexture.active = null;

            ret.Apply();

            RenderTexture.ReleaseTemporary(rt);

            return ret;
        }
    }
}