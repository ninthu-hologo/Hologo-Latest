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
    [CustomEditor(typeof(experienceLocalizationData_SO))]
    public class experienceLocalizationDataEditor_SO_CI : Editor
    {


        public string FileName = "expLoc.dtm";
        bool showAllFields = true;
        public override void OnInspectorGUI()
        {
   //         int posX = 10;
//            int posY = 40;

            showAllFields = GUILayout.Toggle(showAllFields, "show all fields");

            if (showAllFields)
            {
               // GUI.Label(new Rect(posX, posY, 300, 40), "label Id: ");

                base.OnInspectorGUI();
            }

            experienceLocalizationData_SO el = target as experienceLocalizationData_SO;
            if (GUILayout.Button("save data to disk"))
            {
                string expToJson = JsonUtility.ToJson(el, true);
               // string fileName = readWriteData.GetPath("expLoc",FileName);
                readWriteData.WriteFileToDisk(expToJson, false, "expLoc", FileName);
            }

            if (GUILayout.Button("load data from disk"))
            {
                
            }

            if (GUILayout.Button("clone Data into Quiz "))
            {
                el.copyList();
            }

        }



    }
}
