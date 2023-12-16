using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTextureForAr : MonoBehaviour
{
    public RenderTexture ScreenShotAr;
    public Camera TextureCamera;
    public RawImage TextureShot;

    Resolution currentResolution;

    // Start is called before the first frame update
    public void TakeScreenShot()
    {
        currentResolution = Screen.currentResolution;
        ScreenShotAr = new RenderTexture(currentResolution.width, currentResolution.height, 16, RenderTextureFormat.ARGB32);
        ScreenShotAr.Create();
        TextureCamera.gameObject.SetActive(true);
        TextureCamera.targetTexture = ScreenShotAr;
        TextureCamera.Render();
        TextureCamera.targetTexture = null;
        TextureCamera.gameObject.SetActive(false);
        //TextureShot.gameObject.SetActive(true);
        TextureShot.texture = ScreenShotAr;

    }

    private void OnDestroy()
    {
        if (ScreenShotAr != null)
            ScreenShotAr.Release();
    }
}
