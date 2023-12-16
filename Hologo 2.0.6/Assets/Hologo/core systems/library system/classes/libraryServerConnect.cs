using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class gets categories from server or local
    /// </summary>
    public class libraryServerConnect
    {
        // loading experiences from server
        public async Task<string> loadExperiencesFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> updateExperiencesFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url,false);
        }

        // loading experiences belonging to the user -probably dont need this any more
        public void loadMyExperiencesFromServer(string url,string token)
        {
            // not implemented
        }

        // loading data from storage
        public string loadExperiencesFromLocalStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        // writing data to storage
        public bool writeLibraryToStorage<T>(T jsonObject, bool checkFileExits , params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson,checkFileExits, pathStrings);
        }

        // delete library
        public bool deleteLibrary(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }
    }


}
