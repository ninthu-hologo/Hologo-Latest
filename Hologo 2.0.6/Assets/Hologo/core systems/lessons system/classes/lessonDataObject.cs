using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{

    [System.Serializable]
    public class audioClipDetail : IdataObject, IEquatable<audioClipDetail>
    {
        public string FileName;
        public float Duration;
        public string Title;
        public string Description;
        public bool IsUploaded;
        public int ExperienceId;
        public string User;
        public bool isPrivateShare;
        public int LessonId;

        public RecordedLessonDetail lessonDetailFromServer;

        public bool Equals(audioClipDetail other)
        {
            if (this.LessonId == other.LessonId)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public void Update(audioClipDetail other)
        {

        }
        // language and country details
    }

    /// <summary>
    /// incoming teacher recorded lessons
    /// </summary>
    [System.Serializable]
    public class RecordedLessonDetail :IdataObject, IEquatable<RecordedLessonDetail>
    {
        public int id;
        public string title;
        public int experience_id;
        public int closed;
        public int country_id;
        public int language_id;
        public int syllabus_id;
        public int curriculum_id;
        public int localized_experience_id;
        public int user_id;
        public bool approved;
        public string fileName;


        public bool Equals(RecordedLessonDetail other)
        {
            if (this.id == other.id)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    /// Created by: Hamid - 13-11-2017
    /// Modified by:
    /// Data Object Script for Incoming lesson data from server
    /// </summary>
    [System.Serializable]
    public class LessonDetail : IdataObject
    {
        #region Public Variables
        public string LessonName;
        public int LessonId;
        public string FileName;
        public float Duration;
        public bool MyTeacher;
        public string Information;
        #endregion

    }

    /// <summary>
    /// Created by: Hamid - 11-11-2017
    /// Modified by:
    /// Class to hold list of lessons
    /// </summary>
    [System.Serializable]
    public class studentLessonTable
    {
        public List<LessonDetail> studentLessonList;
    }

    [Serializable]
    public class LessonInventory
    {
        public List<LessonDownloadClass> public_lesson;
        public List<LessonDownloadClass> private_lesson;
    }


    [Serializable]
    public class publicAudioList
    {
        public List<LessonDownloadClass> LessonsAudio;
    }


    [Serializable]
    public class LessonDownloadClass : IdataObject
    {

        public int id;
        public int teacher_lesson_package_id;
        public string title;
        public string body;
        public string experience_id;
        public AudioLessonClass audio;
    }


    [Serializable]
    public class AudioLessonClass
    {
        public int id;
        public string file_name;

    }
}
