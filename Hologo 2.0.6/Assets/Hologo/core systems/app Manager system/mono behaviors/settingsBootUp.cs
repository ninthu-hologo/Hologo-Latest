using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// settings boot up script
    /// </summary>
    public class settingsBootUp : MonoBehaviour, IBootUp
    {
        [SerializeField]
        private settingsManager sManager;
        private bool bootUpSuccess;

        public void runBootSequence()
        {
            // if false its the first time app is running
            // if true app is already setup;
            bootUpSuccess = sManager.loadSettings();
        }

        public bool didBoot()
        {

            return bootUpSuccess;
        }

        public bool saveSettings()
        {
            return sManager.SaveSettingsToStorage();
        }
    }
}
