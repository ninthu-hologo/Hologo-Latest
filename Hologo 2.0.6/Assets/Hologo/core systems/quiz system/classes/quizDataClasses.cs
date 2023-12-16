using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    // class that keeps track of user quiz details
    [Serializable]
    public class quizScoreData : IdataObject, IEquatable<quizScoreData>
    {
        public string quizName;
        public int expereinceId;
        public float finishedTime = 0;
        public int totalAttempsPerSesson = 0;
        // maximum is 3 and minimum is 1 , default is 0
        public int starCount = 0;
        public int score;

        public void calculateScore()
        {
            score = (100/(totalAttempsPerSesson+1)) + (1000/((int)finishedTime+1));
        }

        public bool Equals(quizScoreData other)
        {
            if (this.expereinceId == other.expereinceId)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

    }


    [Serializable]
    public class quizStarCriteria:IdataObject
    {
        public int tire_one_time;
        public int tire_one_attempt;
        public int tire_two_time;
        public int tire_two_attempt;
    }


}