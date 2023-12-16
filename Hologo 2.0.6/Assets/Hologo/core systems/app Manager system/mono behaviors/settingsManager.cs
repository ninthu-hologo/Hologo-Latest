using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class is to do with the app settings
    /// </summary>
    public class settingsManager : messageLogging
    {
        [SerializeField]
        private settings_SO settings;

        [SerializeField]
        private dataPaths_SO dataPath;

        

        // load settings when app is opened
        public bool loadSettings()
        {
            bool success = false;

            if (settings.configLoadedStatus())
                return true;
            if (checkIfSettingsExists())
            {
                if (convertSettingsData(readSettingsFromStorage()))
                {
                    settings.increaseAppCount();
                    success = true;
                }
                else
                {
                    settings.setNewSettings();
                }
            }
            else
            {
                settings.setNewSettings();
            }

            SaveSettingsToStorage();
            // success false means its the first time app is opened or failed to load and new settings been made
            return success;
        }

        // everytime user edits settings the settings is updated and saved
        public void saveAndUpdateSettings()
        {
            SaveSettingsToStorage();
        }


        // helper functions
        bool checkIfSettingsExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // read settings from storage
        string readSettingsFromStorage()
        {
            settingsConnect sc = new settingsConnect();
            return sc.loadSettingsFromLocalStorage(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //save library data to storage
        public bool SaveSettingsToStorage()
        {
            settingsConnect sc = new settingsConnect();
            return sc.writeSettingsToStorage(settings, false,  dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //convert stored data to data object
        bool convertSettingsData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, settings))
            {
                success = true;
            }
            return success;
        }

        private void OnApplicationQuit()
        {
            SaveSettingsToStorage();
        }
        private void OnApplicationPause(bool pause)// Hamid 2nd Nov 2019 2.00
        {
            SaveSettingsToStorage();
        }
    }
}
