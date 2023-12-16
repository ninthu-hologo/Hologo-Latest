using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 20 july 2019
    /// Modified by: 
    /// this class instantiates portal and keeps track of it
    /// </summary>
    public class portalInstantiator : MonoBehaviour
    {
        [Header("GAME OBJECTS FROM SCENE")]
        [SerializeField]
        private Transform gyroParent;
        [SerializeField]
        private Transform ArSessinOrigin;
        [SerializeField]
        private Transform stagingParent;
        [SerializeField]
        private GameObject bg;
        [SerializeField]
        private Transform objectLocation;
        [SerializeField]
        private disableEnableExperienceInteraction expInteraction;
        [Tooltip("the main game object that detaches and places on ar plane")]
        public GameObject experiencParent;
        public Transform portalParent;
        public Transform portalDoorParent;
        public GameObject doorPrefab;
        public Dimension initialDimension;
        [SerializeField]
        private gyroControl gControl;
        [SerializeField]
        private arSwitcher expUI;
        [Header("portal config")]
        [SerializeField]
        private Vector3 objectLocationPosition;
        [SerializeField]
        private Vector3 portalInsideDistance;
        [SerializeField]
        private Vector3 poralOutsideDistance;

        bool toInside;
        bool lerpMe;
        bool toOutside;
        private float startTime;
        [SerializeField]
        private float timeToReachTarget = 12f;

        private Portal myPortal;
        private portalDoor myPortalDoor;
        // portal door
        //[SerializeField]
        private GameObject portal;
        private BoxCollider portalCollider;

        private bool inPortal = false;
        private bool portalObjectMode = false;
        

        [SerializeField]
        private arSwitcher leavePortalBtn; // shariz 2.0.2 10th Dec 2019

        [SerializeField] 
        private settings_SO mainSettings; // shariz 2.0.2 28th nov 2019





        private GameObject secondDimension;


        public void makePortalExpereince(experienceData_SO Exp)
        {
            if (secondDimension != null)
                Destroy(secondDimension);
            //if (portal != null)
            //    Destroy(portal);

            stagingParent.gameObject.SetActive(true);

            portal = Instantiate(doorPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            portal.transform.SetParent(portalDoorParent);
            portal.transform.localPosition = Vector3.zero;
            //goP.transform.localScale = Vector3.one;
            secondDimension = Instantiate(Exp.Models[0].modelSlide, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            secondDimension.transform.SetParent(portalParent);
            secondDimension.transform.localPosition = Vector3.zero;
            secondDimension.transform.localScale = Vector3.one;

            myPortal = portal.GetComponentInChildren<Portal>();
            myPortalDoor = portal.GetComponent<portalDoor>();
            myPortalDoor.doorEvent.RemoveAllListeners();
            myPortalDoor.doorEvent.AddListener(doorOpenedEvent);
            Dimension experienceDimension = secondDimension.GetComponent<Dimension>();
            myPortal.dimension1 = initialDimension;
            myPortal.dimension2 = experienceDimension;
            portal.SetActive(true);
            myPortal.initiatePortals();
            portalCollider = portal.GetComponent<BoxCollider>();
            // setting object location to portal location distance
            objectLocation.localPosition = objectLocationPosition;//new Vector3(0, -2.44f, 7.53f);
            // unparenting the experience parent so camera can go inside of portal in object mode
            experiencParent.transform.SetParent(null);
            // experiencParent.transform.position = portalLocation;
            // parenting ar session origin under gyro so gryo rotations can be applied in object mode
            ArSessinOrigin.SetParent(gyroParent);
            inPortal = false;
            //disabling move/rotate/scale in object mode. ie initially
            expInteraction.disableEnableManipulation(false);
            portalObjectMode = true;


            myPortalDoor.backPlane.SetActive(false); // shariz 2.0.2 10th Dec 2019
            // shariz 2.0.2 28th nov 2019
            if (portalObjectMode)
            {
                leavePortalBtn.GetObjectModelPortalExitButton().SetActive(false);  // shariz 2.0.2 10th Dec 2019
            }
        }

        public GameObject getPortalCollider()
        {
            return portal;
        }

        // event fired from door open animation
        public void doorOpenedEvent(bool state)
        {
            if (state)
            {
                Debug.Log("going into portal");
                if (portalObjectMode)
                    goingIntoPortal();
            }
        }

        // this will go in and out of portal
        public void GoInOrOutOfPortal(GameObject go)
        {
            
            
            // can go in and out of portal in object mode only
            if (!portalObjectMode)
                return;

            myPortalDoor.backPlane.SetActive(true); // shariz 2.0.2 10th Dec 2019

            if (inPortal)
            {
                goingOutOfPortal();
                leavePortalBtn.GetObjectModelPortalExitButton().SetActive(false); // shariz 2.0.2 10th dec 2019
            }
            else
            {
                myPortalDoor.openDoor();
                leavePortalBtn.GetObjectModelPortalExitButton().SetActive(true); // shariz 2.0.2 10th dec 2019
            }
        }

        void goingIntoPortal()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
            toInside = true;
            toOutside = false;
            bg.SetActive(false);
            lerpMe = true;
            gControl.enableGyroNav(true);
            expUI.GetExperienceDisplayUIElementDevice().disableArButton();
        }

        void goingOutOfPortal()
        {
            Debug.Log("going out of portal");
            toOutside = true;
            toInside = false;
            lerpMe = true;
            gControl.enableGyroNav(false);
            bg.SetActive(true);

            myPortalDoor.closeDoor();
        }


        public void OpenDoorFromArPlace(bool state)
        {
            
            if (state)
            {
                Debug.Log("open door ar");
                myPortalDoor.openDoor();
            }
            else
            {
                Debug.Log("close door ar");
                myPortalDoor.closeDoor();
            }
        }

        // event raised from ar view controller
        public void portalArEvent(bool arModeOn)
        {
            myPortalDoor.backPlane.SetActive(false); // shariz 2.0.2 10th Dec 2019
            expInteraction.disableEnableManipulation(arModeOn);
            portalObjectMode = !arModeOn;
            // disble portal collider so move/rotate/scale can work
            portalCollider.enabled = !arModeOn;
            if (!arModeOn)
            {
               if(myPortal.FromDimension().initialWorld == false)
                {
                    myPortal.SwitchDimensions();
                }
            }
                
        }


        public void cleanUpPortal()
        {
            Debug.Log("cleaning up portal");
            ArSessinOrigin.parent = null;
            myPortal.dimension1 = null;
            myPortal.dimension2 = null;
            myPortal.killPortal();
            Destroy(portal);//hamid 30th oct 2019
            Destroy(secondDimension); //hamid 30th oct 2019
            
        }

        // Hamid 2.0.2 11th dec 2019 fixing portal switch bug
        public void switchDimensionForce()
        {
            if(toOutside)
            {
                // check whether current dimension == todimension
                // else switch dimesion 
                if(myPortal.dimensionSwitched == true)
                    myPortal.SwitchDimensions();
            }
            if(toInside)
            {
                // check whether current dimension == fromdimension
                // else switch dimesion
                if(myPortal.dimensionSwitched == false) 
                    myPortal.SwitchDimensions();
            }


        }


        float t;

        void Update()
        {
            if (toInside)
            {
                if (lerpMe)
                {

                    //increment timer once per frame
                    t += Time.deltaTime;

                    float perc = t / timeToReachTarget;
                    //  Debug.Log(perc);
                    stagingParent.position = Vector3.Lerp(stagingParent.position, portalInsideDistance, perc);
                    if (t >= timeToReachTarget)
                    {

                        t = 0;

                        inPortal = true;
                        lerpMe = false;
                        switchDimensionForce(); // Hamid 2.0.2 11th dec 2019 fixing portal switch bug
                    }
                }
            }

            if (toOutside)
            {
                if (lerpMe)
                {
                    t += Time.deltaTime;

                    float perc = t / timeToReachTarget;
                    // Debug.Log(perc);
                    stagingParent.position = Vector3.Lerp(stagingParent.position, poralOutsideDistance, perc);
                    if (t >= timeToReachTarget)
                    {
                        t = 0;

                        inPortal = false;
                        
                        // Shariz 2.0.2 28th nov 2019
                        if(mainSettings.isArSupported){
                        expUI.GetExperienceDisplayUIElementDevice().enableArButton();
                        }

                        lerpMe = false;
                        switchDimensionForce();// Hamid 2.0.2 11th dec 2019 fixing portal switch bug

                    }
                }
            }

        }

    }
}