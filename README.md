AsyncCaptureTest
================

This is an attempt at implementing fast screen capture with using the
asynchronous GPU readback API.

About *AsyncGPUReadback*
------------------------

*AsyncGPUReadback* is one of the new experimental features of Unity 2018.1. It
allows retrieving GPU data (textures and compute buffers) without introducing
render pipeline stall due to synchronization.

https://docs.unity3d.com/2018.1/Documentation/ScriptReference/Experimental.Rendering.AsyncGPUReadbackRequest.html

It's useful when GPU readback is required but small amounts of latency is
acceptable.

Note that the *AsyncGPUReadback* feature is still in its experimental phase and
only supported on DX11/12 at them moment. Further platform support will be
added in later releases.

<!--4567890123456789012345678901234567890123456789012345678901234567890123456-->
