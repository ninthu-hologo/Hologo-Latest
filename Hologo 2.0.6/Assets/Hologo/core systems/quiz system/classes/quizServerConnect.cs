using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 August 2019
    /// Modified by: 
    /// this class connects to server/storage and get quiz score lists and saves.
    /// </summary>
    public class quizServerConnect
    {



        // readfile from storage
        public string getQuizScoreFromStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }


        // writing data to storage
        public bool writeQuizScoreDataToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson, checkFileExits, pathStrings);
        }

    }
}
