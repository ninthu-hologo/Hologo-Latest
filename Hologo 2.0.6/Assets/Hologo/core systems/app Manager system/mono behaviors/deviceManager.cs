using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 24 August 2019
    /// Modified by: 
    /// this behavior gets device information . should be run only once
    /// </summary>
    [System.Serializable]
    public class deviceManager : MonoBehaviour
    {

        [Header("DATA")]
        [SerializeField]
        private settings_SO mainSettings;

        private Vector2 ReferenceResolution = new Vector2(375, 667);
        public float tabletAspectRatio = 1.3f;
        public float androidTabletRatio = 1.6f;
        public float phoneAspectRatio = 1.7f;
        public float IphoneXAspectRation = 2.0f; //

        public OSPlatform osplatform;
        private deviceType device;
        private float canvasScaleFactor;
        float titleMargin;
        float bodyHeight;
        float footerMargin;

        public deviceType getDevice()
        {
            return device;
        }

        public float getCanvasScaler()
        {
            return canvasScaleFactor;
        }


        public void getAppPlatform()
        {
            // Debug.Log("width: " + Screen.width + ":: height:" + Screen.height);

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                osplatform = OSPlatform.IOS;
                return;
            }
            if (Application.platform == RuntimePlatform.Android)
            {
                osplatform = OSPlatform.ANDROID;
                return;
            }

            osplatform = OSPlatform.IOS;
        }

        public void getDeviceType()
        {
            float aspectRatio = GetLongerDimension().y / GetLongerDimension().x;
            Debug.Log("aspect ratio:" + aspectRatio);

            if (aspectRatio >= IphoneXAspectRation - 0.1f)
            {
                device = deviceType.IPHONEX;
                return;
            }
            else if (aspectRatio >= phoneAspectRatio - 0.1 && aspectRatio < IphoneXAspectRation)
            {
                device = deviceType.PHONE;
                return;
            }
            else if (aspectRatio >= androidTabletRatio - 0.1 && aspectRatio < phoneAspectRatio)
            {
                device = deviceType.TABLET;
                return;
            }
            else if (aspectRatio >= tabletAspectRatio - 0.1 && aspectRatio < androidTabletRatio)
            {
                device = deviceType.TABLET;
                return;
            }
            
            device = deviceType.PHONE;
        }


        public void getCanvasScaleFactor()
        {

            switch (device)
            {
                case deviceType.TABLET:
                    canvasScaleFactor = GetLongerDimension().x / ReferenceResolution.y;
                    break;
                case deviceType.PHONE:
                    canvasScaleFactor = GetLongerDimension().x / ReferenceResolution.x;
                    break;
                case deviceType.IPHONEX:
                    canvasScaleFactor = GetLongerDimension().x / ReferenceResolution.x;
                    break;
                default:
                    canvasScaleFactor = GetLongerDimension().x / ReferenceResolution.x;
                    break;
            }

            Debug.Log("canvas scale > "+canvasScaleFactor);
        }


        public void getMargins()
        {
            if(device == deviceType.IPHONEX)
            {
                titleMargin = 10f;
                bodyHeight = 0;
                footerMargin = 20f;
            }
            else
            {
                titleMargin = 0f;
                bodyHeight = 0f;
                footerMargin = 0f;
            
        }
        }


        public void setDeviceDetailsToSettings()
        {
            //  a check to see if main settings has been already populated
           
            getAppPlatform();
            getDeviceType();
            getCanvasScaleFactor();
            getMargins();

            //mainSettings.titleMargin = titleMargin;
            //mainSettings.bodyHeight = bodyHeight;
            //mainSettings.footerMargin = footerMargin;
            //mainSettings.osplatform = osplatform;
            //mainSettings.device = device;
            //mainSettings.canvasScaleFactor = canvasScaleFactor;
        }

        public float getTitleMargin()
        {
            return titleMargin;
        }

        public float getFooterMargin()
        {
            return footerMargin;
        }

        public float getBodyHeight()
        {
            return bodyHeight;
        }

        public void setValuesForMainSettings()
        {
            mainSettings.titleMargin = titleMargin;
            mainSettings.bodyHeight = bodyHeight;
            mainSettings.footerMargin = footerMargin;
            mainSettings.osplatform = osplatform;
            mainSettings.device = device;
            mainSettings.canvasScaleFactor = canvasScaleFactor;
        }


        Vector2 GetLongerDimension()
        {
            float widerDimension = 0f;
            float narrowerDimension = 0f;
            if (Screen.width > Screen.height)
            {
                widerDimension = Screen.width;
                narrowerDimension = Screen.height;
            }
            else
            {
                widerDimension = Screen.height;
                narrowerDimension = Screen.width;
            }
            return new Vector2(narrowerDimension, widerDimension);
        }
    }


    public enum OSPlatform
    {
        ANDROID,
        IOS,
    }

    public enum deviceType
    {
        PHONE,
        TABLET,
        IPHONEX,
        GALAXYS10,
    }

    public enum deviceOrientation
    {
        LANDSCAPE,
        PORTRAIT,
    }

}