using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 july 2019
    /// Modified by: 
    /// date checking class
    /// </summary>
    public class settingsConnect
    {

        // loading data from storage
        public string loadSettingsFromLocalStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        // writing data to storage
        public bool writeSettingsToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson, checkFileExits,  pathStrings);
        }

    }
}
