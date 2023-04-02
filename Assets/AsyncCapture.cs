using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using OperationCanceledException = System.OperationCanceledException;
using File = System.IO.File;

public sealed class AsyncCapture : MonoBehaviour
{
    [SerializeField] string _outputFileName = "Screenshot.png";
    [SerializeField] float _interval = 1;

    async void Start()
    {
        Application.targetFrameRate = 60;

        var (w, h) = (Screen.width, Screen.height);
        var (scale, offs) = (new Vector2(1, -1), new Vector2(0, 1));

        var grabRT = new RenderTexture(w, h, 0);
        var flipRT = new RenderTexture(w, h, 0);
        var rtFormat = flipRT.graphicsFormat;

        var buffer = new NativeArray<byte>(w * h * 4, Allocator.Persistent,
                                           NativeArrayOptions.UninitializedMemory);

        try
        {
            for (var cancel = destroyCancellationToken;;)
            {
                await Awaitable.WaitForSecondsAsync(_interval, cancel);
                await Awaitable.EndOfFrameAsync(cancel);

                ScreenCapture.CaptureScreenshotIntoRenderTexture(grabRT);
                Graphics.Blit(grabRT, flipRT, scale, offs);

                var req = await AsyncGPUReadback.RequestIntoNativeArrayAsync(ref buffer, flipRT, 0);

                if (req.hasError)
                {
                    Debug.Log("GPU readback error detected.");
                    continue;
                }

                await Awaitable.BackgroundThreadAsync();

                using var encoded = ImageConversion.
                  EncodeNativeArrayToPNG(buffer, rtFormat, (uint)w, (uint)h);

                await Awaitable.MainThreadAsync();

                await File.WriteAllBytesAsync(_outputFileName, encoded.ToArray(), cancel);
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
