using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 August 2019
    /// Modified by: 
    /// this can be used as a base class for ui elements orientation change
    /// </summary>
    public class iUILayoutBase : MonoBehaviour, ILayoutUI
    {
        [Header("MARGIN SETTER RECTS")]
        [SerializeField]
        public RectTransform titlePanel;
        [SerializeField]
        public RectTransform bodyPanel;
        [SerializeField]
        public RectTransform footerPanel;

        private CanvasScaler canvasScaler;
        

        public virtual void changeLayout(deviceOrientation orientation, float UIwidth, float BodyWidth, float UIHeight)
        {


        }


        public RectTransform getTitleTransform()
        {
            return titlePanel;
        }
        public RectTransform getBodyTransform()
        {
            return bodyPanel;
        }

        public virtual void setCanvasScaler(float scaler)
        {
            canvasScaler = gameObject.GetComponent<CanvasScaler>();
            canvasScaler.scaleFactor = scaler;
        }

        public virtual void setMargins(float titleMargin, float bodyHeight, float footerMargin, float rightMargin, float leftMargin)
        {

            // set all the margins here.
            if(titleMargin >0 && titlePanel !=null)
            {
                titlePanel.sizeDelta = new Vector2(titlePanel.sizeDelta.x, titlePanel.sizeDelta.y + titleMargin);
            }
            if(bodyHeight > 0 && bodyPanel != null)
            {
                bodyPanel.offsetMax = new Vector2(0f, bodyPanel.offsetMax .y+bodyHeight *-1);
                bodyPanel.offsetMin = new Vector2(0f, 0f);
                bodyPanel.pivot = new Vector2(0.5f, 1f);
                bodyPanel.anchorMax = new Vector2(1f, 1f);
                bodyPanel.anchorMin = new Vector2(0f, 0f);
            }
            if(footerMargin >0 && footerPanel != null)
            {
                footerPanel.sizeDelta = new Vector2(footerPanel.sizeDelta.x, footerPanel.sizeDelta.y + footerMargin);
            }
        }


    }


}