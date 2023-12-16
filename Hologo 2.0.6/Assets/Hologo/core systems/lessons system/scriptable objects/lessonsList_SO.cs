using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 3 june 2019
    /// Modified by: 
    /// this scriptable object is a data container for lessons
    /// </summary> 
    [CreateAssetMenu(fileName ="lessonList.asset",menuName = "Hologo V2/new lesson List")]
    public class lessonsList_SO : ScriptableObject
    {

        [SerializeField]
        private List<audioClipDetail> lessonList;
        [SerializeField]
        private List<audioClipDetail> serverLessonList;
        [SerializeField]
        private int idCount;

        public bool isLessonListFilled()
        {
            return lessonList.Count > 0;
        }

        public List<audioClipDetail> getlessonList()
        {
            return lessonList;
        }

        public int getNewId()
        {
            return idCount++;
        }

        public void setServerLessonList(List<audioClipDetail> serverList)
        {
            serverLessonList = serverList;
        }

        public List<audioClipDetail> getServerLessonList()
        {
            return serverLessonList;
        }

        public void flushData()
        {
            lessonList.Clear();
            serverLessonList.Clear();
            idCount = 1;
        }


        public void addLessonFromRecord(audioClipDetail clip)
        {
            lessonList.Add(clip);
        }

        public void addIfUnique(audioClipDetail clip)
        {
            bool exists = false;
            if (lessonList.Count <= 0)
            {
                lessonList.Add(clip);
            }
            else
            {
                for (int i = 0; i < lessonList.Count; i++)
                {
                    if (clip.Equals(lessonList[i]))
                    {
                        exists = true;
                    }
                }

                if (!exists)
                    lessonList.Add(clip);
            }

           
        }

        public void addIfServerUnique(audioClipDetail clip)
        {
            bool exists = false;
            for (int i = 0; i < lessonList.Count; i++)
            {
                if (clip.lessonDetailFromServer.Equals(lessonList[i].lessonDetailFromServer))
                {
                    lessonList[i].lessonDetailFromServer = clip.lessonDetailFromServer;
                    exists = true;
                }
            }

            if (!exists)
            {
                lessonList.Add(clip);
            }
        }

        public void updateLesson(audioClipDetail clip)
        {

            for (int i = 0; i < lessonList.Count; i++)
            {
                if (clip.Equals(lessonList[i]))
                {
                    lessonList[i].Update(clip);
                    break;
                }
            }
        }

        public void removeLesson(audioClipDetail clip)
        {
            lessonList.Remove(clip);
        }

        public void removeServerLesson(audioClipDetail clip)
        {
            serverLessonList.Remove(clip);
        }

        // filtering the list for lessons belonging to the current listen
        public List<audioClipDetail> filterAudioClipsToCurrentExperience(int id)
        {
            List<audioClipDetail> filteredList = new List<audioClipDetail>();
            for (int i = 0; i < lessonList.Count; i++)
            {
                if(lessonList[i].ExperienceId == id)
                {
                    filteredList.Add(lessonList[i]);
                }
            }

            return filteredList;
        }

    }
}
