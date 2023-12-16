using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 24 August 2019
    /// Modified by: 
    /// this behavior keeps all the canvas orientation and canvas scale factors
    /// 
    /// </summary>
    [System.Serializable]
    public class uiLayoutController : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private settings_SO mainSettings;
        [Header("UI CANVAS SCALERS")]
        [SerializeField]
        private List<ILayoutUI> canvasUIScalers;

        private deviceOrientation dOrientation;

        protected virtual void OnRectTransformDimensionsChange()
        {
            DetermineScreenOrientation();
            updateLayouts();
        }


        public deviceOrientation getDeviceCurrentOrientation()
        {
            return dOrientation;
        }


        private void DetermineScreenOrientation()
        {
           
            if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                dOrientation = deviceOrientation.LANDSCAPE;
                //Debug.Log(orientation = "Landscape");
            }
            else
            {
                dOrientation = deviceOrientation.PORTRAIT;
                // Debug.Log(orientation = "portrait");
            }
        }


        public void setCanvasScalers()
        {
            for (int i = 0; i < canvasUIScalers.Count; i++)
            {
                canvasUIScalers[i].setCanvasScaler(mainSettings.canvasScaleFactor);
            }
        }


        void updateLayouts()
        {

        }

    }
}
