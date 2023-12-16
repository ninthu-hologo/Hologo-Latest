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
    [CustomEditor(typeof(categoryData_SO))]
    public class categoryDataEditor_CI : Editor
    {
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            categoryData_SO sm = target as categoryData_SO;
            if (GUILayout.Button("flush category data"))
            {
                sm.flushData();
            }

        }
    }
}
