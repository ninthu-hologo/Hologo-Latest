using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 August 2019
    /// Modified by: 
    /// this manages quiz scores
    /// </summary>
    public class quizManager : messageLogging
    {

        [SerializeField]
        quizProgression_SO quizScores;
        [SerializeField]
        dataPaths_SO dataPath;
        [SerializeField]
        settings_SO mainSettings;
        [SerializeField]
        userData_SO userData;

        /// <summary>
        /// loads all the quiz data list from storage
        /// </summary>
        /// <returns></returns>
        public bool loadQuizScores()
        {
            Debug.Log("is run");
            bool success = false;
            if (quizScores.isQuizDataFIlled())
                return true;

            if (checkIfQuizScoreExists())
            {
                quizServerConnect qsc = new quizServerConnect();
                string result = qsc.getQuizScoreFromStorage(dataPath.getFolder(0), dataPath.getFileName(0));
                if (string.IsNullOrEmpty(result))
                    return false;

                jsonHelper.DeserializeJsonToScriptableObject(result, quizScores);
                success = true;
            }
            else
            {
                // no recorded data exists
                success = false;
            }
            return success;
        }

        
        // add score return true if its a new highscore or first time completed
        public bool addScore(quizScoreData score)
        {
           bool highscore = quizScores.addScoreEntry(score);
            saveQuizScores();
            return highscore;
        }

        // get score from score list so 
        public quizScoreData getScore(int expId)
        {
           return quizScores.getScore(expId);
        }

        // helper functions
        bool checkIfQuizScoreExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // save scores
        bool saveQuizScores()
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            return lsc.writeRecordedLessonListDataToStorage(quizScores, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // delete score data
        public bool deleteScoresData()
        {
            return readWriteData.DeleteFileOnDisk(dataPath.getFolder(0), dataPath.getFileName(0));
        }
        
    }
}
