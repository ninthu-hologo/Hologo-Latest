using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Hologo.iOSUI;

public class portalArGyroControl : MonoBehaviour
{

    public bool toggleArGyroMode = false;
    public bool toggleEnter = false;

    public Transform CameraParent;
    public Transform ArCamera;
    public Transform door;
    public GameObject DoorWay;

    public Vector3 defaultDoorPosition;
    public Vector3 insidePan;
    public Vector3 outsidePan;

    [SerializeField]
    private ARSession arSession;
    [SerializeField]
    private gyroControl gyro;
    [SerializeField]
    private ARPlaneManager planeManager;
    bool moveToArPlane = false;
    
    [SerializeField]
    private iOS_UIModalWindowMain windowMain;
    [Tooltip("The \"From\" real world.")]
    public Dimension dimension1;
    [Tooltip("The \"From\" virtual world.")]
    public Dimension dimension2;

    // position of ar plane 
    private Vector3 arPlanePosition;
    bool toAr;
    bool lerpMe;
    bool toObject;
    private float startTime;
    private float timeToReachTarget = 0.3f;

    public void toggleMode()
    {
        if (toggleArGyroMode)
        {
            
            ArMode();
            toggleArGyroMode = false;
        }
        else
        {
            ObjectMode();
            toggleArGyroMode = true;
        }

    }


    void resetArCamera()
    {
        ArCamera.localPosition = Vector3.zero;
        ArCamera.localRotation = Quaternion.identity;
    }

    void Awake()
    {
        arSession.enabled = false;
        planeManager.planesChanged += planesChangedMine;
        moveToArPlane = true;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= planesChangedMine;
        if (arSession != null)
            arSession.enabled = false;
    }

    private void Start()
    {
        toggleArGyroMode = false;
        toggleMode();
        initiatePortal();
    }


    void initiatePortal()
    {
        Portal pt = DoorWay.GetComponent<Portal>();
        pt.dimension1 = dimension1;
        pt.dimension2 = dimension2;
        pt.initiatePortals();
    }

    public void enterWorld()
    {
        if (toggleEnter)
        {
            //setCameraParentLocation(insidePan);
            toAr = true;
            toObject = false;
            lerpMe = true;
            gyro.enableGyroNav(true);
            toggleEnter = false;
        }
        else
        {
            //setCameraParentLocation(outsidePan);
            toAr = false;
            toObject = true;
            lerpMe = true;
            gyro.enableGyroNav(false);
            toggleEnter = true;
        }

    }

    void ObjectMode()
    {
        Debug.Log("object mode");
        arSession.enabled = false;
        // ArHandler.PauseAr();
        // disabling plane detected
        resetArCamera();
        door.position = defaultDoorPosition;
        planeManager.enabled = false;
        moveToArPlane = true;

    }


    void setCameraParentLocation(Vector3 pos)
    {
        CameraParent.position = pos;
    }

    void ArMode()
    {
        Debug.Log("ar mode");
        gyro.enableGyroNav(false);
        if (!arSession.enabled)
        {
            arSession.enabled = true;
        }
       
            arSession.Reset();

        // enable plane detected
        planeManager.enabled = true;
    }

    void toAr2()
    {
        Debug.Log("ar plane location");
        door.position = arPlanePosition;

    }

    // event fired when ar plane manager detects a plane
    void planesChangedMine(ARPlanesChangedEventArgs args)
    {
        
        detectedPlane();

    }

    void removeExistingPlanes()
    {
       
    }

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
            arPlanePosition = arPlanes[arPlanes.Count-1].gameObject.transform.position;
            windowMain.InfomationToast.ShowToast("ar planes found", 2f);
            // setting the location of arplane in arobjectplacer to detected plane
            // finally move the ar object to the plane
            // disabling plane detected
            toAr2();
            planeManager.enabled = false;
            moveToArPlane = false;
        }
    }

    float t;

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
                CameraParent.position = Vector3.Lerp(CameraParent.position, insidePan, t);
                

                //moveParent.localPosition = Vector3.Lerp(moverTranslate, Vector3.zero, t);
                //moveScaleParent.localRotation = Quaternion.Lerp(moverRotation, Quaternion.identity, t);
                //rotationParent.localRotation = Quaternion.Lerp(rotateObjectRotation, Quaternion.identity, t);
                //// RotateObject.localRotation()
                //mainObjectScaleSetter.localScale = Vector3.Lerp(MoverDefaultScaleInObjectMode, MoverDefaultScaleInARMode, t);

                if (t >= timeToReachTarget + 1f)
                {
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                    // ArHandler.TurnOffPlaneDetection();
                    t = 0;
                    lerpMe = false;
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
                CameraParent.position = Vector3.Lerp(CameraParent.position, outsidePan, t);


                if (t >= timeToReachTarget + 1f)
                {
                    t = 0;
                    lerpMe = false;
                    
                }
            }
        }

    }

}