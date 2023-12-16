using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Hologo.iOSUI
{

    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// iOS smart button where user can drag from right to left to reveal more options.
    /// </summary>
    /// 
    public class iOS_SmartButtonOptions : iOS_UI_Buttons_Base
    {

        public iOSUIRect FollowRect;
        public List<iOSUIRect> ButtonList;
        float endPosition;
        public LayoutElement mylayoutElement;
        public iOSUIRect TextArea;
        public iOSUIRect HighLighter;
        public ScrollRect MyScrollRect;

        [SerializeField]
        public UnityEvent TapEvent;


        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void OnTapped()
        {
            
          //  Debug.Log(" tap");

            if (ButtonList[0].iOSRect.anchoredPosition.x <= ButtonList[0].Destination)
            {
                

                if (LeanTween.isTweening(FollowRect.iOSRect))
                    LeanTween.cancel(FollowRect.iOSRect);
                if (LeanTween.isTweening(ButtonList[0].iOSRect))
                    LeanTween.cancel(ButtonList[0].iOSRect);
                if (LeanTween.isTweening(ButtonList[1].iOSRect))
                    LeanTween.cancel(ButtonList[1].iOSRect);
                if (LeanTween.isTweening(ButtonList[2].iOSRect))
                    LeanTween.cancel(ButtonList[2].iOSRect);

                MoveToZeroPosition();
            }
            else
            {
                //some other function tap
                TapEvent.Invoke();
                LeanTween.alpha(HighLighter.iOSRect, 0.4f, 0.1f).setEase(LeanTweenType.easeInOutQuart).setLoopPingPong(1);


            }

        }


        public override void OnDeSelected()
        {
            base.OnDeSelected();
            //Debug.Log("deslected");

            MoveToZeroPosition();

        }

        protected override void SeletedFuncs()
        {
            if (isMovingHorizontal)
            {
                if (MyScrollRect != null)
                {
                    MyScrollRect.vertical = false;
                }
            }

            if (isMovingHorizontal)
            {
                Translate(FollowRect.iOSRect, canvasDelta, 0f);
                Translate(TextArea.iOSRect, canvasDelta, 0f);

                if (FollowRect.iOSRect.anchoredPosition.x >= ButtonList[0].Destination)
                {
                    Translate(ButtonList[0].iOSRect, canvasDelta, 0f);
                    float deltaDivide = canvasDelta.x / 3f;

                    Translate(ButtonList[1].iOSRect, new Vector2((deltaDivide * 2), ButtonList[1].iOSRect.anchoredPosition.y), 0f);
                    Translate(ButtonList[2].iOSRect, new Vector2((deltaDivide), ButtonList[1].iOSRect.anchoredPosition.y), 0f);
                }
            }
        }



        protected override void FingerUpFuncs()
        {
            base.FingerUpFuncs();
            if (MyScrollRect != null)
            {
                MyScrollRect.vertical = true;
            }
            if (FollowRect.iOSRect.anchoredPosition.x <= ButtonList[1].Destination)
            {
                LeanTween.moveX(FollowRect.iOSRect, ButtonList[0].Destination, 0.3f);
                LeanTween.moveX(ButtonList[0].iOSRect, ButtonList[0].Destination, 0.3f);
                LeanTween.moveX(ButtonList[1].iOSRect, ButtonList[1].Destination, 0.3f);
                LeanTween.moveX(ButtonList[2].iOSRect, ButtonList[2].Destination, 0.3f);
                LeanTween.moveX(TextArea.iOSRect, ButtonList[0].Destination, 0.3f);
            }
            else
            {
                MoveToZeroPosition();

            }
           

        }

        public void MoveToZeroPosition()
        {
            LeanTween.moveX(FollowRect.iOSRect, 0f, 0.3f);
            LeanTween.moveX(ButtonList[0].iOSRect, 0f, 0.3f);
            LeanTween.moveX(ButtonList[1].iOSRect, 0f, 0.3f);
            LeanTween.moveX(ButtonList[2].iOSRect, 0f, 0.3f);
            LeanTween.moveX(TextArea.iOSRect, 0f, 0.3f);
        }

        public void DeleteMe()
        {
            
            LeanTween.value(this.gameObject, ScaleLayoutElement, ButtonList[2].iOSRect.rect.height, 0, 0.2f)
                     .setEase(LeanTweenType.easeOutExpo)
                     .setOnComplete(DestroyMe);
        }

        void ScaleLayoutElement(float moved)
        {
            mylayoutElement.preferredHeight = moved;
        }

        void DestroyMe()
        {
           // Debug.Log("deleted");
            Destroy(mylayoutElement.gameObject);
        }
    }
}
