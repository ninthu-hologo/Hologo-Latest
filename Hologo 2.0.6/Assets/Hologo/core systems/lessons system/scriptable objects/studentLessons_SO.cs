using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// this scriptable object is a data container for student lessons
    /// </summary> 
    [CreateAssetMenu(fileName = "studentLessonList.asset", menuName = "Hologo V2/new studentLessonList")]
    public class studentLessons_SO : ScriptableObject
    {


        [SerializeField]
        private List<RecordedLessonDetail> studentLessonList;

        public bool isDataFilled()
        {
            return studentLessonList.Count > 0;
        }


        public void addStudentLessonToTheList(RecordedLessonDetail lesson)
        {
            if (studentLessonList == null)
                studentLessonList = new List<RecordedLessonDetail>();

            if(studentLessonList.Count <= 0)
            studentLessonList.Add(lesson);

            bool exists = false;
            for (int i = 0; i < studentLessonList.Count; i++)
            {
                if(studentLessonList[i].Equals(lesson))
                {
                    exists = true;
                }
            }

            if(!exists)
                studentLessonList.Add(lesson);
        }


        public string getLessonFileName(int experienceId)
        {
            string ld = null;
            for (int i = 0; i < studentLessonList.Count; i++)
            {
                if(studentLessonList[i].experience_id== experienceId)
                {
                    return studentLessonList[i].fileName;
                }
            }
            return ld;
        }

        public RecordedLessonDetail getLesson(int experienceId)
        {
            RecordedLessonDetail ld = null;
            for (int i = 0; i < studentLessonList.Count; i++)
            {
                if (studentLessonList[i].experience_id == experienceId)
                {
                    return studentLessonList[i];
                }
            }
            return ld;
        }

        public List<RecordedLessonDetail> getDataList()
        {
            return studentLessonList;
        }

        public void flushData()
        {
            studentLessonList.Clear();
 
        }


    }
}
