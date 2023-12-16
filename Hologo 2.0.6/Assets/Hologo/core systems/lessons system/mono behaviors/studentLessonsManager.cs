using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// this class manages student recorded lessons
    /// </summary>
    public class studentLessonsManager : messageLogging
    {

        [SerializeField]
        studentLessons_SO studentLessonList;
        [SerializeField]
        dataPaths_SO dataPath;
        [SerializeField]
        settings_SO mainSettings;
        [SerializeField]
        userData_SO userData;

        [SerializeField]
        private List<RecordedLessonDetail> lessonsFromServer;

        public LessonDownloadClass CurrentLesson;



        // load student lessons from storage
        public bool loadStudentLessonListFromStorage()
        {
            bool success = false;
            if (studentLessonList.isDataFilled())
                return true;

            if (checkIfStudentLessonListExitsExists())
            {
                lessonsServerConnect lsc = new lessonsServerConnect();
                string result = lsc.getUserRecordedLessonsFromStorage(dataPath.getFolder(0), dataPath.getFileName(0));
                if (string.IsNullOrEmpty(result))
                    return false;

                jsonHelper.DeserializeJsonToScriptableObject(result, studentLessonList);
                success = true;
            }
            else
            {
                // no recorded data exists
                success = false;
            }
            return success;
        }



        // download student audio clip from server 
        public async Task<bool> downloadAudioClip(RecordedLessonDetail lesson)
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            bool success = false;
            string url = dataPath.getUrl(2) +lesson.id + "?token="+userData.requestMyToken();
            lesson.fileName = mainSettings.currentExperience.experience.asset_bundles[0].title + ".wav";
            success = await lsc.downloadRecordedLesson(url, dataPath.getFolder(0), lesson.fileName);

            if (success)
            {
                studentLessonList.addStudentLessonToTheList(lesson);
                saveStudentLessonList();
            }

            return success;

        }

        // load audioclip to play
        public async Task<AudioClip> loadStudentLesson(string fileName)
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            return await lsc.loadRecordedLessonFromStorage(dataPath.getFolder(0), fileName);
        }

        // getting the file name of lesson for the loaded experience
        public RecordedLessonDetail getTheLoadedExperiencesLesson()
        {
            return studentLessonList.getLesson (mainSettings.currentExperience.experience_id);
        }




        // method gets all the lessons recorded for the current experience. private and public
        public async Task<bool> getStudentLessonsFromServerForLoadedExperience()
        {
            bool success = false;
            lessonsServerConnect lsc = new lessonsServerConnect();
            // making up the url
            string url = dataPath.getUrl(0) + "?token="+ userData.requestMyToken();
            Debug.Log(url);
            string result = "";
            // making task
            var task = lsc.getSelectedExpRecorededLessonListFromServer(url, mainSettings.language.id, mainSettings.currentExperience.experience_id);

            try
            {
                // awaiting for the task to finish
                result = await task;
                Debug.Log(result);
                // try to deserialize into json the rest api gotton from server
                GenericObjectAndStatus<HologoWebAPIGeneric<List<RecordedLessonDetail>>> lessonsAndStatus =
                        jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<RecordedLessonDetail>>>(result);

                // check to see if rest api was success full
                if (lessonsAndStatus.GenericObject.success)
                {
                    lessonsFromServer = lessonsAndStatus.GenericObject.data;
                    //lessonsFromServer = new List<LessonDownloadClass>();
                    //// check to see if there are in fact any lessons

                    success = true;
                    //success = AllLessons.private_lesson.Count > 0 || AllLessons.public_lesson.Count > 0;

                    //lessonsFromServer = lessonsAndStatus.GenericObject.data.public_lesson;
                    //lessonsFromServer.AddRange(lessonsAndStatus.GenericObject.data.private_lesson);
                }
                else
                {
                    createMessage("error");
                }

            }
            catch (HologoInternetException ex)
            {

                createMessage(ex.Message);
            }

            return success;

        }

        // returning the list of lessons from server
        public List<RecordedLessonDetail> getStudentLessonsList()
        {
            return lessonsFromServer;
        }

        //checking to see if the lessons data object on storage exists
        private bool checkIfStudentLessonListExitsExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(1));
        }
        // saving lessons data object to storage
        public bool saveStudentLessonList()
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            return lsc.writeRecordedLessonListDataToStorage(studentLessonList, false, dataPath.getFolder(0), dataPath.getFileName(1));
        }
    }
}


