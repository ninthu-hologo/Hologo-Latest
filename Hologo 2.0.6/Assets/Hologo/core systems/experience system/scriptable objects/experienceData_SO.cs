using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// this scriptable object is main data holder for experience
    /// </summary>
    [CreateAssetMenu(fileName = "experience data.asset", menuName = "Hologo V2/Experience Asset/new experience data")]
    public class experienceData_SO : ScriptableObject
    {
        #region Public Variables
        [Header("EXPERIENCE TITLE")]
        public string modelName;
        public string description;
        //public int modelId;
        //public Sprite modelIcon;
        ////model image
        //public Sprite modelImage;
        [Header("EXPERIENCE MODES")]
        public bool isLearnModeEnabled =true;
        public bool isExploreModeEnabled =true;
        public bool isDescriptionEnabled =true;
        public bool learnLoopMode =true;
        [Header("EXPERIENCE TYPES")]
        [Tooltip("1 normal experience and 2 for portal/journeys")]
        public int modelType = 1; // type 1 = experience , type 2 = portal/journeys , etc
        [Header("AR CONFIG")]
        public bool enableShadow = true;
        public bool reflectionTexturing;
        public bool customLighting = false;
       // public bool realWorldScale;
        public float objectModeScale =1f;
        public Vector3 objectLocation = new Vector3(0, -0.898f, 2.56f);
        public float arModeScale =1f;
        
       // public UnityARPlaneDetection DetectionPlane = UnityARPlaneDetection.Horizontal;
        //setting to a default gray
        public Color backgroundColor = new Color(0.32f, 0.36f, 0.40f);
        public bool isArEnabled = true;
        public float shadowPlanScale = 1f;
        public float shadowPlanHeight = 1f;
        // public float shadowSize = 1.5f; //othographic size
        // public float shadowNear = 3.35f; //near clip
        // public float shadowFar = 5.84f; //far clip
        public float minScaleLimit = 0.2f;
        public float maxScaleLimit = 70f;


        // list of the model details( main model, sections etc...)
        [Header("EXPERIENCE MODELS")]
        public List<slideDetails> Models;
        [Header("QUIZ")]
        public quiz_SO quiz;


        public bool isDatafilled()
        {
            return Models.Count > 0;

        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// This class groups a model and its highlighe details
    /// </summary>
    [System.Serializable]
    public class slideDetails
    {
        public string name;
        public GameObject modelSlide;
        public bool isInLearnMode;
        public bool isInExploreMode;
        public List<highLightDetail> modelSlideHighLightDetails;
        [HideInInspector]
        public bool isInfoDisplayed;
        [HideInInspector]
        public bool isNarrationEnabled;
        [HideInInspector]
        public AudioClip narration;
        public int id;

    }



    /// <summary>
    /// this class group together a highlight gameobject , name and description.
    /// </summary>
    [System.Serializable]
    public class highLightDetail
    {
        public string highlightName;
        public string highlightDescription;
        public bool isInfoDisplayed = false;
        public GameObject highlightSelector;
    }

    #endregion

    public enum HologoArDetectionPlane
    {
        None = 0,
        Horizontal = (1 << 0),
        Vertical = (1 << 1),
        HorizontalAndVertical = (1 << 1) | (1 << 0)
    }
}
