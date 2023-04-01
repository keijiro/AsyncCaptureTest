using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using OperationCanceledException = System.OperationCanceledException;

public sealed class AsyncCapture : MonoBehaviour
{
    async void Start()
    {
        var (w, h) = (Screen.width, Screen.height);
        var (scale, offs) = (new Vector2(1, -1), new Vector2(0, 1));

        var grabRT = new RenderTexture(w, h, 0);
        var flipRT = new RenderTexture(w, h, 0);

        var buffer = new NativeArray<byte>(w * h * 4, Allocator.Persistent,
                                           NativeArrayOptions.UninitializedMemory);

        try
        {
            for (var cancel = destroyCancellationToken;;)
            {
                await Awaitable.WaitForSecondsAsync(1, cancel);
                await Awaitable.EndOfFrameAsync(cancel);

                ScreenCapture.CaptureScreenshotIntoRenderTexture(grabRT);
                Graphics.Blit(grabRT, flipRT, scale, offs);

                var req = await AsyncGPUReadback.RequestIntoNativeArrayAsync(ref buffer, flipRT, 0);

                if (req.hasError)
                {
                    Debug.Log("GPU readback error detected.");
                    continue;
                }

                using var encoded = ImageConversion.
                  EncodeNativeArrayToPNG(buffer, flipRT.graphicsFormat, (uint)w, (uint)h);

                System.IO.File.WriteAllBytes("test.png", encoded.ToArray());
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            if (flipRT != null) Destroy(flipRT);
            if (grabRT != null) Destroy(grabRT);
            if (buffer.IsCreated) buffer.Dispose();
        }
    }
}
