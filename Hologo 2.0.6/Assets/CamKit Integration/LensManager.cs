using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LensManager : MonoBehaviour
{
    // [SerializeField]
    // public AnimationManager animationManager;


    // [SerializeField]
    // private GameObject _closeButton;
    // [SerializeField]
    // private Image _lens1;
    // [SerializeField]
    // private Image _lens2;
    // [SerializeField]
    // private Image _lens3;



    private string _activeLensId;
    void OnEnable()
    {
        // --- Listening to response callback from CameraKit's Remote APIs  ---
        CameraKitHandler.OnResponseFromLensEvent += OnLensDataSentToUnity;
        CameraKitHandler.OnCaptureFinished += OnCameraKitCaptured;
        CameraKitHandler.OnCameraDismissed += OnCameraKitDismissed;
        CameraKitHandler.OnLensRequestedUpdatedState += OnLensRequestedState;
    }

    void OnDisable()
    {
        CameraKitHandler.OnResponseFromLensEvent -= OnLensDataSentToUnity;
        CameraKitHandler.OnCaptureFinished -= OnCameraKitCaptured;
        CameraKitHandler.OnCameraDismissed -= OnCameraKitDismissed;
        CameraKitHandler.OnLensRequestedUpdatedState -= OnLensRequestedState;
    }


    private void OnLensDataSentToUnity(SerializedResponseFromLens responseObj)
    {
        // --- Obtaining a response from CameraKit ---
        // In order to receive data from the Lens to the Unity project, your Lens needs to use Remote APIs
        // More info: https://docs.snap.com/camera-kit/guides/tutorials/communicating-between-lenses-and-app#lens-studio-best-practices-for-remote-apis  
        // if (_activeLensId == Constants.LENS_ID_COLLECT_COINS) {
        //     // For the purposes of this demo, the only time we're responding to 
        //     // Lens events is when the coin collection lens is active
        //     CollectCoinsHandleEnvet(responseObj);
        // }
    }

    private void InvokeCameraKit_onSelectedLens(string lensID, string lensGroupID)
    {
        var config = new CameraKitConfiguration()
        {
            LensID = lensID,
            LensGroupID = lensGroupID,
            RemoteAPISpecId = Constants.API_SPEC_ID,
            RenderMode = CameraKitRenderMode.Fullscreen,
            StartWithCamera = CameraKitDevice.FrontCamera,
            ShutterButtonMode = CameraKitShutterButtonMode.Off,
            UnloadLensAfterDismiss = true
        };
        CameraKitHandler.InvokeCameraKit(config);
    }



    private void DismissCameraKit()
    {
        CameraKitHandler.DismissCameraKit();
    }

    private void OnCameraKitCaptured(string capturedFileUri)
    {
        // This handles the event of when media is captured in Camera Kit
        // animationManager.PlayScene("MediaCaptured");
        // _activeLensId = null;
    }

    private void OnCameraKitDismissed()
    {
        // This is triggered every time Camera Kit is dismissed
        _activeLensId = null;
    }

    private void OnLensRequestedState()
    {
        // This is triggered every time Camera Kit requests an updated state

        // (no-op. leave request open until we're ready to update state)
    }



    #region Button Handlers
    public void onLensPressed(string lensInfo)
    {

        //lensID and lensGroupID will be added to the same string 'lensInfo' with a comma in between them
        string lensID= lensInfo.Split(',')[0];
        string lensGroupID = lensInfo.Split(',')[1];

        InvokeCameraKit_onSelectedLens(lensID, lensGroupID);

        _activeLensId = lensID;
    }
    private void OnCloseButtonSelected()
    {
        CameraKitHandler.DismissCameraKit();
    }

    #endregion
}