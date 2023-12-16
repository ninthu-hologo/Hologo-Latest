using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 Aug 2019
    /// Modified by: 
    /// this class instantiates quiz models and keepts track
    /// </summary>
    public class quizInstantiator : MonoBehaviour
    {

        [Header("SCENE ELEMENTS")]
        [SerializeField]
        private Transform quizParent;
        [SerializeField]
        private Camera MainCamera;
        [SerializeField]
        private Material lineMaterialCommon;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private localizePrefab_SO quizLabelPrefab;
        [SerializeField]
        private localizePrefab_SO quizPickMeLabelPrefab;
        [SerializeField]
        private GameObject quizOriginPrefab;
        [SerializeField]
        List<GameObject> quizModelList;
        [SerializeField]
        List<GameObject> quizLabelList;
        List<GameObject> quizOriginList;

        public void MakeQuiz(quiz_SO quiz, List<localzationQuizSlide> quizSlides,float scaleMultiplier)
        {
            // check to see if a quiz exists for this experience
            if (quiz == null)
                return;

            quizModelList = new List<GameObject>();
            if (quiz.quizType == 1)
            {
                Debug.Log("quiz type match labels");
                // making label match quiz
                GameObject go = Instantiate(quiz.quizModels[0].model, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.position = quizParent.position;
                go.transform.rotation = quizParent.rotation;
                go.transform.SetParent(quizParent);
                go.transform.localScale = Vector3.one;
                // go.SetActive(false);
                quizModelList.Add(go);

                makeLabels(quizSlides[0].quizlabels, scaleMultiplier);

                quizlabeler ql = go.GetComponent<quizlabeler>();
                if (ql != null)
                    ql.initQuizLabels(quizOriginPrefab,quizLabelList, MainCamera, quizParent, lineMaterialCommon);
            }
            else if(quiz.quizType == 2)
            {
                // making pick the right one quiz
                Debug.Log("quiz type pick me");
                for (int i = 0; i < quiz.quizModels.Count; i++)
                {
                    GameObject go = Instantiate(quiz.quizModels[i].model, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.position = quizParent.position;
                    go.transform.rotation = quizParent.rotation;
                    go.transform.SetParent(quizParent);
                    quizModelList.Add(go);
                }

                makePickMeLabels(quizSlides[0].quizlabels, scaleMultiplier);
                SetupPickMeQuiz();
            }

        }

        public void setQuizModelActiveState(bool state)
        {
            quizParent.gameObject.SetActive(state);
        }

        public GameObject giveQuizModel()
        {
            return quizModelList[0];
        }

        #region QUIZ TYPE LABEL MATCH
        // getting labels from localized data and instantiating them .
        List<GameObject> makeLabels(List<localizedSlide> labelList,float scaleMultiplier)
        {
            quizLabelList = new List<GameObject>();
            for (int i = 0; i < labelList.Count; i++)
            {
                GameObject go = Instantiate(quizLabelPrefab.givePrefab(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
               // Vector3 zoomedSize = labelList[i].ZoomedSize;
                go.transform.localScale = new Vector3(1f * scaleMultiplier, 1f * scaleMultiplier, 1f);

                bool enabledStatus = !string.IsNullOrEmpty(labelList[i].label);

                labelElement le = go.GetComponent<labelElement>();


                le.fillWithData(labelList[i].label, labelList[i].labelAlignment, enabledStatus);

                quizLabelList.Add(go);
                go.SetActive(false);
            }
            return quizLabelList;
        }

        public bool doesCountMatch(int count)
        {
            return count == getTheNumberOfMatches();
        }

        public void matchMadeResult(int id)
        {
            quizlabeler ql = quizModelList[0].GetComponent<quizlabeler>();
            ql.startSelectedLabelFollow(id);
        }

        public void wrongMatchMadeResult()
        {
            quizlabeler ql = quizModelList[0].GetComponent<quizlabeler>();
            ql.labelsBackToSpawnPoint();
        }

        public void reSuffleQuiz()
        {
            quizlabeler ql = quizModelList[0].GetComponent<quizlabeler>();
            ql.reShuffleQuiz();
        }

        public int getTheNumberOfMatches()
        {
            quizlabeler ql = quizModelList[0].GetComponent<quizlabeler>();
            return ql.getTotalNumberOfMatches();
        }
        #endregion


        #region QUIZ TYPE PICK THE RIGHT ONE
        List<GameObject> makePickMeLabels(List<localizedSlide> labelList, float scaleMultiplier)
        {
            quizLabelList = new List<GameObject>();

            for (int i = 0; i < labelList.Count; i++)
            {
                bool enabledStatus = !string.IsNullOrEmpty(labelList[i].label);
                GameObject go = Instantiate(quizPickMeLabelPrefab.givePrefab(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
              //  Vector3 zoomedSize = labelList[i].ZoomedSize;
                go.transform.localScale = new Vector3(1f* scaleMultiplier, 1f * scaleMultiplier, 1f);
                labelElement le = go.GetComponent<labelElement>();

                le.fillWithData(labelList[i].label, labelList[i].labelAlignment,enabledStatus);

                quizLabelList.Add(go);
               // go.SetActive(false);
            } 
            return quizLabelList;
        }


        public void SetupPickMeQuiz()
        {
            quizPickMeLabeler qpml = quizModelList[0].GetComponent<quizPickMeLabeler>();

            List<twoTransforms> trans = qpml.getSpawnPoints();
            for (int i = 1; i < quizModelList.Count; i++)
            {
                quizModelList[i].transform.SetParent(trans[i-1].spawnPoint);
                quizModelList[i].transform.localScale = Vector3.one;
                quizModelList[i].transform.localPosition = Vector3.zero;
                quizLabelList[i-1].transform.SetParent(trans[i-1].labelPoint);
              //  quizLabelList[i-1].transform.localScale = Vector3.one;
                quizLabelList[i-1].transform.localPosition = Vector3.zero;
            }
        }

        #endregion

    }
}
