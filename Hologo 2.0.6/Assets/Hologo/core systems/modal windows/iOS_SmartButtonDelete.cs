using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//tweening engine.
using DentedPixel;
using Lean.Touch;
using UnityEngine.Events;


namespace Hologo.iOSUI
{

    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// iOS smart button where user can drag from right to left to delete the object in focus. or swipe to enable delete button.
    /// </summary>
    /// 
    public class iOS_SmartButtonDelete : iOS_UI_Buttons_Base
    {

        public iOSUIRect FollowRect;
        public float FollowRectScreenPercentage;
        public iOSUIRect RedBand;
        public iOSUIRect DeleteButton;
        public iOSUIRect TextArea;
        public iOSUIRect HighLighter;
        float endPosition;
        public LayoutElement mylayoutElement;
        public ScrollRect MyScrollRect;

        [SerializeField]
        public UnityEvent SmartDelete;
        [SerializeField]
        public UnityEvent SmartFingerHeld;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if(Canvas != null)
            {
                RedBand.iOSRect.sizeDelta = new Vector2(Canvas.rect.width, RedBand.iOSRect.sizeDelta.y);
                calculateScreenWidth();
            }


        }


        public override void OnSwiped()
        {
            
            //LeanTween.moveX(FollowRect.iOSRect, RedBand.Destination, 0.2f);
            //LeanTween.moveX(RedBand.iOSRect, RedBand.Destination, 0.2f);
            //LeanTween.moveX(DeleteButton.iOSRect, RedBand.Destination, 0.2f);
            //LeanTween.moveX(TextArea.iOSRect, RedBand.Destination, 0.2f);
        }

    


        public override void OnTapped()
        {
            
            LeanTween.cancel(FollowRect.iOSRect);
            LeanTween.cancel(RedBand.iOSRect);
            LeanTween.cancel(TextArea.iOSRect);
            LeanTween.cancel(DeleteButton.iOSRect);

            if(FollowRectScreenPercentage > 5)
            {
                //Debug.Log("retract tap");

                MoveToZeroPosition();
            }else
            {
               //some other function tap
            }
        }
        public override void OnHeld()
        {
           // Debug.Log("Held");
            if(FollowRect.iOSRect.anchoredPosition.x > -0.1)
            {
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                LeanTween.alpha(HighLighter.iOSRect, 0.6f, 0.2f).setEase(LeanTweenType.easeInOutQuart).setLoopPingPong(1);
                SmartFingerHeld.Invoke();
            }

        }

        public override void OnSelected()
        {

            base.OnSelected();
            FollowRectScreenPercentage = (FollowRect.iOSRect.anchoredPosition.x / Canvas.rect.width) * -100;
            LeanTween.cancel(FollowRect.iOSRect);
            LeanTween.cancel(RedBand.iOSRect);
            LeanTween.cancel(TextArea.iOSRect);
            LeanTween.cancel(DeleteButton.iOSRect);
            calculateScreenWidth();

        }
        public override void OnDeSelected()
        {
            base.OnDeSelected();
           // Debug.Log("deslected");

            MoveToZeroPosition();

        }



        bool followRectMoveToDestination = true;
        bool followDeleteMoveToDestination = true;

