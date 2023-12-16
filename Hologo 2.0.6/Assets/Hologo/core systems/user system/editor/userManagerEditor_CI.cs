using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// custom inspector for user manager
    /// </summary>
    [CustomEditor(typeof(userManager))]
    public class userManagerEditor_CI : Editor
    {

        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            userManager um = target as userManager;
            if (GUILayout.Button("load saved user data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                um.loadUserFromStorage();
            }

            if (GUILayout.Button("Save user data"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                um.saveUserToStorage();
            }

        }


    }
}

