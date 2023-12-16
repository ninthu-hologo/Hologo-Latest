using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// custom inspector for students lessons manager
    /// </summary>
    [CustomEditor(typeof(studentLessonsManager))]
    public class studentLessonsManager_CI : Editor
    {
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            studentLessonsManager slm = target as studentLessonsManager;
            if (GUILayout.Button("load saved student lesson data"))
            {
                slm.loadStudentLessonListFromStorage();
            }

            if (GUILayout.Button("Save student lesson data"))
            {
                slm.saveStudentLessonList();
            }

        }


    }
    
}
