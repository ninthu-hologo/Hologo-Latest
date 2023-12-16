using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 june 2019
    /// Modified by: 
    /// this class gets localization lists from server or local
    /// </summary>
    public class localizationServerConnect
    {
        // getting a list of country and its languages and returns as a string
        public async Task<string> getCountryLanguageListFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        // loading data from storage
        public string loadLocalizationListFromLocalStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        // writing data to storage
        public bool writeLocalizationListToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson,checkFileExits, pathStrings);
        }

        // delete library
        public bool deleteLocalizationList(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }

    }
}
