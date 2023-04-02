AsyncCaptureTest
================

![screenshot](https://user-images.githubusercontent.com/343936/131082461-5e9579cb-9aba-48f9-8017-52a2e80e016c.png)

This Unity sample project shows how to use the [asynchronous GPU readback API]
to capture render outputs without blocking the main thread.

[asynchronous GPU readback API]:
    https://docs.unity3d.com/ScriptReference/Rendering.AsyncGPUReadback.html

Note that there is a trade-off between performance and latency. It's only
applicable when a small amount of latency is acceptable. Screen capture is one
of the best-fit cases for the feature.

Unity 2023 Update
-----------------

I updated this project to use the [Awaitable] class introduced in Unity 2023.1.
Now you can use the GPU readback API with the C# asynchronous programming
features (async/await).

[Awaitable]:
  https://docs.unity3d.com/2023.1/Documentation/ScriptReference/Awaitable.html
