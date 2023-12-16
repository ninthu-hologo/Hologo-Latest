using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
using System.IO;
using System;

namespace Hologo2.UpdateApp

{


    public class oldAppUpdater : MonoBehaviour
    {
        [SerializeField]
        userManager uManager;
        [SerializeField]
        recordedLessonsManager rManager;
        UserData oldUserData;
        Hologo2.UserData newUserData;
        

        private bool userIsUpdated = false;
        private bool isTeacher = false;

        public void CleanOldApp()
        {
            updateOldUserToNewUser();

            // if(isTeacher) // hamid 1st nov 2019 not updating all teacher recorded lessons when updating to new app
            // {
            //     updateRecordedLessons();
            //     rManager.saveLessonList();
            // }
            // save recorded data.
            clearOldData();
            // save user data after old data is cleared
            if (userIsUpdated)
                uManager.updateUser(newUserData);
            
        }


        void updateOldUserToNewUser()
        {
            if (readWriteData.CheckIfFileExists("hcg", "hud.dtm"))
            {
                string result = readWriteData.ReadFileFromDisk("hcg", "hud.dtm");

                GenericObjectAndStatus<UserData> uds = jsonHelper.DeserializeFromJson<UserData>(result);
                if (uds.Success)
                {
                    Debug.Log("old app user is being updated");
                    oldUserData = uds.GenericObject;
                    // we convert old user to a new user
                    newUserData = new Hologo2.UserData();
                    newUserData.user = new Hologo2.UserDetail();

                    newUserData.user.id = oldUserData.data.id;
                    newUserData.user.name = oldUserData.data.name;
                    newUserData.user.email = oldUserData.data.email;
                    newUserData.user.password = oldUserData.data.password;
                    newUserData.user.IsSignedIn = oldUserData.data.IsSignedIn;
                    if (uds.GenericObject.data.user_type_id == 1)
                    {
                        Debug.Log("old user is a teacher");
                        newUserData.user.user_type = "teacher";
                        isTeacher = true;
                    }
                    else
                    {
                        Debug.Log("old user is a student");
                        newUserData.user.user_type = "student";
                    }
                    userIsUpdated = true;
                    newUserData.user.user_type_id = uds.GenericObject.data.user_type_id;
                  
                }
            }
            else
            {
                Debug.Log("no old user is being updated");
                // no need to go further
            }

        }



        void updateRecordedLessons()
        {
            AudioSavedCollection audioSavedCollection = new AudioSavedCollection();
            if (readWriteData.CheckIfFileExists("hlss", "hlr.dtm"))
            {
                string result = readWriteData.ReadFileFromDisk("hlss", "hlr.dtm");

                // catching any json parse errors and handling it

                GenericObjectAndStatus<AudioSavedCollection> recordedLessonsAndStatus = jsonHelper.DeserializeFromJson<AudioSavedCollection>(result);

                if (recordedLessonsAndStatus.Success)
                {
                    audioSavedCollection = recordedLessonsAndStatus.GenericObject;
                }

                if(audioSavedCollection.AudioClipList.Count>0)
                {
                    // make new collection
                    for (int i = 0; i < audioSavedCollection.AudioClipList.Count; i++)
                    {
                        Debug.Log("converting audio clip >" + audioSavedCollection.AudioClipList[i].Title);
                        audioClipDetail acd = new audioClipDetail();
                        acd.FileName = audioSavedCollection.AudioClipList[i].FileName;
                        acd.ExperienceId = audioSavedCollection.AudioClipList[i].ExperienceId;
                        acd.Title = audioSavedCollection.AudioClipList[i].Title;
                        acd.Description = audioSavedCollection.AudioClipList[i].Description;
                        acd.isPrivateShare = audioSavedCollection.AudioClipList[i].StudentShare;
                        acd.LessonId =rManager.getNewLessonId();
                        acd.User = audioSavedCollection.AudioClipList[i].User;
                        acd.Duration = audioSavedCollection.AudioClipList[i].Duration;
                        rManager.addOldAudioLessonToList(acd);

                    }
                }

                readWriteData.DeleteFileOnDisk("hlss", "hlss.dtm");
            }
           
        }


        void clearOldData()
        {
            List<string> folders = new List<string>();
            folders.Add("hab");
            folders.Add("hcg");
            folders.Add("hsp");
            folders.Add("hlss"); // hamid 1st nov 2019 deleting all teacher recorded lessons when updating to new app
            // if(!isTeacher)
            // {
            //     folders.Add("hlss");
            // }

            for (int i = 0; i < folders.Count; i++)
            {
                if (Directory.Exists(readWriteData.GetPath(folders[i])))
                {
                    string[] files = Directory.GetFiles(readWriteData.GetPath(folders[i]));
                    if (files.Length == 0)
                        break;

                    for (int j = 0; j < files.Length; j++)
                    {
                        Debug.Log(files[j] + " is deleted!");
                        File.Delete(files[j]);
                    }
                }
                else
                {
                    Debug.Log(" no directory > " + folders[i]);
                }

            }
        }


    }


    // this class is the container for user details from json
    [Serializable]
    public class UserDetail
    {
        //fields from server
        public int id;
        public string name;
        public string email;
        public string password;
        public int user_type_id; // teacher or student or anonymous
        public int email_token;
        public int status;
        public int profile_id;
        //app only fields
        public bool IsSignedIn;
        public bool IsSubscribed;
        public bool IsUnderTeacher;

    }

    // class to hold the user authentication from json
    [Serializable]
    public class UserAuthenticate
    {
        public UserDataAuth data;
    }



    // class to hold the user authentication from json
    [Serializable]
    public class UserDataAuth
    {
        public string token;
        public UserDetail user;
    }

    [Serializable]
    public class UserData
    {
        public string token;
        public UserDetail data;
        public UserProfile profile;
    }

    [Serializable]
    public class UserProfile
    {
        public int id;
        public int user_id;
        public string full_name;
        public string address;
        public string mobile_number;
        public string company;

    }


    // for storing teacher details
    [Serializable]
    public class UserRemote : IdataObject
    {
        public int id;
        public string identifier;
        public string subject_name;
    }



    [Serializable]
    public class AudioClipDetail : IdataObject
    {
        public string FileName;
        public float Duration;
        public string Title;
        public string Description;
        public bool IsUploaded;
        public int ExperienceId;
        public string User;
        public bool StudentShare;
        public bool PublicShare;
    }


    [Serializable]
    public class AudioSavedCollection
    {
        #region Public Variables
        public List<AudioClipDetail> AudioClipList;
        #endregion
    }



}