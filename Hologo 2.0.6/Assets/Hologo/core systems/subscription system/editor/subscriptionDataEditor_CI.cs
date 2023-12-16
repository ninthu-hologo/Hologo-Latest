using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hologo2
{
	/// <summary>
	/// Created by: Hamid (hamidibrahim@gmail.com) - 31 july 2019
	/// Modified by: 
	/// custom inspector for subscription manager
	/// </summary>
	[CustomEditor(typeof(subscriptionData_SO))]
	public class subscriptionDataEditor_CI : Editor
	{

		public override void OnInspectorGUI()
		{
			
				base.OnInspectorGUI();


			subscriptionData_SO um = target as subscriptionData_SO;
			if (GUILayout.Button("flush Data"))
			{
				//  hGoap_nodeEditor.showEditor(hGoap);
				um.flushData();
			}

		}

	}
}
