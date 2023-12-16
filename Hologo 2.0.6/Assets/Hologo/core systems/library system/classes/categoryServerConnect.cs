using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this category data scriptable object
    /// </summary
    public class categoryServerConnect
    {

        // loading experiences from server
        public async Task<string> loadCategoriesFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url,false);
        }

        // loading data from storage
        public string loadCategoriesFromLocalStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        // writing data to storage
        public bool writeCategoryToStorage<T>(T jsonObject, bool checkFileExits , params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson, checkFileExits, pathStrings);
        }

        // delete library
        public bool deleteCategories(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }
    }
}