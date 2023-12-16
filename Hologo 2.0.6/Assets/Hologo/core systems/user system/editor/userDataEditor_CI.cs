using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 03 September 2019
    /// Modified by: 
    /// custom inspector for user data
    /// </summary>
    [CustomEditor(typeof(userData_SO))]
    public class userDataEditor_CI : Editor
    {
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            userData_SO sm = target as userData_SO;
            if (GUILayout.Button("flush user data"))
            {
                sm.flushUserData();
            }
        }
    }
}
