using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// settings boot up script
    /// </summary>
    public class deviceBootUp : MonoBehaviour, IBootUp
    {
        [SerializeField]
        private deviceManager dManager;
        [SerializeField]
        private uiCanvasScaleSetter canvasScalerSetter;
        private bool bootUpSuccess;
        [SerializeField]
        orientationLocker oLocker;


        public bool didBoot()
        {
            return true;
        }

        public void runBootSequence()
        {
            dManager.setDeviceDetailsToSettings();
            oLocker.setOrientation(dManager.getDevice());
            setCanvasScalers();
        }


        public void setDeviceToSettings()
        {
            dManager.setValuesForMainSettings();
        }

        public void setCanvasScalers()
        {
            canvasScalerSetter.setCanvasScaleUIPrefabs(dManager.getCanvasScaler(), dManager.getTitleMargin(), dManager.getBodyHeight(), dManager.getFooterMargin());
        }
    }
}