AsyncCaptureTest
================

This is an example that shows how to use the asynchronous GPU readback feature
to capture renders without blocking the main thread.

Asynchronous GPU readback is one of the newly introduced features in Unity
2018.1 (it was labeled as "experimental" at that point, then became official in
2018.3). It allows retrieving GPU data (textures, compute buffers, etc.)
without introducing hard stalls due to render pipeline synchronization.

https://docs.unity3d.com/2018.3/Documentation/ScriptReference/Rendering.AsyncGPUReadbackRequest.html

Note that there is a trade-off between performance and latency -- it's only
useful when a small amount of latency is acceptable. Screen capture is one of
the best-fit case for the feature.
