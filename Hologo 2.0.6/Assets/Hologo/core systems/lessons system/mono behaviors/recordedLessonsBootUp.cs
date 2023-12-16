using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 22 july 2019
    /// Modified by: 
    /// boots up experience asset manager
    /// </summary>
    public class recordedLessonsBootUp : messageLogging, IBootUp
    {
        [SerializeField]
        userData_SO userData;
        [SerializeField]
        recordedLessonsManager rlManager;
        [SerializeField]
        studentLessonsManager slManager;


        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;

        }

        public void runBootSequence()
        {
            //  initLibrary();
        }

        public async Task runBootUp()
        {
            // if user is a teacher
            if (userData.requestUserDetail().user_type_id == 1)
            {
                // intiate recording of narration
                // save it
                // play it
                // delete it
                await initTeachers();
            }

            // if user is a student
            if (userData.requestUserDetail().user_type_id == 2)
            {
                // iniate checking for custom teacher narrations
                // download a narration
                // play it
                // initialy check if a lesson is downloaded for this experience if so load it to be played
                await initStudents();
            }
        }

        
        

        async Task initStudents()
        {
            bootUpSuccess = slManager.loadStudentLessonListFromStorage();
            await Task.Yield();
        }

        async Task initTeachers()
        {
            bootUpSuccess = rlManager.loadRecordedLessonList();
            await Task.Yield();
        }

    }
}
