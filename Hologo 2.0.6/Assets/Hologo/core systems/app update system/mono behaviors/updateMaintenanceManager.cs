using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 july 2019
    /// Modified by: 
    /// behavior that manages updates to app and maintenace
    /// </summary>
    public class updateMaintenanceManager : messageLogging
    {

        [SerializeField]
        private settings_SO settings;

        [SerializeField]
        private serverStatusDataClass serverStatus;

        [SerializeField]
        private dataPaths_SO datapath;

        public async Task<bool> getUpdateAndServerStatus()
        {

            bool success = false;


            if (serverStatus != null && serverStatus.statusIsFilled())
                return true;

            updateMaintenanceServerConnect umsc = new updateMaintenanceServerConnect();
            var task = umsc.getUpdateAndMaintenanceStatusFromServer(datapath.getUrl(0));

            string result;

            try
            {
                result = await task;
                GenericObjectAndStatus<HologoWebAPIGeneric<serverStatusDataClass>> updateAndStatus = 
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<serverStatusDataClass>>(result);

                if(updateAndStatus.GenericObject.success)
                {
                    serverStatus = updateAndStatus.GenericObject.data;
                    Debug.Log(" updates success");
                    success = true;
                }
                else
                {
                    createMessage("error");
                }

            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }
            
            return success;
        }

        /// <summary>
        /// checking to see if the app needs to be updated. by checking in app version vs version from server
        /// </summary>
        /// <returns></returns>
        public bool doesAppNeedUpdating()
        {
            return serverStatus.checkVersionIslater(appEVars.AppVersion);
        }

        // check if ios or android needs update // Shariz 23rd August 2020 v2.1.0
        /// <summary> 
        /// checking to see if the app needs to be updated for specific platform. by checking if IOS_UPDATE is 1 or ANDROID_UPDATE is 1 from server.
        /// </summary>
        /// <returns></returns>
        public bool whichPlatformNeedUpdating()
        {
        #if UNITY_EDITOR
            if (serverStatus.checkIfiOSNeedUpdate() == 1)
            {
                createMessage(serverStatus.IOS_UPDATE_MESSAGE);
                return true;
            }
        #endif

        #if UNITY_ANDROID
            
            if (serverStatus.checkIfAndroidNeedUpdate() == 1)
            {
                createMessage(serverStatus.ANDROID_UPDATE_MESSAGE);
                return true;
            }
        #elif UNITY_IPHONE
            if (serverStatus.checkIfiOSNeedUpdate() == 1)
            {
                createMessage(serverStatus.IOS_UPDATE_MESSAGE);
                return true;
            }
        #endif
            return false;
        }

        /// <summary>
        /// returns true forced update is on
        /// the message to display is in readmessage log
        /// this update will change the app version
        /// </summary>
        /// <returns></returns>
        public bool isForcedUpdate()
        {
            createMessage(serverStatus.FORCE_UPDATED_AT_MESSAGE);
            return serverStatus.dateCompare(appEVars.forcedUpdateDate, serverStatus.FORCE_UPDATED_AT);
        }

        /// <summary>
        /// returns true forced update is on
        /// the message to display is in readmessage log
        /// this update will change the app version
        /// </summary>
        /// <returns></returns>
        public bool isNormalUpdate()
        {
            createMessage(serverStatus.NORMAL_UPDATED_AT_MESSAGE);
            return serverStatus.dateCompare(appEVars.normalUpdateDate, serverStatus.NORMAL_UPDATED_AT);
        }

        public bool doLanguagesNeedUpdated()
        {
            return serverStatus.dateCompare(settings.lastDateLanguageUpdated, serverStatus.LANGUAGE_UPDATED_AT);
        }

        public string getLanguageUpdateDate()
        {
            return serverStatus.LANGUAGE_UPDATED_AT;
        }
    }
}