        protected override void SeletedFuncs()
        {

            if(isMovingHorizontal)
            {
                if(MyScrollRect!=null)
                {
                    MyScrollRect.vertical = false;
                }
            }

            if (isMovingHorizontal)
            {
                checkforHori = false;

                if (!DirectionRight && FollowRect.iOSRect.anchoredPosition.x > -0.1)
                {

                }
                else
                {

                    if (FollowRectScreenPercentage > 70 && DirectionRight)
                    {
                        if (followRectMoveToDestination)
                        {
                            followRectMoveToDestination = false;
                            MoveToEndPosition();
                        }

                    }
                    else if (!DirectionRight)
                    {
                        if (LeanTween.isTweening(FollowRect.iOSRect))
                        {
                            LeanTween.cancel(FollowRect.iOSRect);
                        }
                        if (LeanTween.isTweening(RedBand.iOSRect))
                        {
                            LeanTween.cancel(RedBand.iOSRect);
                        }
                        if (LeanTween.isTweening(TextArea.iOSRect))
                        {
                            LeanTween.cancel(TextArea.iOSRect);
                        }

                        Translate(FollowRect.iOSRect, canvasDelta, 0f);
                        Translate(RedBand.iOSRect, canvasDelta, 0f);
                        Translate(TextArea.iOSRect, canvasDelta, 0f);

                    }
                    else if (FollowRectScreenPercentage < 70)
                    {
                        followRectMoveToDestination = true;
                        Translate(FollowRect.iOSRect, canvasDelta, 0f);
                        Translate(RedBand.iOSRect, canvasDelta, 0f);
                        Translate(TextArea.iOSRect, canvasDelta, 0f);

                    }
                    else if (FollowRect.iOSRect.anchoredPosition.x > 0.1)
                    {
                        FollowRect.iOSRect.anchoredPosition = new Vector2(0, 0);
                        RedBand.iOSRect.anchoredPosition = new Vector2(0, 0);
                        TextArea.iOSRect.anchoredPosition = new Vector2(0, 0);
                        DeleteButton.iOSRect.anchoredPosition = new Vector2(0, 0);

                    }


                    if (FollowRect.iOSRect.anchoredPosition.x >= -90)
                    {
                        Translate(DeleteButton.iOSRect, canvasDelta, 0f);
                    }
                    else if (FollowRectScreenPercentage > 70 && DirectionRight)
                    {
                        if (followDeleteMoveToDestination)
                        {
                            if (LeanTween.isTweening(DeleteButton.iOSRect))
                            {
                                LeanTween.cancel(DeleteButton.iOSRect);
                            }
                            followDeleteMoveToDestination = false;
                            LeanTween.moveX(DeleteButton.iOSRect, endPosition, 0.3f);
                            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                        }

                    }
                    else if (!DirectionRight)
                    {

                        if (!followDeleteMoveToDestination)
                        {
                            if (LeanTween.isTweening(DeleteButton.iOSRect))
                            {
                                LeanTween.cancel(DeleteButton.iOSRect);
                            }
                            followDeleteMoveToDestination = true;
                            LeanTween.moveX(DeleteButton.iOSRect, -90, 0.3f);
                            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                        }

                    }


                    FollowRectScreenPercentage = (FollowRect.iOSRect.anchoredPosition.x / Canvas.rect.width) * -100;
                }
            }

        }

        private void MoveToEndPosition()
        {
            LeanTween.moveX(RedBand.iOSRect, endPosition, 0.2f);
            LeanTween.moveX(FollowRect.iOSRect, endPosition, 0.2f);
            LeanTween.moveX(TextArea.iOSRect, endPosition, 0.2f);
        }

        private void MoveToZeroPosition()
        {
            LeanTween.moveX(FollowRect.iOSRect, 0f, 0.3f);
            LeanTween.moveX(RedBand.iOSRect, 0f, 0.3f);
            LeanTween.moveX(DeleteButton.iOSRect, 0f, 0.3f);
            LeanTween.moveX(TextArea.iOSRect, 0f, 0.3f);
        }

        protected override void FingerUpFuncs()
        {
            base.FingerUpFuncs();

            if (MyScrollRect != null)
            {
                MyScrollRect.vertical = true;
            }

                if (FollowRectScreenPercentage > 85)
                {
                    DeleteMe();

                }
                else if (FollowRectScreenPercentage <= 85 && FollowRect.iOSRect.anchoredPosition.x <= -90)
                {
                    LeanTween.moveX(FollowRect.iOSRect, RedBand.Destination, 0.5f);
                    LeanTween.moveX(RedBand.iOSRect, RedBand.Destination, 0.5f);
                    LeanTween.moveX(DeleteButton.iOSRect, RedBand.Destination, 0.5f);
                    LeanTween.moveX(TextArea.iOSRect, RedBand.Destination, 0.5f);

                }
                else if (FollowRect.iOSRect.anchoredPosition.x > -90)
                {
                    LeanTween.moveX(FollowRect.iOSRect, 0f, 0.3f);
                    LeanTween.moveX(RedBand.iOSRect, 0f, 0.3f);
                    LeanTween.moveX(DeleteButton.iOSRect, 0f, 0.3f);
                    LeanTween.moveX(TextArea.iOSRect, 0f, 0.3f);
                }



        }

        void calculateScreenWidth()
        {
            endPosition = 0.98f * Canvas.rect.width * -1;
        }

        public virtual void DeleteMe()
        {
            
            LeanTween.moveX(RedBand.iOSRect, endPosition, 0.2f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.moveX(DeleteButton.iOSRect, endPosition, 0.2f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.moveX(FollowRect.iOSRect, endPosition, 0.2f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.moveX(TextArea.iOSRect, endPosition, 0.2f).setDelay(0.2f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.value(this.gameObject,ScaleLayoutElement,DeleteButton.iOSRect.rect.height, 0, 0.2f)
                     .setEase(LeanTweenType.easeOutExpo)
                     .setOnComplete(DestroyMe);
        }


        void ScaleLayoutElement(float moved)
        {
            mylayoutElement.preferredHeight = moved;
        }

        protected virtual void DestroyMe()
        {
           // Debug.Log("deleted");
            SmartDelete.Invoke();
            Destroy(mylayoutElement.gameObject);
        }


    }



}
