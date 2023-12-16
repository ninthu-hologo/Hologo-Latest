using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;



namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// base class for iOS UI button controls.
    /// </summary>
    public class iOS_UI_Buttons_Base : MonoBehaviour, IUIControl
    {
        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;

        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("Does translation require an object to be selected?")]
        public LeanSelectable RequiredSelectable;

        //rect that follows the finger/ mouse
        
        public RectTransform Canvas;
        [SerializeField]
        protected bool DirectionRight;

        //parent rect.
        RectTransform ParentRect;

        protected bool Selected = false;
        protected bool DeSelected = true;
        protected bool FingerUp = false;
        protected float screenWidth;
        protected Vector2 canvasDelta;
        protected float canvasPosition;

        protected bool isMovingHorizontal = false;
        protected bool checkforHori = true;


        private void OnEnable()
        {
            Canvas canvas = FindObjectOfType<Canvas>() as Canvas;
            Canvas = canvas.GetComponent<RectTransform>();
        }


        public virtual void OnSelected()
        {
            Selected = true;
            FingerUp = false;
            DeSelected = false;
           // Debug.Log("Selected");
        }

        public virtual void OnFingerUp()
        {
            Selected = false;
            FingerUp = true;
            DeSelected = false;
            FingerUpFuncs();
           // Debug.Log("finger up");
        }

        protected virtual void OnRectTransformDimensionsChange()
        {
            screenWidth = Screen.width;
        }

        public virtual void OnDeSelected()
        {
            Selected = false;
            FingerUp = false;
            DeSelected = true;
        }

        // tap button;
        public virtual void OnTapped()
        {

        }

        //swiped button;
        public virtual void OnSwiped()
        {

        }

        public virtual void OnHeld()
        {
            
        }


        // Use this for initialization
        protected virtual void Start()
        {
           // iOS_UIControlMain cm = FindObjectOfType(typeof(iOS_UIControlMain)) as iOS_UIControlMain;
           // Canvas = cm.Canvas;
           // Debug.Log(Canvas.rect.width.ToString());

            if (RequiredSelectable == null)
            {
                RequiredSelectable = GetComponent<LeanSelectable>();
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            

            // If we require a selectable and it isn't selected, cancel translation
            if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
            {
                return;
            }


            // getting current fingers on screen
            var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

            if (fingers != null)
            {
                MovingVertOrHori(LeanGesture.GetScreenDelta(fingers));
                // Calculate the screenDelta value based on these fingers and scaled down to canvas width from screen width
                canvasDelta = (LeanGesture.GetScreenDelta(fingers) * Canvas.rect.width) / screenWidth;
                // actual position of finger on canvas with respect to screen.
                canvasPosition = (fingers[0].ScreenPosition.x * Canvas.rect.width) / screenWidth;
                // whether finger is moving vertical or horizontal


                GetDirection(fingers);
            }

            if (Selected)
            {
                
                SeletedFuncs();
            }

        }


        private void MovingVertOrHori(Vector2 fingerDelta)
        {
            if(Mathf.Abs(fingerDelta.x)>Mathf.Abs(fingerDelta.y))
            {
                isMovingHorizontal = true;
               // checkforHori = false;
            }
            else if  (Mathf.Abs(fingerDelta.x) < Mathf.Abs(fingerDelta.y))
            {
                isMovingHorizontal = false;
               // checkforHori = false;

            }else
            {
                isMovingHorizontal = false;
            }
        }



        private void GetDirection(List<LeanFinger> fingers)
        {
            float fingerDirection = fingers[0].ScreenDelta.x;
            if (fingerDirection < -0.1f)
            {
                DirectionRight = true;
            }
            if(fingerDirection > 0.1)
            {
                DirectionRight = false;
            }
        }




        protected virtual void SeletedFuncs()
        {

        }

        protected virtual void FingerUpFuncs()
        {
            
        }

        protected virtual void DeSeletedFuncs()
        {

        }


        protected virtual void Translate(RectTransform myRect, Vector2 canvasDelta, float canvasPosition)
        {
            
            myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x + canvasDelta.x, myRect.anchoredPosition.y);
        }




    }
}
