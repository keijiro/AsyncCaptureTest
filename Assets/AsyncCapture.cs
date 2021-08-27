using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class AsyncCapture : MonoBehaviour
{
    (RenderTexture grab, RenderTexture flip) _rt;
    NativeArray<byte> _buffer;

    System.Collections.IEnumerator Start()
    {
        var (w, h) = (Screen.width, Screen.height);

        _rt.grab = new RenderTexture(w, h, 0);
        _rt.flip = new RenderTexture(w, h, 0);

        _buffer = new NativeArray<byte>(w * h * 4, Allocator.Persistent,
                                        NativeArrayOptions.UninitializedMemory);

        var (scale, offs) = (new Vector2(1, -1), new Vector2(0, 1));

        while (true)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitForEndOfFrame();

            ScreenCapture.CaptureScreenshotIntoRenderTexture(_rt.grab);
            Graphics.Blit(_rt.grab, _rt.flip, scale, offs);

            AsyncGPUReadback.RequestIntoNativeArray
              (ref _buffer, _rt.flip, 0, OnCompleteReadback);
        }
    }

    void OnDestroy()
    {
        AsyncGPUReadback.WaitAllRequests();

        Destroy(_rt.flip);
        Destroy(_rt.grab);

        _buffer.Dispose();
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }

        var encoded = ImageConversion.EncodeNativeArrayToPNG
          (_buffer, _rt.flip.graphicsFormat,
           (uint)_rt.flip.width, (uint)_rt.flip.height);

        System.IO.File.WriteAllBytes("test.png", encoded.ToArray());
    }
}
