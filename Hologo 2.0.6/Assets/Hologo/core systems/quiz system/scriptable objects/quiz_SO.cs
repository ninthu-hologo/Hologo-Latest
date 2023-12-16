using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// this scriptable object is main quiz base for experience
    /// </summary>
    [CreateAssetMenu(fileName = "quiz data.asset", menuName = "Hologo V2/new quiz data")]
    public class quiz_SO : ScriptableObject
    {
        [Header("QUIZ NAME")]
        public string quizName;
        [Header("TYPE OF QUIZ")]
        [Tooltip("1 for label connectuon quiz and 2 for pick me quiz")]
        public int quizType = 0; // 0 = label connect quiz , 1 = pick the right one
        [Header("LIST OF QUIZ MODELS")]
        [Tooltip("for pic me add models and a one label for title")]
        public List<quizModel> quizModels;



       
    }

    [System.Serializable]
    public class quizModel
    {
        [Header("MODEL")]
        public GameObject model;
        public Vector3 modelLocalPostiton;
        //[Header("LIST OF LABELS")]
        //public List<quizLabelClass> labels;
    }


    // struct to contain a label and its origin and target to follow gameobjects
    [System.Serializable]
    public class quizLabelClass
    {
        public string title;
        // the point where the origin of the label is . ie part of the model
        public GameObject Origin;
        // the target that the lable point should follow
        public GameObject Target;
        [HideInInspector]
        public GameObject LabelPoint;
        [HideInInspector]
        public bool followTarget;


        public void hideThis()
        {
            Origin.SetActive(false);
            Target.SetActive(false);
           
        }
    }
}