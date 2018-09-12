using UnityEngine;
using Unity.Collections;
using System.IO;
using System.Collections.Generic;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Rendering;
#else
using UnityEngine.Experimental.Rendering;
#endif

public class AsyncCapture : MonoBehaviour
{
    Queue<AsyncGPUReadbackRequest> _requests = new Queue<AsyncGPUReadbackRequest>();

    void Update()
    {
        while (_requests.Count > 0)
        {
            var req = _requests.Peek();

            if (req.hasError)
            {
                Debug.Log("GPU readback error detected.");
                _requests.Dequeue();
            }
            else if (req.done)
            {
                var buffer = req.GetData<Color32>();

                if (Time.frameCount % 60 == 0)
                {
                    var camera = GetComponent<Camera>();
                    SaveBitmap(buffer, camera.pixelWidth, camera.pixelHeight);
                }

                _requests.Dequeue();
            }
            else
            {
                break;
            }
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_requests.Count < 8)
            _requests.Enqueue(AsyncGPUReadback.Request(source));
        else
            Debug.Log("Too many requests.");

        Graphics.Blit(source, destination);
    }

    void SaveBitmap(NativeArray<Color32> buffer, int width, int height)
    {
        var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.SetPixels32(buffer.ToArray());
        tex.Apply();
        File.WriteAllBytes("test.png", ImageConversion.EncodeToPNG(tex));
        Destroy(tex);
    }
}
