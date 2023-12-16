using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;
namespace Hologo2
{
    /// <summary>
    ///  all data resetter 
    /// </summary>
    [CreateAssetMenu(fileName = "dataResetter.asset", menuName = "Hologo V2/data reset")]
    public class dataReset_SO : ScriptableObject
    {
        public settings_SO mainSettings;
        public userData_SO userData;
        public libraryData_SO library;
        public categoryData_SO categories;
        public subscriptionData_SO subscriptionsData;
        public assetBundelData_SO assetBundleData;
        public languageData_SO language;
        public localizationList_SO localizationList;
        public quizProgression_SO quizScores;
        public lessonsList_SO recordedLessons;
        public studentLessons_SO studentLessons;


        public void resetAllData()
        {
            mainSettings.setNewSettings();
            userData.flushUserData();
            library.flushData();
            categories.flushData();
            subscriptionsData.flushData();
            assetBundleData.ClearList();
            language.flushData();
            localizationList.flushData();
            quizScores.flushData();
            recordedLessons.flushData();
            studentLessons.flushData();
        }


    }
}
