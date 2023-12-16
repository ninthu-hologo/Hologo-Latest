using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 july 2019
    /// Modified by: Shariz 13/9/2019
    /// class that localizes the ui of lessons record/play and download for teacher and students
    /// </summary>
    public class lessonsUIConnect : MonoBehaviour
    {

        [Header("UI ELEMENTS")]
        [SerializeField]
        private localizePrefab_SO rLessonUIElement;
        [SerializeField]
        private localizePrefab_SO sLessonUIElement;
        [SerializeField]
        private arSwitcher lessonUIElementParent; //Shariz 15th September 2019 v2
        [SerializeField]
        private arSwitcher lessonCanvas; //Shariz 15th September 2019 v2
        [SerializeField]
        private List<GameObject> lessonGameObjects;
        [SerializeField]

        #region TEACHER RECORDED LESSONS MAKE
        // makes recorded lessons buttons ui for the loaded experience
        public void makeLessonSmartButtons(List<audioClipDetail> clipDataList, params CallbackGameObject[] actions)
        {
            deleteUIElements(lessonGameObjects);
            for (int i = 0; i < clipDataList.Count; i++)
            {
                GameObject go = Instantiate(rLessonUIElement.givePrefab());
                recordedLessonUIElement iui = go.GetComponent<recordedLessonUIElement>();
                iui.fillWithData(clipDataList[i],i, actions);
                go.transform.SetParent(lessonUIElementParent.GetlessonUIElementParent()); //Shariz 15th September 2019 v2
                go.transform.localScale = new Vector3(1, 1, 1);
              lessonGameObjects.Add(go);
            }
        }

        #endregion

        public void makeStudentLessonButtons(List<RecordedLessonDetail> dataList, params CallbackGameObject[] actions)
        {
            deleteUIElements(lessonGameObjects);
            for (int i = 0; i < dataList.Count; i++)
            {
                GameObject go = Instantiate(sLessonUIElement.givePrefab());
                studentLessonUIElement slu = go.GetComponent<studentLessonUIElement>();
                slu.fillWithData(dataList[i], actions);
                go.transform.SetParent(lessonUIElementParent.GetlessonUIElementParent()); //Shariz 15th September 2019 v2
                go.transform.localScale = new Vector3(1, 1, 1);
                lessonGameObjects.Add(go);
            }

        }


        #region GENERAL FUNCTIONS
        void deleteUIElements(List<GameObject> gameObjects)
        {
            if (gameObjects == null)
                gameObjects = new List<GameObject>();

            if (gameObjects.Count <= 0)
                return;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                Destroy(gameObjects[i]);
            }

            gameObjects.Clear();
            gameObjects = new List<GameObject>();
        }

        public void enableLessonCanvas()
        {
            lessonCanvas.GetlessonCanvas().SetActive(true); //Shariz 15th September 2019 v2
        }

        public void disableLessonCanvas()
        {
            lessonCanvas.GetlessonCanvas().SetActive(false); //Shariz 15th September 2019 v2
        }
        #endregion

    }
}
