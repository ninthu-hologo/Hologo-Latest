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
    [CustomEditor(typeof(labelElement))]
    public class labelElementCustomInspector : Editor
    {
//        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
    //        int posX = 10;
//            int posY = 40;

            //showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            base.OnInspectorGUI();

            labelElement le = target as labelElement;
            if (GUILayout.Button("preview label"))
            {
                le.previewLabel();
            }

        }

    }
}
