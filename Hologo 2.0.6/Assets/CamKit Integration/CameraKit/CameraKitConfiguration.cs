using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraKitRenderMode {
    Fullscreen = 0,
    BehindUnity = 1
}

public enum CameraKitShutterButtonMode {
    Off = 0,
    On = 1,
    OnlyOnFrontCamera = 2
}

public enum CameraKitDevice {
    FrontCamera = 0,
    BackCamera = 1
}
public class CameraKitConfiguration 
{
    public string LensID;
    public string LensGroupID;
    public string RemoteAPISpecId;
    public CameraKitShutterButtonMode ShutterButtonMode = CameraKitShutterButtonMode.On;
    public CameraKitRenderMode RenderMode = CameraKitRenderMode.BehindUnity;
    public CameraKitDevice StartWithCamera = CameraKitDevice.FrontCamera;
    public Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();    
    // This will force the lens to be unloaded after CameraKit is dismissed. Useful for flushing internal state of the lenses and force a reinitialization
    public bool UnloadLensAfterDismiss = false;
}
