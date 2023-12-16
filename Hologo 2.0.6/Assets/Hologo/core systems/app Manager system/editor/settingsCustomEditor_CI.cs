using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 03 September 2019
    /// Modified by: 
    /// custom inspector for settings manager
    /// </summary>
    [CustomEditor(typeof(settings_SO))]
    public class settingsCustomEditor : Editor
    {

        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            settings_SO sm = target as settings_SO;
            if (GUILayout.Button("flush settings data"))
            {
                sm.setNewSettings();
            }
            if (GUILayout.Button("flush language"))
            {
                sm.flushLanguage();
            }


        }



    }
}
