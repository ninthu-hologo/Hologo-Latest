using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 16 july 2019
    /// Modified by: 
    /// this class instantiates all the slide and keeps track of them
    /// </summary>
    public class experienceInstantiator : MonoBehaviour
    {
        //[SerializeField]
        //private experienceData_SO currentExperience;
        //[SerializeField]
        // private experienceLocalizationData_SO experienceLocalizedData;
        [Header("SCENE ELEMENTS")]
        [SerializeField]
        private List<GameObject> experienceModelList;
        private List<GameObject> allLabelsObjects;
        [SerializeField]
        private Hologo_LeanScale experienceScaleSet;
        [SerializeField]
        private List<GameObject> descriptionList;


        [Header("SCENE ELEMENTS")]
        [SerializeField]
        private Transform lineScaler;

        [SerializeField]
        private Transform scaleAndMoveParent;
        [SerializeField]
        private Transform scaleParent;
        [SerializeField]
        private Transform ObjectLocation;
        [SerializeField]
        private Transform exprienceSlidesParent;
        [SerializeField]
        private Transform labelsParent;
        [SerializeField]
        private Camera MainCamera;
        [SerializeField]
        private Material lineMaterialCommon;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private localizePrefab_SO labelPrefab;
        [SerializeField]
        private localizePrefab_SO descriptionPrefab;
        [SerializeField]
        private GameObject originMarkerPrefab;




        private GameObject currentExperienceSlide;

        // keeps track of which mode we are in
        private ExperienceSlideMode mode;


        // setting scale limits for the experience
        void setScaleForExperience(float minScale, float maxScale)
        {
            experienceScaleSet.ScaleMin = new Vector3(minScale, minScale, minScale);
            experienceScaleSet.ScaleMax = new Vector3(maxScale, maxScale, maxScale);
        }


        public void setSlidesActiveState(bool state)
        {
            exprienceSlidesParent.gameObject.SetActive(state);
        }

        // this makes a typical experience
        public void makeNormalExperience(experienceData_SO cExp, List<slideDetails> Models, List<experienceLabelNarrationDataClass> slideLocalizationData, bool alwaysShowLabels, float scaleMultiplier)
        {
            if (currentExperienceSlide != null)
                Destroy(currentExperienceSlide);

            setScaleForExperience(cExp.minScaleLimit, cExp.maxScaleLimit);
            // setting object mode scale

            allLabelsObjects = new List<GameObject> ();

            if (Models.Count >= 1)
            {
                experienceModelList = new List<GameObject>();
                descriptionList = new List<GameObject>();
                for (int i = 0; i < Models.Count; i++)
                {
                    // instantiating the model slide
                    GameObject go = Instantiate(Models[i].modelSlide, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.position = exprienceSlidesParent.position;
                    go.transform.rotation = exprienceSlidesParent.rotation;

                    go.transform.SetParent(exprienceSlidesParent);
                    go.transform.localScale = Vector3.one;
                    // initiateExperienceModelLabels(go);
                    // hiding the model
                    go.SetActive(false);
                    experienceModelList.Add(go);
                    //this is trail scale hack
                    //addTrailRendererScale(go);

                    // checking to see if labels exits for this slide
                    if (slideLocalizationData[i].labelsExist)
                    {
                        // making labels from localized data
                        List<GameObject> expLabels = makeLabels(i, slideLocalizationData, scaleMultiplier);
                        allLabelsObjects.AddRange(expLabels);
                        // initializing labels 
                        initiateExperienceModelLabels(alwaysShowLabels, go, expLabels);

                        
                    }
                    // check to see if slides description is enabled to display
                    if (slideLocalizationData[i].isInfoDisplayed)
                    {
                        // make the description ui element
                        makeDescriptionUIElement(go, slideLocalizationData[i]);
                    }
                    // setting the narration of the slide from localized data;
                    Models[i].narration = slideLocalizationData[i].Narration;
                    Models[i].isNarrationEnabled = slideLocalizationData[i].isNarrationEnabled;
                    Models[i].isInfoDisplayed = slideLocalizationData[i].isInfoDisplayed;
                }
            }

            scaleParent.localScale = new Vector3(cExp.objectModeScale, cExp.objectModeScale, cExp.objectModeScale);
            ObjectLocation.localPosition = cExp.objectLocation;
        }



        void addTrailRendererScale(GameObject go)
        {
            var myTrailRenderers = go.GetComponentsInChildren<TrailRenderer>();
            if(myTrailRenderers.Length > 0)
            {
                trailRendererScaler trs = go.AddComponent(typeof(trailRendererScaler)) as trailRendererScaler;
                trs.initTrailScaler(lineScaler, myTrailRenderers);
            }
        }



        // getting labels from localized data and instantiating them .
        List<GameObject> makeLabels(int id, List<experienceLabelNarrationDataClass> slideLocalizationData, float scaleMultiplier)
        {
            List<GameObject> labels = new List<GameObject>();
            for (int i = 0; i < slideLocalizationData[id].labelList.Count; i++)
            {
                // check to see if the label has any text. if not then we set label enabled to false even though
                // we create a label
                // we create empty label so we can keep the count ids same 
                bool enabledStatus = !string.IsNullOrEmpty(slideLocalizationData[id].labelList[i].label);
                // Debug.Log("label enable status " + enabledStatus + ">" + slideLocalizationData[id].labelList[i].labelName);

                GameObject go = Instantiate(labelPrefab.givePrefab(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                // Vector3 zoomedSize = slideLocalizationData[id].labelList[i].ZoomedSize;
                go.transform.localScale = new Vector3(1f * scaleMultiplier, 1f * scaleMultiplier, 1f);

                
                labelElement le = go.GetComponent<labelElement>();

                le.fillWithData(slideLocalizationData[id].labelList[i].label, slideLocalizationData[id].labelList[i].labelAlignment
                    , enabledStatus);
                labels.Add(go);
            }
            return labels;
        }

        // this makes a portal 
        void makeJourney()
        {

        }


        // assiging the line material and label parent to the label script in the exp model and initiating it.
        void initiateExperienceModelLabels(bool alwaysShowLabels, GameObject go, List<GameObject> labels)
        {
            experienceLabeler expL = go.GetComponent<experienceLabeler>();

            if (expL != null)
            {
                expL.InitMe(lineScaler, originMarkerPrefab, alwaysShowLabels, labels, MainCamera, labelsParent, scaleAndMoveParent, lineMaterialCommon);
            }
            // expL.MyLabelsEnable(true);
        }

        // make a description ui element
        void makeDescriptionUIElement(GameObject parent, experienceLabelNarrationDataClass data)
        {
            GameObject go = Instantiate(descriptionPrefab.givePrefab(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            labelElement de = go.GetComponent<labelElement>();
            de.fillWithData(data.desciption, data.descriptionAlignment, data.isInfoDisplayed);
            go.transform.SetParent(parent.transform);
            go.transform.localPosition = data.descriptionPosition;
            descriptionList.Add(go);
        }

        public GameObject changeSlideMode(ExperienceSlideMode mode)
        {
            this.mode = mode;
            hideOrShowGameObjects(false, experienceModelList);
            return changeActiveSlide(0);
        }

        public GameObject changeActiveSlide(int id)
        {
            hideOrShowGameObjects(false, experienceModelList);
            experienceModelList[id].SetActive(true);
            return experienceModelList[id];
        }


        void hideOrShowGameObjects(bool state, List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetActive(state);
            }
        }

        public List<GameObject> getSlideList()
        {
            return experienceModelList;
        }

        public void cleanUpExperience()
        {
            for (int i = 0; i < allLabelsObjects.Count; i++)
            {
                Destroy(allLabelsObjects[i]);
            }

            for (int i = 0; i < experienceModelList.Count; i++)
            {
                Destroy(experienceModelList[i]);
            }
        }

    }
}
