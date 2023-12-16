using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class connects to server/storage and downloads lesson list . and also lesson audio
    /// </summary>
    public class lessonsServerConnect
    {

        // getting a list lessons uploaded to the server by the user a string is returned
        public async Task<string> getRecorededLessonListFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url,false);
        }

        // getting a list lessons that belongs to the selected experience
        public async Task<string> getSelectedExpRecorededLessonListFromServer(string url,int lanId, int expId)
        {
            Debug.Log("lanId" + lanId + " expid " + expId);
            WWWForm signUpForm = new WWWForm();
            signUpForm.AddField("language_id", lanId);
            signUpForm.AddField("experience_id", expId);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, signUpForm);
          
        }

        // readfile from storage
        public string getUserRecordedLessonsFromStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        public async Task<string> availableLessonsForSelectedExperience(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        //
        public async Task<string> deleteLessonOnServer(string url)
        {
            return await InternetfetchData.deleteDataFromServer(url);
            
        }

        // return true if the file was downloaded
        // used for student lesson download
        // used for teacher retrive uploaded lesson
        public async Task<bool> downloadRecordedLesson(string url, string folderName, string fileName)
        {
            return await downloadHelper.downloadToStorage(url, folderName, fileName,false);

        }

        // load recorded lesson from storage
        public async Task<AudioClip> loadRecordedLessonFromStorage(params string[] pathStrings)
        {
            AudioClip myClip = null;
            string filepath = "file://" + readWriteData.GetPath(pathStrings);
            if (readWriteData.CheckIfFileExists(pathStrings))
            {
                filepath = System.Uri.EscapeUriString(filepath);
                UnityWebRequest audioLoader = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.WAV);
                await audioLoader.SendWebRequest();
                myClip = DownloadHandlerAudioClip.GetContent(audioLoader);
            }
            else
            {
                return null;
            }

            return myClip;
        }

        // return true if the recorded lesson was uploaded
        // this is returning a tuple checking to see if internet connection was ok and user had quota to upload
        public async Task<System.Tuple<bool, bool,string>> uploadRecordedLessons(audioClipDetail clip, int publicShare, string url, string folderName)
        {
            Debug.Log(url);
            WWWForm postForm = new WWWForm();
            postForm.AddField("localized_experience_id", clip.ExperienceId);
            postForm.AddField("title", clip.Title);
            postForm.AddField("closed", publicShare);

            return await uploadHelper.uploadFileToServerWithForm(postForm, url, "lesson_audio_file", clip.FileName, folderName, "audio/x-wav");
        }


        // writing data to storage
        public bool writeRecordedLessonListDataToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson,checkFileExits, pathStrings);
        }
    }
}
