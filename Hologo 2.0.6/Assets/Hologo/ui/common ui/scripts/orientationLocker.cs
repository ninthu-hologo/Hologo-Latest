using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;

public class orientationLocker : MonoBehaviour
{
    [SerializeField]
    private settings_SO mainSettings;
    [Tooltip("1 = portrait, 2 = landscapeLeft, 3= landscape right , >3 auto rotation")]
    [Range(0,3)]
    public int screenOrientationLockMobile =1;
    [Tooltip("1 = portrait, 2 = landscapeLeft, 3= landscape right , >3 auto rotation")]
    [Range(0, 3)]
    public int screenOrientationLockTablet =1;
    [SerializeField]
    private bool setOrientationFromAwake = true;

    // 0 = portrait, 1 = landscapeLeft, 2= landscape right , >2 auto rotation

    public void setOrientation(deviceType dType)
    {
        updateOrientation(dType);
    }

     //Shariz 15th September 2019 v2  

     // edited by hamid 15 sep 2019 v2 from start to awake
    void Awake()
    {
        if (!setOrientationFromAwake)
            return;

        updateOrientation(mainSettings.device);
    }

    private void updateOrientation(deviceType dType)
    {
        if (dType == deviceType.TABLET)
        {
            if (screenOrientationLockTablet == 0)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            else if (screenOrientationLockTablet == 1)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            else if (screenOrientationLockTablet == 2)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
            }
            else
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
        }
        else
        {
            if (screenOrientationLockMobile == 0)
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
            else if (screenOrientationLockMobile == 1)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            else if (screenOrientationLockMobile == 2)
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
            }
            else
            {
                Screen.orientation = ScreenOrientation.AutoRotation;
            }
        }
    }
}
