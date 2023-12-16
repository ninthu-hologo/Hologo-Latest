using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 August 2019
    /// Modified by: 
    /// this scriptable object is a data container for quiz data list
    /// </summary> 
    [CreateAssetMenu(fileName = "quizProgress.asset", menuName = "Hologo V2/new quizProgress")]
    public class quizProgression_SO : ScriptableObject
    {

        [SerializeField]
        private List<quizScoreData> quizScoreList;

        // data filled check
        public bool isQuizDataFIlled()
        {
            return quizScoreList.Count > 0;
        }

        // returns true if score is high score or a new one
        public bool addScoreEntry(quizScoreData score)
        {

            if (quizScoreList == null)
                quizScoreList = new List<quizScoreData>();

            for (int i = 0; i < quizScoreList.Count; i++)
            {
                if (quizScoreList[i].Equals(score))
                {
                    if (quizScoreList[i].score < score.score)
                    {
                        quizScoreList[i] = score;

                        return true;
                    }

                    return false;
                }
            }

            quizScoreList.Add(score);
            return true;
        }


        public void flushData()
        {
            quizScoreList.Clear();
        }

        // get score data for the selected experience
        public quizScoreData getScore(int expId)
        {
            try
            {
                for (int i = 0; i < quizScoreList.Count; i++)
                {
                    if (quizScoreList[i].expereinceId == expId)
                        return quizScoreList[i];
                }

                return null;
            }
            catch (System.Exception)
            {

                return null;
            }

        }

    }
}
