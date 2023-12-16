using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hologo2
{
    [CustomEditor(typeof(ProcessDeepLinkMngr))]
    public class deepLinkEditor_CI : Editor
    {

        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
                base.OnInspectorGUI();
            }

            ProcessDeepLinkMngr dlMngr = target as ProcessDeepLinkMngr;


            if (GUILayout.Button("Deep Link Load"))
            {
                dlMngr.testURL();
            }



        }
    }
}
