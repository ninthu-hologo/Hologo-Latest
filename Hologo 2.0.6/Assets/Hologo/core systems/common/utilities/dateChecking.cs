using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// date checking class
    /// </summary>
    public class dateChecking
    {
        // date time compare.
        // return true if otherDate is newer
        public static bool CompareDateTime(string thisDate, string otherDate)
        {
            bool state = true;
            // try parsing date string to date time
            DateTime dateOne = DateTime.Parse(thisDate);
            DateTime dateTwo = DateTime.Parse(otherDate);
            // date comparison
            int result = DateTime.Compare(dateOne, dateTwo);

            // if datetwo us newer than date one > set state to false
            if (result > 0 || result == 0)
            {
                state = false;
            }
            else if (result < 0)
            {
                state = true;
            }

            return state;
        }


        public static bool CheckDateparsing(string date)
        {
            bool state = true;

            try
            {
                DateTime d = DateTime.Parse(date);
            }
            catch (Exception ex)
            {
                state = false;
            }

            return state;
        }

    }
}