using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 21 August 2019
    /// Modified by: 
    /// this behavior places object in ar plane and object mode.
    /// </summary>
    public class arObjectPlacer : MonoBehaviour
    {
        [Header("AR GAMEOBJECT FIELDS")]
        [SerializeField]
        private Transform objectParent;
        [SerializeField]
        private Transform scaleParent;
        [SerializeField]
        private Transform moveParent;
        //[SerializeField]
        //private Transform rotateParent;
        [SerializeField]
        private Transform rotationParent;
        [SerializeField]
        private Transform ObjectModeLocation;
        [SerializeField]
        private Transform mainObjectScaleSetter;
        // object that snaps to the arp plane. and the object
        [SerializeField]
        private Vector3 ArPlaneLocation = new Vector3(0f, 0f, 2.24f);

        [Header("SCRIPTS")]
        [SerializeField]
        private arViewController arVController;
        [SerializeField]
        private Hologo_Lean_Rotate RotationScript;
        [Header("EVENTS")]
        [SerializeField]
        private event_SO placedOnPlaneEvent;



        [Header("PLACEMENT FIELDS")]
        [Tooltip("this will set the time for the model to move between object mode and ar mode")]
        [SerializeField]
        private float timeToReachTarget = 0.3f;

        private float startTime;
        private Vector3 objectScale;
        private Quaternion objectRotation;
        private Vector3 moverTranslate;
        private Vector3 moverScale;
        private Quaternion moverRotation;
        private Quaternion rotateObjectRotation;
        private Vector3 MoverDefaultScaleInObjectMode;
        private Vector3 MoverDefaultScaleInARMode;

        private bool lerpMe;
        private bool toAr;
        private bool toObject;

        private bool isPortal = false;

        public void setPortalState(bool state)
        {
            isPortal = state;
        }

        public void setArPlaneLocations(Vector3 loc)
        {
            ArPlaneLocation = loc;
        }

        public void setScales(float objModeScale, float ArModeScale)
        {
            MoverDefaultScaleInObjectMode = new Vector3(objModeScale, objModeScale, objModeScale);
            MoverDefaultScaleInARMode = new Vector3(ArModeScale, ArModeScale, ArModeScale);
        }

        public void setObjectModeScale()
        {
            mainObjectScaleSetter.localScale = MoverDefaultScaleInObjectMode;
        }

        public void ResetModelLocalMRS()
        {
            moveParent.localPosition = Vector3.zero;
            rotationParent.localRotation = Quaternion.identity;
            scaleParent.localScale = Vector3.one;
            scaleParent.localPosition = Vector3.zero;
            rotationParent.localRotation = Quaternion.identity;
        }


        public void ToArMode()
        {
            Debug.Log("To Ar");
            // OpaqueBg.SetActive(false);
            arVController.ObjectRenderTextureActive(false);
            //  ArKMain.WholeExperienceActive(true);
            objectParent.SetParent(null);
            toAr = true;
            lerpMe = true;
            toObject = false;
            startTime = Time.time;
            RotationScript.Relative = false;
            // OpaqueBg.SetActive(false);
            objectScale = objectParent.localScale;
            objectRotation = objectParent.rotation;
            moverTranslate = moveParent.localPosition;
            moverRotation = moveParent.localRotation;
            rotateObjectRotation = moveParent.localRotation;
            moverScale = scaleParent.localScale;
            scaleParent.localPosition = Vector3.zero;
            scaleParent.localScale = Vector3.one;
        }

        public void ToObjectMode()
        {
            Debug.Log("back to object");
            //   OpaqueBg.SetActive(true);
            lerpMe = true;
            toObject = true;
            toAr = false;
            RotationScript.Relative = true;
            //   OpaqueBg.SetActive(true);
            startTime = Time.time;
            objectScale = objectParent.localScale;
            objectRotation = objectParent.rotation;
            moverTranslate = moveParent.localPosition;
            moverRotation = moveParent.localRotation;
            rotateObjectRotation = moveParent.localRotation;
            moverScale = scaleParent.localScale;
            scaleParent.localPosition = Vector3.zero;
            scaleParent.localScale = Vector3.one;
        }

        float t;


        // Update is called once per frame
        void Update()
        {
            if (toAr)
            {
                if (lerpMe)
                {
                    float timeSinceStarted = Time.time - startTime;
                    //   float percentageComplete = timeSinceStarted / speed;

                    t += Time.deltaTime / timeToReachTarget;
                    // Set our position as a fraction of the distance between the markers.
                    objectParent.position = Vector3.Lerp(ObjectModeLocation.position, ArPlaneLocation, t);
                    objectParent.localScale = Vector3.Lerp(objectScale, Vector3.one, t);
                    objectParent.rotation = Quaternion.Lerp(objectRotation, Quaternion.identity, t);

                    moveParent.localPosition = Vector3.Lerp(moverTranslate, Vector3.zero, t);
                    scaleParent.localRotation = Quaternion.Lerp(moverRotation, Quaternion.identity, t);
                    moveParent.localRotation = Quaternion.Lerp(rotateObjectRotation, Quaternion.identity, t);
                    // RotateObject.localRotation()
                    mainObjectScaleSetter.localScale = Vector3.Lerp(MoverDefaultScaleInObjectMode, MoverDefaultScaleInARMode, t);

                    if (t >= timeToReachTarget + 1f)
                    {
                        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                        // ArHandler.TurnOffPlaneDetection();
                        t = 0;
                        
                        lerpMe = false;
                        if (isPortal)
                            placedOnPlaneEvent.raiseBoolEvent(true);
                    }
                }
            }


            if (toObject)
            {
                if (lerpMe)
                {
                    float timeSinceStarted = Time.time - startTime;
                    //  float percentageComplete = timeSinceStarted / speed;

                    t += Time.deltaTime / timeToReachTarget;
                    // Set our position as a fraction of the distance between the markers.
                    objectParent.position = Vector3.Lerp(ArPlaneLocation, ObjectModeLocation.position, t);
                    objectParent.localScale = Vector3.Lerp(objectScale, Vector3.one, t);
                    objectParent.rotation = Quaternion.Lerp(objectRotation, ObjectModeLocation.rotation, t);

                    moveParent.localPosition = Vector3.Lerp(moverTranslate, Vector3.zero, t);
                    scaleParent.localRotation = Quaternion.Lerp(moverRotation, Quaternion.identity, t);
                    moveParent.localRotation = Quaternion.Lerp(rotateObjectRotation, Quaternion.identity, t);
                    mainObjectScaleSetter.localScale = Vector3.Lerp(MoverDefaultScaleInARMode, MoverDefaultScaleInObjectMode, t);


                    if (t >= timeToReachTarget + 1f)
                    {
                        t = 0;
                        lerpMe = false;
                        
                        if (!isPortal)
                        {
                            objectParent.SetParent(ObjectModeLocation);
                            
                        }
                        else
                        {
                            placedOnPlaneEvent.raiseBoolEvent(false);
                        }
                            
                    }
                }
            }

        }



    }



}