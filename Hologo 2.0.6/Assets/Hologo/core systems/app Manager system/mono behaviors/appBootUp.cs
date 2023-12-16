using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hologo2.library;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// class that boots up the app
    /// </summary>
    public class appBootUp : MonoBehaviour
    {

        [Header("DATA FIELDS")]
        [SerializeField]
        private settings_SO settings;
        [Header("BOOT UP SCRIPTS")]
        [SerializeField]
        private settingsBootUp settingsBoot;
        [SerializeField]
        private updatesBootUp updatesBoot;
        [SerializeField]
        private localizationBootUp localizationBoot;
        [SerializeField]
        private userBootUp userBoot;
        [SerializeField]
        private subscriptionsBootUp subsBoot;
        [SerializeField]
        private libraryBootUp libraryBoot;


        private bool appFirstTime = false;

        private void Start()
        {
            /* 1. load settings
             * 2. check for updates/maintenance
             * 3. initialize localization
             * 4. log in user / create first time user
             * 5. check for active subscription
             * 6. load library
             * 7. load guru
             * */
            // LOAD SETTINGS
            // 1. load settings if a false is returned then app is launching first time.
            // else app is already setup.
            // GET MAINTENANCE AND UPDATES FROM SERVER
            // 2. next up is gettings maintenance and updates from server. if this returns a false
            // while its first time launch then inform user that to connect to the internet before 
            // launching app. otherwise we will skip the warning.
            // after checking for updates will check whether there is an update to the app this comes 
            // two varities forced update and normal update. if its forced the app will halt and it wont 
            // go anyfurther unless app is updated . if its normal information is displayed to the user 
            // recommending top update. if both forced and normal returns false . that means no updates.
            // then check for language updates. if there is language updates . update the language list
            // and download the latest language asset bundle.
            // LOCAD LOCALIZATION
            // 3. load localizatiion and lanaguage . if its the first time user can select langauge and country
            // USER
            //4. if load user returns false it means its the first time an a new user is created and saved. 
            // if true then app attempts to log-in the user. and it may return false if app is unable to login or 
            // connect to the server. if able to log in then user is set and working normal.
            // // SUBSCRIPTION
            // 5. check for user subscription if true then working normal. if false then no active subscription and 
            // user can only access demo content
            // if subscription was unable to  check due to server issued then skip and use the settings to give user 
            // access.
            // LIBRARY
            // 6. try to load libary. if cant then library cannot be loaded
            // GURU
            // 7. show search filter if a search hasnt been made. if a search was made then get results.
            //.........................
            /*
             * first try load settings if its returned a false a new settings is made and its decided its apps first launch.
             * next update maintenance boot will run. it will try to get server status . if false it cant get server status .then app halts untill 
             * relaunch with an internet connection. settings 
             * and if true then it will check for the app version comparison. if there is a new version then it will check for forced update and normal
             * update.
             * if not then it will check for langauge updates. and if there is an update set language update settings in settings so. localization booter 
             * can use it.
             * if there are no updates it will go to the next boot up
             * next is localization. it will check the settings for a new version of language. if there is then it will update the localization.
             * if its apps first time then user is prompted with country/language selection. if not it will go to next one.
             * now settings will be saved.
             * */

            // booting config
            //TODO: save settings at some point
            initSettings();
            // other systems will initialize only if update/maintenance returns a true
            if (initUpdates())
            {
                // booting up localization
                if (initLocalization() && settings.firstTime)
                {
                    // run language select
                    // USER INTERACTION POINT
                }
                // can setup a bool here to check for new user and inform stuff <-- shariz
                unitUser();
                initSubscription();
                initLibrary();
                if (settings.firstTime)
                {

                }
            }

        }

        // boot up settings.
        // will return false if a new settings is made
        void initSettings()
        {
            settingsBoot.runBootSequence();
            appFirstTime = settingsBoot.didBoot();
        }

        bool initUpdates()
        {
            bool success = true;
            updatesBoot.runBootSequence();
            if (!updatesBoot.didBoot() && settings.firstTime)
            {
                // halt app as this is the first time app is opening \
                // USER INTERACTION POINT
                // and there is no internet connection
                Debug.Log(" updates > no internet");
                success = false;
            }
            else
            {
                if (updatesBoot.isForcedUpdate())
                {
                    // halt app
                    // iunform user to update app.
                    // USER INTERACTION POINT
                    Debug.Log(" updates > forced update");
                    success = false;
                    // returning as there is not point in going further
                    return success;
                }

                if (updatesBoot.isNormalUpdate())
                {
                    // inform and recommend user to update app.
                    // USER INTERACTION POINT
                    Debug.Log(" updates > normal update");
                }

                settings.languageUpdate = updatesBoot.languageNeedUpdating();

            }

            return success;
        }

        bool initLocalization()
        {
            localizationBoot.runBootSequence();
            return localizationBoot.didBoot();
        }

        bool unitUser()
        {
            userBoot.runBootSequence();
            return userBoot.didBoot();
        }

        bool initSubscription()
        {
            subsBoot.runBootSequence();
            return subsBoot.didBoot();
        }

        bool initLibrary()
        {
            libraryBoot.runBootSequence();
            return libraryBoot.didBoot();
        }
    }
}
