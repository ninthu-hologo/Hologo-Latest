using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 31 july 2019
    /// Modified by: 
    /// custom inspector for subscription manager
    /// </summary>
    [CustomEditor(typeof(subscriptionManager))]
    public class subscriptionManagerEditor_CI : Editor
    {

        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            subscriptionManager um = target as subscriptionManager;
            if (GUILayout.Button("request subscritpions from server"))
            {
                //  hGoap_nodeEditor.showEditor(hGoap);
                um.loadSubscriptionsEditorOnly();
            }  

        }


    }
}
