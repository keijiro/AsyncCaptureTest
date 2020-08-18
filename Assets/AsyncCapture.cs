using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Collections;

public class AsyncCapture : MonoBehaviour
{
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitForEndOfFrame();

            var rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);
            AsyncGPUReadback.Request(rt, 0, TextureFormat.ARGB32, OnCompleteReadback);
            RenderTexture.ReleaseTemporary(rt);
        }
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }

        var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        tex.LoadRawTextureData(request.GetData<uint>());
        tex.Apply();
        File.WriteAllBytes("test.png", ImageConversion.EncodeToPNG(tex));
        Destroy(tex);
    }
}
