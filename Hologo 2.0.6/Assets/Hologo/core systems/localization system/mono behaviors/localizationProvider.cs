using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
	/// <summary>
	/// Created by: Hamid (hamidibrahim@gmail.com) - 11 September 2019
	/// Modified by: 
	/// provides loaded language to current view controllers or managers
	/// </summary>
	public class localizationProvider : MonoBehaviour
	{
        [Header("DATA")]
        [SerializeField]
        private languageData_SO currentLanguage;


        private static localizationProvider instance;


        // this makes sure that this script will remain in memory throught out the life of the app
        public static localizationProvider provide
        {
            get { return instance; }
        }


        public languageData_SO getLanguage()
        {
            return currentLanguage;
        }


        private void Awake()
        {
            instance = this;

        }

    }
}