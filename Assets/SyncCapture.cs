using UnityEngine;
using System.IO;

public class SyncCapture : MonoBehaviour
{
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var tempRT = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, tempRT);

        var tempTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        tempTex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
        tempTex.Apply();

        if (Time.frameCount % 60 == 0)
            File.WriteAllBytes("test.png", ImageConversion.EncodeToPNG(tempTex));

        Destroy(tempTex);
        RenderTexture.ReleaseTemporary(tempRT);

        Graphics.Blit(source, destination);
    }
}

