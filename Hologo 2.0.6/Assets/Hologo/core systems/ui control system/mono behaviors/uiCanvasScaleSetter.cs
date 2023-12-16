using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 August 2019
    /// Modified by: 
    /// for setting all the ui canvas scalers
    /// 
    /// </summary>
    public class uiCanvasScaleSetter : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private settings_SO mainSettings;
        [Header("LOCALIZATION PREFABS UI TO USE IN CANVAS SCALING")]
        [SerializeField]
        private List<GameObject> canvasScaleSetters;
        [SerializeField]
        private bool setScaleOnStart = false;



        private void Awake()
        {
            if (setScaleOnStart)
                setResolutions(mainSettings.canvasScaleFactor, mainSettings.titleMargin, mainSettings.bodyHeight, mainSettings.footerMargin);
        }


        public void setCanvasScaleUIPrefabs(float scaleFactor,float tMargin, float bHeight, float fMargin)
        {
            setResolutions(scaleFactor, tMargin, bHeight, fMargin);
            

        }

        void setResolutions(float scaleFactor, float tMargin, float bHeight, float fMargin)
        {
            for (int i = 0; i < canvasScaleSetters.Count; i++)
            {
                ILayoutUI iLayout = canvasScaleSetters[i].GetComponent<ILayoutUI>();
                if (iLayout == null)
                    return;
                iLayout.setCanvasScaler(scaleFactor);
                iLayout.setMargins(tMargin, bHeight, fMargin, 0, 0);
            }

        }

    }
}
