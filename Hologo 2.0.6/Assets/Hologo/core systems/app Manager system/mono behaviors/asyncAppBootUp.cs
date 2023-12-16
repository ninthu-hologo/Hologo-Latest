using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hologo2.library;
using Hologo.iOSUI;
using Hologo2.UpdateApp;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// class that boots up the app
    /// </summary>
    public class asyncAppBootUp : MonoBehaviour
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
        [SerializeField]
        private deviceBootUp deviceBoot;
        [SerializeField]
        private loadScene loader;
        [SerializeField]
        private arSupportChecker arChecker;
        [SerializeField]
        private assetBundleBootUp aBBoot;
        [Header("VIEW CONTROLLERS")]
        [SerializeField]
        private localizationListViewController lLVController;
        [SerializeField]
        private appBootUpUICanvas appBootUpUI;
        [SerializeField]
        private directoryDeleterCreator direct;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;
        [Header("old app updater")]
        [SerializeField]
        private oldAppUpdater oAUpdater;

        bool isInternet;


        // Start is called before the first frame update
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

        [SerializeField]
        private int countryId;
        [SerializeField]
        private int languageId;




        private void Awake()
        {
            deviceBoot.runBootSequence();
        }

        // booting config
        //TODO: save settings at some point
        async void Start()
        {

            appBootUpUI.setFirstBootUIState(true);
           
            // getting settings
            await initSettings();
            // getting device details
            deviceBoot.setDeviceToSettings();

            deviceBoot.setCanvasScalers();

            if(settings.firstTime)
            {
                // run this once then forget for forever
                oAUpdater.CleanOldApp();
                direct.createDirectories();
            }
            

            // app already has launched
            if (settings.configLoadedStatus())
            {
                if (settings.LanguageChangeFlag)
                {
                    // set language
                    countryId = settings.country.id;
                    languageId = settings.language.id;
                    lLVController.setLocalizationViewActive();
                    return;
                }
            }

            if (!settings.firstTime)
            {
                appBootUpUI.setFirstBootUIState(false);

            }


            if (await initUpdates())
            {
                await initLocalization();

                if(localizationBoot.didBoot())
                {
                    // after language is update we set the date to current date from server and set update to false
                    settings.lastDateLanguageUpdated = updatesBoot.getLanguageDate();
                    settings.languageUpdate = false;
                }
                // can setup bools here to check for new user and inform stuff
                // booting up localization
                if (settings.firstTime)
                {
                    Debug.Log("first time");
                    // run language select
                    // USER INTERACTION POINT
                    // country + language selection screen
                    lLVController.setLocalizationViewActive();
                    return;
                }
                else
                {
                    Debug.Log("nomal boot");

                     //Shariz - 23 Jan 2020 v2.0.4   Reseting the library data
                    if(settings.resetLibraryData == true){
                        settings.resetLibraryData = false;
                        changeLanguageEvent();
                    }
                }

                await continueBooting();
            }
        }

        public void EventContinueAfterLocalizationSelection(GameObject go)
        {
            ContinueBootingAfterLocalizationSelection();
            // commonLoadingScreenSingleton.Singleton.disableLoadingScreen();// Shariz 12th Nov 2019 2.01
        }

        public async void ContinueBootingAfterLocalizationSelection()
        {
            await continueBooting();
        }

        public async Task continueBooting()
        {
            // returns a false if it is a new user
            await initUser();
            await initAssetBundles();
            // check for active subscriptions
            if (isInternet)
            {
                await initSubscriptions();
            }
            // load library
            if (await initLibrary())
            {
                if (settings.firstTime)
                {
                    settings.setFirstTimeFalse();
                    settingsBoot.saveSettings();
                }
            }
            else
            {
                if(settings.firstTime)
                {
                    // cant load library so cant go ahead at this time.
                    appBootUpUI.NoInternetWarningFirstTime();
                }
            }
            StartCoroutine(runEnumerators());
        }


        async Task initDevice()
        {
            deviceBoot.runBootSequence();
            //settingsBoot.saveSettings();
        }

        // boot up settings.
        // will return false if a new settings is made
        async Task initSettings()
        {
            settingsBoot.runBootSequence();
            Debug.Log("langauage updated date: "+settings.lastDateLanguageUpdated);
            //settingsBoot.saveSettings();
        }


        async Task<bool> initUpdates()
        {
            bool success = true;
            await updatesBoot.runBootUp();
            if (!updatesBoot.didBoot())
            {
                if(settings.firstTime)
                {
                    // halt app as this is the first time app is opening \
                    // USER INTERACTION POINT

                    Debug.Log(" first time > no internet");
                    appBootUpUI.NoInternetWarningFirstTime();
                    success = false;
                    return success;
                }
                // and there is no internet connection
                Debug.Log(" updates > no internet");
                success = true;
            }
            else
            {
				isInternet = true;

                // Shariz 23rd August 2020 v2.1.0 adding version check to update START
                if (updatesBoot.doesAppNeedUpdating() && updatesBoot.whichPlatformNeedUpdating())
                {
                    Debug.Log("app version different so need update");
                    Debug.Log(updatesBoot.readMessage());
                    if (updatesBoot.isForcedUpdate())
                    {
                        // halt app
                        // iunform user to update app.
                        // USER INTERACTION POINT
                        Debug.Log(" updates > forced update");
                        success = false;
                        // returning as there is not point in going further
                        appBootUpUI.ForcedUpdate();
                        return success;
                    }

                    if (updatesBoot.isNormalUpdate())
                    {
                        // inform and recommend user to update app.
                        // USER INTERACTION POINT
                        settings.togglerRequireNormalUpdate(true);
                        Debug.Log(" updates > normal update");
                    } else
                    {
                        settings.togglerRequireNormalUpdate(false);
                    }
                } else
                {
                    settings.togglerRequireNormalUpdate(false);
                }
                // Shariz 23rd August 2020 v2.1.0 adding version check to update END
                if (!settings.firstTime)
                {
                    settings.languageUpdate = updatesBoot.languageNeedUpdating();

                    if (settings.languageUpdate)
                    {
                        Debug.Log("language needs updating");
                    }
                }
            }
            return success;
        }

        


        async Task<bool> initLocalization()
        {
            await localizationBoot.runBootUp();
            return localizationBoot.didBoot();
        }


        async Task<bool> initAssetBundles()
        {
            await aBBoot.runBootUp();
            return aBBoot.didBoot();
        }

        async Task<bool> initUser()
        {
            await userBoot.runBootUp();
            return userBoot.didBoot();
        }

        async Task<bool> initSubscriptions()
        {
            await subsBoot.runBootUp();
            return subsBoot.didBoot();
        }

        async Task<bool> initLibrary()
        {
            Debug.Log("library booting");
            await libraryBoot.runBootUp();
            return libraryBoot.didBoot();
        }

        IEnumerator runEnumerators()
        {
            yield return StartCoroutine(arChecker.CheckARSupport());
            settings.setArIsSupported(arChecker.getSupportStatus());
            yield return StartCoroutine(localizePrefabs());
        }

        IEnumerator localizePrefabs()
        {
            yield return StartCoroutine(localizationBoot.localizeAllPrefabs());
            loader.goToScene();
            // then go to next scene here;
        }

        public void changeLanguageEvent()
        {
             /* Shariz - 23 Jan 2020 v2.0.4   Regenerating localization even if same language 
            // if choosen language and country are the same ones just send to library
            // if (countryId == settings.country.id && languageId == settings.language.id)
            // {
            //     settings.LanguageChangeFlag = false;
            //     settingsBoot.saveSettings();
            //     loader.goToScene();
            //     commonLoadingScreenSingleton.Singleton.disableLoadingScreen();// Shariz 12th Nov 2019 2.01
            //     return;
            // } */

            Debug.Log("changing localization");
            //else we need to init. library and clear cache again.
            Caching.ClearCache();
            direct.deleteFilesInDirectories();
            libraryBoot.clearLibrary();
            initLibraryAfterLocalizationChange();
        }

        async void initLibraryAfterLocalizationChange()
        {
            await initLocalization();
            await initLibrary();
            settings.LanguageChangeFlag = false;
            settingsBoot.saveSettings();
            StartCoroutine(localizePrefabs());
            commonLoadingScreenSingleton.Singleton.disableLoadingScreen();// Shariz 12th Nov 2019 2.01
        }
    }
}
