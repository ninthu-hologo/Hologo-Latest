using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 4 july 2019
    /// Modified by: 
    /// this manages teacher recorded lessons
    /// </summary>
    public class recordedLessonsManager : messageLogging
    {

        [SerializeField]
        lessonsList_SO recordedLessonList;
        [SerializeField]
        List<audioClipDetail> serverRecordedLessonList;
        [SerializeField]
        dataPaths_SO dataPath;
        [SerializeField]
        settings_SO mainSettings;
        [SerializeField]
        userData_SO userData;



        #region TEACHERS
        /// <summary>
        /// loads all the lessons recorded from storage
        /// </summary>
        /// <returns></returns>
        public bool loadRecordedLessonList()
        {
            Debug.Log("is run");
            bool success = false;
            if (recordedLessonList.isLessonListFilled())
                return true;

            if (checkIfRecordedLessonListExitsExists())
            {
                lessonsServerConnect lsc = new lessonsServerConnect();
                string result = lsc.getUserRecordedLessonsFromStorage(dataPath.getFolder(0), dataPath.getFileName(0));
                if (string.IsNullOrEmpty(result))
                    return false;

                jsonHelper.DeserializeJsonToScriptableObject(result, recordedLessonList);
                success = true;
            }
            else
            {
                // no recorded data exists
                success = false;
            }
            return success;
        }

        //TODO: implement the method
        // load recordedlessons in server if there is any
        public async Task<bool> getRecordedLessonListFromServer()
        {
            bool success = false;
            lessonsServerConnect lsc = new lessonsServerConnect();
            string url = dataPath.getUrl(3) + "?token=" + userData.requestMyToken();
            Debug.Log(url);
            string result;

            var task = lsc.getRecorededLessonListFromServer(url);
            try
            {
                result = await task;
                Debug.Log(result);

                GenericObjectAndStatus<HologoWebAPIGeneric<List<RecordedLessonDetail>>> lessonsAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<RecordedLessonDetail>>>(result);

                if (lessonsAndStatus.GenericObject.success)
                {
                    success = true;
                    convertServerLessonsToAudioClipList(lessonsAndStatus.GenericObject.data);
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


        public async Task<bool> downloadUploadedLesson(audioClipDetail clip)
        {
            bool success = false;
            lessonsServerConnect lsc = new lessonsServerConnect();
            string url = dataPath.getUrl(7) + clip.lessonDetailFromServer.id + "/?token=" + userData.requestMyToken();
            Debug.Log(url);
            string fileName = System.DateTime.Now.ToString("MMddyyyyHHmmss") + clip.lessonDetailFromServer.id + ".wav";
            var task = lsc.downloadRecordedLesson(url, dataPath.getFolder(0), fileName);
            try
            {
                success = await task;
                if (success)
                {
                    clip.FileName = fileName;
                    saveLessonList();
                }
            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }


            return success;
        }


        void convertServerLessonsToAudioClipList(List<RecordedLessonDetail> lessons)
        {
            serverRecordedLessonList = new List<audioClipDetail>();

            for (int i = 0; i < lessons.Count; i++)
            {
                audioClipDetail acd = new audioClipDetail();
                acd.Title = lessons[i].title;
                acd.ExperienceId = lessons[i].localized_experience_id;
                acd.lessonDetailFromServer = lessons[i];
                acd.IsUploaded = true;
                acd.LessonId = lessons[i].id;
                serverRecordedLessonList.Add(acd);
            }

            recordedLessonList.setServerLessonList(serverRecordedLessonList);
            combineRecordedListFromInternetAndStorage();
            saveLessonList();
        }




        //copy over unique entries in server lesson list to storage lesson list
        public void combineRecordedListFromInternetAndStorage()
        {
            if (recordedLessonList.getServerLessonList().Count <= 0)
                return;

            for (int a = 0; a < recordedLessonList.getServerLessonList().Count; a++)
            {
                recordedLessonList.addIfServerUnique(recordedLessonList.getServerLessonList()[a]);
            }

            saveLessonList();
        }

        public List<audioClipDetail> getRecordedLessonList()
        {
            return recordedLessonList.getlessonList();
        }

        public List<audioClipDetail> getServerRecordedLessonList()
        {
            return recordedLessonList.getServerLessonList();
        }

        // returing a list of audio clips belonging to the current experience
        public List<audioClipDetail> getfilteredRecordedListForCurrentExperience()
        {
            if (!recordedLessonList.isLessonListFilled())
                return null;

            List<audioClipDetail> tempRecordedLessonList = recordedLessonList.filterAudioClipsToCurrentExperience(mainSettings.currentExperience.id);

            if (tempRecordedLessonList.Count <= 0)
                return null;

            return tempRecordedLessonList;
        }

        public audioClipDetail createRecordedLessonDetail(string FileName, float lenght)
        {
            audioClipDetail currentAudioClipDetail = new audioClipDetail();
            currentAudioClipDetail.FileName = FileName;
            currentAudioClipDetail.Duration = lenght;
            currentAudioClipDetail.ExperienceId = mainSettings.currentExperience.id;
            string title = mainSettings.currentExperience.title + " -custom lesson from :" + userData.requestUserDetail().name;
            string description = mainSettings.currentExperience.body;
            currentAudioClipDetail.Title = title;
            currentAudioClipDetail.Description = description;
            currentAudioClipDetail.LessonId = recordedLessonList.getNewId();
            return currentAudioClipDetail;
        }

        public void addRecordedLessonToList(audioClipDetail clip)
        {
            Debug.Log("step three > lesson added");
            recordedLessonList.addIfUnique(clip);
            saveLessonList();
        }

        public void updateRecordedLesson(audioClipDetail clip)
        {
            recordedLessonList.updateLesson(clip);
            saveLessonList();
        }

        public void addOldAudioLessonToList(audioClipDetail clip)
        {
            recordedLessonList.addIfUnique(clip);
        }


        public void removeAndDeleteLessonFromList(audioClipDetail clip)
        {
            readWriteData.DeleteFileOnDisk(dataPath.getFolder(0), clip.FileName);
            recordedLessonList.removeLesson(clip);
            saveLessonList();
        }

        public void removeLessonFromServerList(audioClipDetail clip)
        {
            readWriteData.DeleteFileOnDisk(dataPath.getFolder(0), clip.FileName);
            recordedLessonList.removeServerLesson(clip);
            saveLessonList();
        }

        public async Task<bool> deleteLessonFromServer(audioClipDetail clip)
        {
            bool success = false;

            lessonsServerConnect lsc = new lessonsServerConnect();
            string url = dataPath.getUrl(5) + clip.lessonDetailFromServer.id + "?token=" + userData.requestMyToken();
            Debug.Log(url);
            string result;
            var task = lsc.deleteLessonOnServer(url);
            try
            {
                result = await task;
                GenericObjectAndStatus<HologoAPI> deleteStatus =
                   jsonHelper.DeserializeFromJson<HologoAPI>(result);

                if (deleteStatus.GenericObject.success)
                {
                    success = true;
                }
                Debug.Log(result);
            }
            catch (System.Exception ex)
            {
                createMessage(ex.Message);
            }

            return success;
        }

        #endregion

        // getting the filename of the last recorded clip of the loaded experience if it exists
        public audioClipDetail getLatestRecordedClip()
        {
            List<audioClipDetail> temp = getfilteredRecordedListForCurrentExperience();

            if (temp == null)
                return null;


            return temp[temp.Count - 1];
        }

        public AudioClip testClip;


        #region HELPERS


        public int getNewLessonId()
        {
            return recordedLessonList.getNewId();
        }

        /*{"success":true,
         * "data":
         * {"title":"Root Hair Cell -custom lesson from :duku",
         * "private":"2",
         * "experience_id":96,
         * "localized_experience_id":427,
         * "country_id":1,
         * "language_id":6,
         * "syllabus_id":11,
         * "curriculum_id":201,
         * "user_id":26117,
         * "updated_at":"2019-09-10 07:03:22","created_at":"2019-09-10 07:03:22","id":3},
         * "message":"Lesson Uploaded successfully"}
        */
        public async Task<bool> uploadRecordedLesson(audioClipDetail clip, bool publicShare)
        {

            bool success = true;
            lessonsServerConnect lsc = new lessonsServerConnect();
            int p;
            if (publicShare)
            {
                p = 2;
            }
            else
            {
                p = 1;
            }


            // string filepath = readWriteData.GetPath(dataPath.getFolder(0), clip.FileName);
            //// filepath = System.Uri.EscapeUriString(filepath);
            // Debug.Log(filepath);
            // byte[] myData = System.IO.File.ReadAllBytes(filepath);
            // Debug.Log("my data lenght :" + myData.Length);

            string url = dataPath.getUrl(6) + "?token=" + userData.requestMyToken();

            System.Tuple<bool, bool, string> tuple = new System.Tuple<bool, bool, string>(false, false, "");

            var task = lsc.uploadRecordedLessons(clip, p, url, dataPath.getFolder(0));

            try
            {
                tuple = await task;

                GenericObjectAndStatus<HologoWebAPIGeneric<RecordedLessonDetail>> mystatusUpload =
               jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<RecordedLessonDetail>>(tuple.Item3);

                if (tuple.Item1 && tuple.Item2)
                {
                    clip.IsUploaded = true;
                    clip.lessonDetailFromServer = mystatusUpload.GenericObject.data;
                    saveLessonList();
                }

            }
            catch (System.Exception ex)
            {
                Debug.Log("error");
            }


            if (!tuple.Item1)
            {
                createMessage("something wrong with the internet");
                success = tuple.Item1;
                return success;
            }

            if (!tuple.Item2)
            {
                success = tuple.Item2;
                createMessage("you dont have any quota to upload");
            }
            return success;
        }

        //// download audio clip from server 
        //public async Task<bool> downloadAudioClip(string fileName)
        //{
        //    lessonsServerConnect lsc = new lessonsServerConnect();
        //    return await lsc.downloadRecordedLesson(dataPath.getUrl(0), dataPath.getFolder(0), fileName);
        //}

        // load audioclip to play
        public async Task<AudioClip> loadRecordedLesson(string fileName)
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            return await lsc.loadRecordedLessonFromStorage(dataPath.getFolder(0), fileName);
        }


        // helper functions
        bool checkIfRecordedLessonListExitsExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        public bool saveLessonList()
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            return lsc.writeRecordedLessonListDataToStorage(recordedLessonList, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // generating a file name for the recorded lesson using the experience name and data time stamp
        public string getFileNameForLessonRecord()
        {
            string experienceName = mainSettings.currentExperience.title;
            string dateTime = System.DateTime.Now.ToString("MMddyyyyHHmmss");
            return experienceName + dateTime + ".wav";
        }

        public string getFolderNameForLessonRecord()
        {
            return dataPath.getFolder(0);
        }

        #endregion


#if UNITY_EDITOR
        public void SaveLessonListForEditor()
        {
            saveLessonList();
        }
#endif
    }
}
