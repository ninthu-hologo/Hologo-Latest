using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// this class contains data object for labels and audio clip
    /// </summary>
    [System.Serializable]
    public class experienceLabelNarrationDataClass
    {
        public string slideName;
        public string localizedName;
        [Header("DESCRIPTION")]
        public string desciption;
        public bool isInfoDisplayed;
        public Vector3 descriptionPosition;
        public labelAlignment descriptionAlignment;
       // public Vector2 descriptionCanvasSize;
        [Header("LABELS")]
        public bool labelsExist = false;
        public List<localizedSlide> labelList;
        [Header("NARRATION")]
        public bool isNarrationEnabled;
        public AudioClip Narration;

    }

    [System.Serializable]
    public class localizedSlide
    {
        // for english title . ease in translation
        public string labelName;
        // actual label. that needs to be translated. if this is left out empty
        // then label wont be generated
        public string label;
        // description
        public string description;
        // label alignment
        public labelAlignment labelAlignment;
        // zoom
        //public Vector3 ZoomedSize = new Vector3(1,1,1);
    }

    [System.Serializable]
    public class localzationQuizSlide
    {
        public string title;
        public string LocaliziedTitle;
        public List<localizedSlide> quizlabels;
    }

}
