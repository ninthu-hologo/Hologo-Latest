using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 03 September 2019
    /// Modified by: 
    /// custom inspector for user data
    /// </summary>
    [CustomEditor(typeof(libraryData_SO))]
    public class libraryDataEditor_CI : Editor
    {
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            libraryData_SO sm = target as libraryData_SO;
            if (GUILayout.Button("flush library data"))
            {
                sm.flushData();
            }

        }
    }
}
