using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// custom inspector for settings manager
    /// </summary>
    [CustomEditor(typeof(settingsManager))]
    public class settingsManagerEditor_CI : Editor
    {

        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            settingsManager sm = target as settingsManager;
            if (GUILayout.Button("load saved settings data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                sm.loadSettings();
            }

            if (GUILayout.Button("Save settings data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                sm.saveAndUpdateSettings();
            }

        }


    }
}
