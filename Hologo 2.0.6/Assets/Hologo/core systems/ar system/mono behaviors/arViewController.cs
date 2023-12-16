using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 21 August 2019
    /// Modified by: 
    /// this controlles ar foundation for hologo
    /// </summary>
    public class arViewController : MonoBehaviour
    {
        [Header("SCENE OBJECTS")]
        [SerializeField]
        private GameObject objectScreenShotUI;
        [SerializeField]
        private GameObject objectModeBackground;
        [SerializeField]
        private GameObject experienceContainer;


        [Header("SCRIPTS")]
        [SerializeField]
        private ARSession arSession;
        [SerializeField]
        private ARPlaneManager planeManager;
        [SerializeField]
        private TrackedPoseDriver mydriver;
        [SerializeField]
        [Tooltip("The ARCameraManager which will produce frame events.")]
        ARCameraManager m_CameraManager;
        [SerializeField]
        private arObjectPlacer arOP;
        [SerializeField]
        private RenderTextureForAr renderTexAr;

        [Header("UI ELEMENTS")]
        [SerializeField]
        private arSwitcher expUIELement;
        [SerializeField]
        private GameObject arUsageUi;

        [Header("EVENTS")]
        [SerializeField]
        private event_SO portalArEvent;

        private bool mode = false;//Shariz 26th Feb 2020 v2.0.4

        [SerializeField]
        private bool TestMode;


        // position of ar plane 
        private Vector3 arPlanePosition;


        private bool moveToArPlane = false;
        // used for showing ar usage ui
        private bool showArUseUI = false;

        private bool isArSupported = false;

        private int expType;

//        private bool isPortal;

        /// <summary>
        /// Get or set the <c>ARCameraManager</c>.
        /// </summary>
        public ARCameraManager cameraManager
        {
            get { return m_CameraManager; }
            set
            {
                if (m_CameraManager == value)
                    return;

                if (m_CameraManager != null)
                    m_CameraManager.frameReceived -= FrameChanged;

                m_CameraManager = value;

                if (m_CameraManager != null & enabled)
                    m_CameraManager.frameReceived += FrameChanged;
            }
        }

        public void initiateARViewController(bool isArSupported)
        {
            this.isArSupported = isArSupported;
            if (!isArSupported)
            {
                expUIELement.GetExperienceDisplayUIElementDevice().disableArButton();
            }
        }



        void Awake()
        {
            arSession.enabled = false;
            planeManager.planesChanged += planesChangedMine;
            m_CameraManager.frameReceived += FrameChanged;
            moveToArPlane = true;

            // //Shariz 26th Feb 2020 v2.0.4
            // #if UNITY_EDITOR
            // Debug.Log("AR supported so toggling to ARMode");
            // ToggleObjectArMode();
            // #endif
        }

        private void OnDisable()
        {
            m_CameraManager.frameReceived -= FrameChanged;
            planeManager.planesChanged -= planesChangedMine;
            if (arSession != null)
                arSession.enabled = false;
        }


        public void ObjectRenderTextureActive(bool state)
        {
            objectScreenShotUI.SetActive(state);
        }

        // this script will run ar to place object on plane after event is detected
        void detectedPlane()
        {
            if (planeManager.trackables.count > 0)
            {
                if (!moveToArPlane)
                    return;

                List<ARPlane> arPlanes = new List<ARPlane>();
                // converting the detected planes dictionary to a list
                foreach (var plane in planeManager.trackables)
                    arPlanes.Add(plane);
                // getting the position of the first detected plane
                arPlanePosition = arPlanes[0].gameObject.transform.position;
                // setting the location of arplane in arobjectplacer to detected plane
                arOP.setArPlaneLocations(arPlanePosition);
                // finally move the ar object to the plane
                setExperienceContainerState(true);
                ObjectRenderTextureActive(false);
                arOP.ToArMode();
                // disabling plane detected 
                planeManager.enabled = false;
                moveToArPlane = false;
            }
        }

        // event fired when ar plane manager detects a plane
        void planesChangedMine(ARPlanesChangedEventArgs args)
        {
            detectedPlane();

        }

        // this is send from arexperiencemain to set the options for the experience.
        public void RecieveArFoundationSpecifisFromExperience(bool ar, bool enableShadow, float objectModeScale, float arModeScale, Color bgColor ,int experienceType)
        {
            expType = experienceType;
            if (experienceType == 1)
            {
                // isPortal = false;
               
                arOP.setPortalState(false);
            }

            if (experienceType == 2)
            {
               // isPortal = true;
                arOP.setPortalState(true);
            }

            if (!ar && !TestMode)
            {
                // ar mode is not supported so hide the button
                expUIELement.GetExperienceDisplayUIElementDevice().hideArModeChangeButton();
            }
            arOP.setScales(objectModeScale, arModeScale);
            changeBGColor(bgColor);
            // setting the object mode scale of the ar model
            arOP.setObjectModeScale();
        }


        // assigned to ar mode button
        public void ToggleObjectArMode()
        {
            if (!TestMode)
            {
                ArModeChange();
                
            }
            else
            {
                // in test mode the object is moved from near camera to a point away from camera
                testModeChange();
            }
        }

        #region AR TESTMODE METHODS
        // for testing . we don initiate ar sesson
        private void testModeChange()
        {
            if (mode)
            {
                mode = false;
                objectModeBackground.SetActive(true);
                setExperienceContainerState(true);
                expUIELement.GetExperienceDisplayUIElementDevice().changeArMode(true);
                arOP.ToObjectMode();
                portalEventExcute(false);
            }
            else
            {
                mode = true;
                objectModeBackground.SetActive(false);
                arOP.ResetModelLocalMRS();
                // taking a screen shot of the experience model and display it on screen
                renderTexAr.TakeScreenShot();
                setExperienceContainerState(false);
                expUIELement.GetExperienceDisplayUIElementDevice().changeArMode(false);
                StartCoroutine(simulateAr());
            }
        }


        IEnumerator simulateAr()
        {
            ObjectRenderTextureActive(true);
            yield return new WaitForSeconds(1f);
            ObjectRenderTextureActive(false);
            setExperienceContainerState(true);
            arOP.ToArMode();
            portalEventExcute(true);
        }

        void portalEventExcute(bool state)
        {
            if(expType ==2)
            {
                Debug.Log("portal test");
                portalArEvent.raiseBoolEvent(state);
            }
        }

        #endregion

        // for real ar mode change
        private void ArModeChange()
        {
            if (mode)
            {
                // object mode is on
                mode = false;
                objectModeBackground.SetActive(true);
                ObjectRenderTextureActive(false);
                setExperienceContainerState(true);
                ObjectMode();
                expUIELement.GetExperienceDisplayUIElementDevice().changeArMode(true);
                portalEventExcute(false);
            }
            else
            {
                // ar mode is on
                mode = true;
                objectModeBackground.SetActive(false);
                arOP.ResetModelLocalMRS();
                renderTexAr.TakeScreenShot();
                ObjectRenderTextureActive(true);
                //hide the experience while ar is being tracked
                setExperienceContainerState(false);
                ArMode();
                expUIELement.GetExperienceDisplayUIElementDevice().changeArMode(false);
                portalEventExcute(true);
            }
        }

        private void setExperienceContainerState(bool state)
        {
            experienceContainer.SetActive(state);
        }

        void ObjectMode()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            Debug.Log("object mode");
            // ArHandler.PauseAr();
            // disabling plane detected
            mydriver.enabled = false;
            planeManager.enabled = false;
            arOP.ToObjectMode();
            moveToArPlane = true;
            setARUsageUiState(false); // Shariz 16th Oct 2019 Hologo 2
            showArUseUI = false; // Shariz 16th Oct 2019 Hologo 2
        }

        void ArMode()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            Debug.Log("ar mode");
            if (!arSession.enabled)
            {
                arSession.enabled = true;
                return;
            }

            arSession.Reset();
            // enable plane detected
            mydriver.enabled = true;
            planeManager.enabled = true;
        }

        // change the color of object mode bj
        void changeBGColor(Color bgColor)
        {
            Renderer myRend = objectModeBackground.GetComponent<Renderer>();
            myRend.material.color = bgColor;
        }


        public void disableArSession()
        {
            planeManager.enabled = false;
            arSession.enabled = false;
        }
        #region SHOW HOW TO USE AR UI


        // event fired when camera frame changes
        void FrameChanged(ARCameraFrameEventArgs args)
        {
            if (!PlanesFound() && !showArUseUI)
            {
                showArUseUI = true;
                StartCoroutine(checkIfPlaneIsFound());
            }
        }
        // after ar mode is on . method will wait ten seconds before it starts showing this ui
        IEnumerator checkIfPlaneIsFound()
        {
            Debug.Log("Checking if plane is found and waiting for 10seconds");
            yield return new WaitForSeconds(10f);
            Debug.Log("Checking if plane is found and done waiting for 10seconds");
            if (!PlanesFound() && showArUseUI && mode) // Shariz 1st March 2020 v2.0.4
            {
                // show ar ui
                setARUsageUiState(true);
                Debug.Log("Checking if plane is found shwoing AR UI after because planes not found and ShoARUseUI");
            }
            
            yield return null;
        }

        bool PlanesFound()
        {
            if (planeManager == null)
                return false;

            
            bool p = planeManager.trackables.count > 0;
            if(p)
            {
                showArUseUI = false;
                setARUsageUiState(false);
            }
            return p;
        }

        void setARUsageUiState(bool state)
        {
            arUsageUi.SetActive(state);
            Debug.Log("Ar Usage Ui state is "+state);
        }

        #endregion


    }
}
