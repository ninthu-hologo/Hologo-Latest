using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// settings boot up script
    /// </summary>
    public class updatesBootUp : messageLogging, IBootUp
    {
        [SerializeField]
        updateMaintenanceManager uMManager;

        private bool bootUpSuccess;

        public void runBootSequence()
        {
           // Init();
        }

        public async Task runBootUp()
        {
            await Init();
        }

        public bool didBoot()
        {
            return bootUpSuccess;
        }

        public async Task Init()
        {

            if(!await uMManager.getUpdateAndServerStatus())
            {
                // inform interface about the problem
                // this reads the error message
                bootUpSuccess = false;
                //
                createMessage(uMManager.readMessage());
                return;
            }
            else
            {
                bootUpSuccess = true;
            }

            // if sucessfull
            // checking if app needs updates first by checking if app version is older then one from server
            if (uMManager.doesAppNeedUpdating())
            {
                createMessage(uMManager.readMessage());
            }
        }

        // check if app version is less // Shariz 23rd August 2020 v2.1.0
        public bool doesAppNeedUpdating()
        {
            // version is different, check if app need normal or forced upate
            createMessage(uMManager.readMessage());
            return uMManager.doesAppNeedUpdating();
        }

        // check if ios or android needs update // Shariz 23rd August 2020 v2.1.0
        public bool whichPlatformNeedUpdating()
        {
            // check if its an ios update or android update that is needed
            createMessage(uMManager.readMessage());
            Debug.Log(uMManager.readMessage());
            return uMManager.whichPlatformNeedUpdating();
        }

        // check if its a forced update
        public bool isForcedUpdate()
        {
            // halt the app and inform user about the forced update
            // app cant go ahead.
            createMessage(uMManager.readMessage());
            return uMManager.isForcedUpdate();
        }

        // check if its a normal update
        public bool isNormalUpdate()
        {
            // inform user there is an update available and update the app.
            createMessage(uMManager.readMessage());
            return uMManager.isNormalUpdate();
        }

        // check to see langauge needs updating
        public bool languageNeedUpdating()
        {
            return uMManager.doLanguagesNeedUpdated();
        }

        public string getLanguageDate()
        {
            return uMManager.getLanguageUpdateDate();
        }

    }
}
