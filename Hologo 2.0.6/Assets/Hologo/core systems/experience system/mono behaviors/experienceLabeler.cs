using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// experience label generator
    /// </summary>
    public class experienceLabeler : MonoBehaviour
    {
        // list of labels
        public List<labelclass> MyLabels;
        // label line width
        public float LineWidth = 0.005f;
        // label line color
        public Color LineColor = Color.white;
        // label follow speed
        public float LabelFollowSpeed = 5f;
        // labels face camera toggle
        public bool LabelsAlwaysFaceCamera = true;
        // labels follow delay toggle
        public bool AnimateLabelsFollow = true;
        // hiding these public fields since they will be populated at run time by ARExperienceMainScript
        [HideInInspector]
        public bool ShowLables = true;
        // parent transform where all label gameobjects will be parented under
        [HideInInspector]
        public Transform LabelParent;
        // main camera
        [HideInInspector]
        public Camera MyCamera;
        // line renderer material
        [HideInInspector]
        public Material LineMaterial;


        [HideInInspector]
        public Transform ScaleParent;

        // list to store the instantiated line renderers
        List<LineRenderer> lineList;
        // start updating the labels in update method
        bool startUpdating = false;
        // bool for firing each labels haptic method once
        List<bool> isHapticList;

        // for labels always visible state. set from settings
        private bool alwaysShowLabels = false;

//        private float degree = 0.4f; // to check if label is facing camera threshold
        private float nearDistance = 2f; // distance for the nearness to camera

        bool facingCamera;
        bool nearCamera;
        bool onHotSpot;

        private Transform parentScale;

        // Update is called once per frame
        void Update()
        {
            if (startUpdating)
            {
                if (ShowLables)
                {
                    if (alwaysShowLabels)
                    {
                        // show labels always
                        showlabelsAlways();
                    }
                    else
                    {
                        LabelsProximityCheck();
                    }

                    if (LabelsAlwaysFaceCamera)
                    {
                        OrientLabelsTowardstheCamera();
                    }
                    if (AnimateLabelsFollow)
                    {
                        labelsFollowTargets();
                    }
                    DrawLines();
                }

            }
        }

        // for showing labels intelligently.
        void LabelsProximityCheck()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                        LabelProximityCheckAndShowPointOriginsAndLabel(MyLabels[i], i);
                }
            }
        }

        // for showing labels always 
        void showlabelsAlways()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    showLabel(MyLabels[i], i);
                }
            }
        }


        void showLabel(labelclass mylabel, int i)
        {
            Animator myAnim = mylabel.Origin.GetComponentInChildren<Animator>();
            myAnim.SetBool("dotshow", true);
            myAnim.SetBool("dotanim", true);
            mylabel.LabelsActiveState(true);
            showLine(i);
        }

        // this mothod checks proximity of label to camera and displays them
        void LabelProximityCheckAndShowPointOriginsAndLabel(labelclass mylabel, int i)
        {

            // float dot = Vector3.Dot(mylabel.Origin.transform.position, (MyCamera.transform.position - mylabel.Origin.transform.position).normalized);
            // facingCamera = dot > degree;
            //  if (facingCamera && !mylabel.OmniDirectional)
            //{
            // getting distance between camera and label origin
            float camdistance = Vector3.Distance(mylabel.Origin.transform.position, MyCamera.transform.position);
            // check for nearness threshold
            nearCamera = camdistance < (nearDistance * ScaleParent.localScale.x);
            // if near camera
            if (nearCamera)
            {
                // here show dot
                // mylabel.EnableNodeMesh();
                // we enable dot and animate to reveal
                Animator myAnim = mylabel.Origin.GetComponentInChildren<Animator>();
                myAnim.SetBool("dotshow", true);
                // getting the screen postion of the camera
                Vector3 screenPoint = MyCamera.WorldToViewportPoint(mylabel.Origin.transform.position);
                // we are doing a check to see if the camera is in the center of the screen
                if (screenPoint.x > 0.3f && screenPoint.x < 0.6f && screenPoint.y > 0.3f && screenPoint.y < 0.6f)
                {
                    // check to see if haptic is false . to prevent it from firing always
                    if (isHapticList[i] == false)
                    {
                        // fire haptic
                        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                        // myAnim = mylabel.Origin.GetComponentInChildren<Animator>();
                        // animate the dot to reveal the label
                        myAnim.SetBool("dotanim", true);

                        // set haptic to true so it wont fire again
                        isHapticList[i] = true;
                    }
                    if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("dot_afteranim"))
                    {
                        // activating the label
                        mylabel.LabelsActiveState(true);
                        // drawing line between label and origin
                        showLine(i);
                    }
                }
                else
                {

                    if (isHapticList[i] == true)
                    {
                        // myAnim = mylabel.Origin.GetComponentInChildren<Animator>();
                        mylabel.LabelsActiveState(false);
                        hideLine(i);
                        myAnim.SetBool("dotanim", false);
                        isHapticList[i] = false;
                    }

                }
            }
            else
            {
                if (isHapticList[i] == true)
                {

                    isHapticList[i] = false;

                }

                mylabel.LabelsActiveState(false);
                Animator myAnim = mylabel.Origin.GetComponentInChildren<Animator>();
                myAnim.SetBool("dotshow", false);
                hideLine(i);

            }
        }






        public void InitMe(Transform scaler, GameObject originPrefab,bool alwaysShowLabels, List<GameObject> labels, Camera camera, Transform labelsParent, Transform scaleParent, Material material)
        {
            parentScale = scaler;
            this.alwaysShowLabels = alwaysShowLabels;
            MyCamera = camera;
            LineMaterial = material;
            LabelParent = labelsParent;
            ScaleParent = scaleParent;
            setLabels(labels);
            setOriginPrefabs(originPrefab);

            AssignMeACamera();

            if (AnimateLabelsFollow)
            {
                ChangeMyParent();
            }

            HideLabelNodeMeshes();
            MakeMyLines();

            if (!AnimateLabelsFollow)
            {
                DrawLines();
            }


            startUpdating = true;
            isHapticList = new List<bool>();

            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    MyLabels[i].LabelsActiveState(false);
                    isHapticList.Add(false);
                }
            }
            // assignButtons();
        }

        // assiging the instantiated labels to label points and
        void setLabels(List<GameObject> labels)
        {
            for (int i = 0; i < MyLabels.Count; i++)
            {
                labelElement le = labels[i].GetComponent<labelElement>();
                MyLabels[i].LabelPoint = labels[i];
                MyLabels[i].LabelPoint.transform.position = MyLabels[i].Target.transform.position;
                MyLabels[i].isNeverShown = le.isLabelActive();
                if (!le.isLabelActive())
                    MyLabels[i].hideLabel();

            }
        }


        void setOriginPrefabs(GameObject originPrefab)
        {
            for (int i = 0; i < MyLabels.Count; i++)
            {
                GameObject go = Instantiate(originPrefab) as GameObject;
                go.transform.SetParent(MyLabels[i].Origin.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }

        void assignButtons()
        {
            //if (MyLabels.Count > 0)
            //{
            //    for (int i = 0; i < MyLabels.Count; i++)
            //    {
            //        string title = MyLabels[i].Title;
            //        string description = MyLabels[i].Description;

            //        if (!string.IsNullOrEmpty(MyLabels[i].Title) && !string.IsNullOrEmpty(MyLabels[i].Description))
            //        {

            //            MyLabels[i].InfoButton.onClick.RemoveAllListeners();
            //            MyLabels[i].InfoButton.onClick.AddListener(() => ShowDescription(title, description));
            //            MyLabels[i].InfoButton.onClick.AddListener(ShowMe);
            //        }
            //    }
            //}
        }

        void ShowMe()
        {
            Debug.Log("working!");

        }

        // if there is more information to a label user can click it and get it.
        void ShowDescription(string title, string description)
        {
            //show them
            Debug.Log(title + "::" + description);

            // windowMain.ModalWindowD.ShowInfo(false, true, title, description, "Close");

        }

        // make a list of line renderers, a line for each label present;
        void MakeMyLines()
        {
            lineList = new List<LineRenderer>();
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    lineList.Add(createALine(MyLabels[i].Origin)); 
                }
            }
        }


        // make a list of line renderers, a line for each label present;
        void HideLabelNodeMeshes()
        {

            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    // MyLabels[i].DisableNodeMesh();
                }
            }
        }

        // drawing line between label origin and label point.
        void DrawLines()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                  //  Debug.Log(lineList[i].startWidth);
                    lineList[i].startWidth = (LineWidth * parentScale.localScale.x);
                    lineList[i].SetPosition(0, MyLabels[i].Origin.transform.position);
                    lineList[i].SetPosition(1, MyLabels[i].LabelPoint.transform.position);
                }
            }
        }

        void hideLine(int id)
        {
            lineList[id].enabled = false;
        }

        void showLine(int id)
        {
            lineList[id].enabled = true;
        }

        // making all the labels face the camera
        void OrientLabelsTowardstheCamera()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    Vector3 v = MyCamera.transform.position - MyLabels[i].LabelPoint.transform.position;
                    v.x = v.z = 0.0f;
                    MyLabels[i].LabelPoint.transform.LookAt(MyCamera.transform.position - v);
                    MyLabels[i].LabelPoint.transform.rotation = (MyCamera.transform.rotation); // Take care about camera rotation
                    MyLabels[i].Origin.transform.LookAt(MyCamera.transform.position - v);
                    MyLabels[i].Origin.transform.rotation = (MyCamera.transform.rotation);
                }
            }
        }

        // labels follow the target
        void labelsFollowTargets()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                        MyLabels[i].LabelPoint.transform.position = Vector3.Lerp(MyLabels[i].LabelPoint.transform.position,
                                                                             MyLabels[i].Target.transform.position,
                                                                             Mathf.SmoothStep(0, 1, Time.deltaTime * LabelFollowSpeed));
                }
            }
        }

        
        // creatting one line renderer and returning it
        LineRenderer createALine(GameObject go)
        {
            // Add a Line Renderer to the GameObject
            LineRenderer line = go.AddComponent<LineRenderer>();
            // Set the width of the Line Renderer
            line.startWidth = LineWidth;
            line.positionCount = 2;
            line.startColor = LineColor;
            line.material = LineMaterial;
            return line;
        }


        // assigning camera to lables world space canvas event camera
        void AssignMeACamera()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    Canvas canvas = MyLabels[i].LabelPoint.GetComponentInChildren<Canvas>();
                    canvas.worldCamera = MyCamera;
                }
            }
        }

        // changing the parent of the label so it can follow the target
        void ChangeMyParent()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                  
                        MyLabels[i].LabelPoint.transform.SetParent(LabelParent);
               
                }
            }
        }


        // from experience ui button toggle
        public void MyLabelsEnable(bool enable)
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    MyLabels[i].LabelsActiveState(enable);
                    lineList[i].gameObject.SetActive(enable);

                    // Shariz 29th Feb 2020 v2.0.4
                    if(MyLabels[i].isNeverShown == false){
                        MyLabels[i].LabelsActiveState(false);
                        lineList[i].gameObject.SetActive(false);
                    }
                }
            }
            ShowLables = enable;
            
            //  EnableDisableLabels(enable);

        }


        private void OnEnable()
        {

            // EnableDisableLabels(ShowLables);
        }

        private void OnDisable()
        {
            if (MyLabels.Count > 0)
            {
                for (int i = 0; i < MyLabels.Count; i++)
                {
                    MyLabels[i].LabelsActiveState(false);
                }
            }
        }
    }

    // struct to contain a label and its origin and target to follow gameobjects
    [System.Serializable]
    public class labelclass
    {
        public string LabelName;
        // the point where the origin of the label is . ie part of the model
        public GameObject Origin;
        // the target that the lable point should follow
        public GameObject Target;
        // label point where the line rendere connects 
        public GameObject LabelPoint;
        // button
       // public Button InfoButton;
        // is omini
        public bool OmniDirectional = true;

        // bool for toggling labels on and off
        public bool showLabel = true;
        [HideInInspector]
        // bool for not showing the label .. for localization purpose
        public bool isNeverShown = false;


        // show hide label gameobjects
        public void LabelsActiveState(bool enable)
        {
            // Origin.SetActive(enable);
            if (showLabel)
            {
                LabelPoint.SetActive(enable);
            }
        }

        public void labelIsNeverShown(bool state)
        {
            isNeverShown = state;
        }

        public void hideLabel()
        {
            Origin.SetActive(false);
            Target.SetActive(false);
            LabelPoint.SetActive(false);
            showLabel = false;
            
        }
    }
}