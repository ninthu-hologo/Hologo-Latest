using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 july 2019
    /// Modified by: 
    /// this is sever maintenace and update object class
    /// </summary>
    [System.Serializable]
    public class serverStatusDataClass
    {
        //status = 0 == server in maintenance mode, status =1 == server is online
        public int APP_STATUS;
        // message to display when in maintenance mode;
       // public string message;
        // incoming app version
        public int APP_VERSION;
        public string LANGUAGE_UPDATED_AT;
        public string DATA_UPDATED_AT;
        public string FORCE_UPDATED_AT;
        public string NORMAL_UPDATED_AT;
        public string DATA_UPDATED_AT_MESSAGE;
        public string FORCE_UPDATED_AT_MESSAGE;
        public string NORMAL_UPDATED_AT_MESSAGE;
        public int HOME_ID;
        public int IOS_UPDATE;// Shariz 23rd August 2020 v2.1.0 adding normal update notification
        public string IOS_UPDATE_MESSAGE;// Shariz 23rd August 2020 v2.1.0 adding normal update notification
        public int ANDROID_UPDATE;// Shariz 23rd August 2020 v2.1.0 adding normal update notification
        public string ANDROID_UPDATE_MESSAGE;// Shariz 23rd August 2020 v2.1.0 adding normal update notification


        public bool statusIsFilled()
        {
            return APP_VERSION > 0;
        }

        public bool checkVersionIslater(int inApp)
        {
            return APP_VERSION > inApp;
        }

        // check if ios needs update // Shariz 23rd August 2020 v2.1.0
        public int checkIfiOSNeedUpdate()
        {
            return IOS_UPDATE;
        }

        // check if android needs update // Shariz 23rd August 2020 v2.1.0
        public int checkIfAndroidNeedUpdate()
        {
            return ANDROID_UPDATE;
        }


        public bool dateCompare(string mydate, string apiDate)
        {
            if (dateChecking.CheckDateparsing(mydate) && dateChecking.CheckDateparsing(apiDate))
            {
                return dateChecking.CompareDateTime(mydate, apiDate);
            }
            else
            {
                return true;
            }

        }

    }
}
