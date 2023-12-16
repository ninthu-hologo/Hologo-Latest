using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// custom inspector for recorded lessons manager
    /// </summary>
    [CustomEditor(typeof(recordedLessonsManager))]
    public class recordedLessonManagerEditor_CI : Editor
    {
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            recordedLessonsManager rlm = target as recordedLessonsManager;
            if (GUILayout.Button("load saved lesson data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
               rlm.loadRecordedLessonList();
            }

            if (GUILayout.Button("Save lesson data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                rlm.SaveLessonListForEditor();
            }

        }


    }
}
