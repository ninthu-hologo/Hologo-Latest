using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hologo2
{
	/// <summary>
	/// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
	/// Modified by: 
	/// custom inspector for students lessons manager
	/// </summary>
	[CustomEditor(typeof(dataReset_SO))]
	public class dataResetEditor_CI : Editor
	{

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

            dataReset_SO el = target as dataReset_SO;
            

            if (GUILayout.Button("Reset All Data"))
            {
                el.resetAllData();
            }

           

        }
    }
}
