AsyncCaptureTest
================

![screenshot](https://user-images.githubusercontent.com/343936/131082461-5e9579cb-9aba-48f9-8017-52a2e80e016c.png)

This is an example that shows how to use the [asynchronous GPU readback API]
to capture renders without blocking the main thread.

[asynchronous GPU readback API]:
    https://docs.unity3d.com/ScriptReference/Rendering.AsyncGPUReadback.html

Note that there is a trade-off between performance and latency -- it's only
useful when a small amount of latency is acceptable. Screen capture is one of
the best-fit case for the feature.

### 2023 Update

I updated this sample project to use the Awaitable class that is introduced in
Unity 2023.1. Now you can use the GPU readback API using the C# asynchronous
programming features (async/await).
