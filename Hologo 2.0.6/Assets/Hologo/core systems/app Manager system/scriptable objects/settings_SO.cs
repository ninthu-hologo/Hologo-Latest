using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 01 july 2019
    /// Modified by: 
    /// main settings configuration saves
    /// </summary>
    [CreateAssetMenu(fileName = "settings.asset", menuName = "Hologo V2/new settings")]
    public class settings_SO : ScriptableObject
    {
        // where all settings are saved and loaded.
        [Header("APP CONFIG")]
        public bool firstTime = false;
        public bool isAppSoundsOn = true;
        public bool startInArMode = true;
        public bool labelsAlwaysOn = true;
        
        bool isConfigLoaded = false;

        public bool pushNotificationOn; // Shariz 16th Oct 2019 2.00
        public bool appRated = false; // Shariz 16th Oct 2019 2.00
        public bool onboardingOn = true; // Shariz 25th Oct 2019 2.00
        public categoryClass currentCategory;
        public bool SubNoticeClosed = false;// shariz 1.1.5
        // for language upadting
        public bool languageUpdate = false;
        public int AppCount;
        public labelsSize labelSize = labelsSize.SMALL;
        public bool isArSupported = true;

        //Shariz - 23 Jan 2020 v2.0.4   Adding library data reset settings
        public bool resetLibraryData = false;

        [Header("EXPERIENCE FEILDS")]
        [HideInInspector]
        public experienceDataClass currentExperience;
        [Header("DEVICE FEILDS")]
        //[HideInInspector]
        public OSPlatform osplatform;
        // [HideInInspector]
        public deviceType device;
        //[HideInInspector]
        public float canvasScaleFactor;
        // [HideInInspector]
        public float titleMargin;
        // [HideInInspector]
        public float bodyHeight;
        // [HideInInspector]
        public float footerMargin;

        [Header("LOCALIZATION FEILDS")]
        //[HideInInspector]
        public localizationListDataClass country;
       // [HideInInspector]
        public languageDetails language;
       // [HideInInspector]
        public syllabiDetails syllabus;
       // [HideInInspector]
        public curriculaDetails curriculum;

        [Header("UPDATE DATES")]
        public string lastDateLanguageUpdated = "2019-09-01";

        public bool requireNormalUpdate = false; // Shariz 23rd August 2020 v2.1.0 adding normal update notification

        public bool settingsIsLoaded()
        {
            return isConfigLoaded;
        }

        // set once when the app is launched first time
        public void setNewSettings()
        {
            Debug.Log("new settings");
            firstTime = true;
            isAppSoundsOn = true;
            startInArMode = true;
            labelsAlwaysOn = true;
            isConfigLoaded = false;
            SubNoticeClosed = false;
            pushNotificationOn = false;// Shariz 16th Oct 2019 2.00
            appRated = false;// Shariz 16th Oct 2019 2.00
            onboardingOn = true;// Shariz 25th Oct 2019 2.00
            languageUpdate = false;
            languageBundle = null;
            languageChangeFlag = false;
            canvasScaleFactor = 0f;
            currentExperience = null;
            currentCategory = new categoryClass("Home",18);
            currentCategory.id = 18;
            country = new localizationListDataClass();
            country.id = 0;
            language = new languageDetails();
            language.name = "English";
            syllabus = null;
            curriculum = null;
            AppCount = 1;
            resetLibraryData = false;  //Shariz - 23 Jan 2020 v2.0.4   Library data reset settings create
            requireNormalUpdate = false;  // Shariz 23rd August 2020 v2.1.0 adding normal update notification
        }

        public void makeHomeDefaultCategory()
        {
            currentCategory = new categoryClass("Home", 18);
            currentCategory.id = 18;
        }

        public void setArIsSupported(bool state)
        {
            isArSupported = state;
            Debug.Log("Is AR supported " + isArSupported);
        }

        public void flushLanguage()
        {
            if (languageBundle != null)
                languageBundle.Unload(true);
            languageBundle = null;
        }
        public void setLocalizationSettings(localizationListDataClass country, languageDetails language,
                                             syllabiDetails syllabus, curriculaDetails curriculum)
        {
            this.country = country;
            this.language = language;
            this.syllabus = syllabus;
            this.curriculum = curriculum;
        }

        public string getPlatform()
        {
            switch (osplatform)
            {
                case OSPlatform.ANDROID:
                    return "android";
                case OSPlatform.IOS:
                    return "ios";
                default:
                    return "android";
            }

        }


        public void increaseAppCount()
        {
            AppCount++;
        }

        public void setFirstTimeFalse()
        {
            firstTime = false;
        }

        public void setCurrentCategory(categoryClass category)
        {
            currentCategory = category;
        }

        public void toggleSound(bool state)
        {
            isAppSoundsOn = state;
        }


        // Shariz 25th Oct 2019 2.00
        public void toggleOnboarding(bool state)
        {
            onboardingOn = state;
            Debug.Log("Toggling onboarding from settings data file" + state);
        }

        public void toggleLabels(bool state)
        {
            labelsAlwaysOn = state;
        }

        public void toggleARMode(bool state)
        {
            startInArMode = state;
        }

        //shariz 31st oct 2019 2.00
        public float getLabelSize()
        {
            switch (labelSize)
            {
                case labelsSize.SMALL:
                    return 0.62f;
                case labelsSize.MEDIUM:
                    return 1.12f;
                case labelsSize.LARGE:
                    return 1.62f;
                default:
                    return 0.73f;
            }
        }

         //Shariz - 22 Jan 2020 v2.0.4   Toggling library data reset
        public void toggleResetLibraryData(bool state)
        {
            resetLibraryData = state;
        }

        // Shariz 23rd August 2020 v2.1.0 adding normal update notification
        public void togglerRequireNormalUpdate(bool state)
        {
            requireNormalUpdate = state;
        }

        public string getCurrentLanguageName()
        {
            return language.name;
        }

        public enum labelsSize
        {
            SMALL,
            MEDIUM,
            LARGE,
        }



       // [SerializeField]
        private bool languageChangeFlag = false;

        public bool LanguageChangeFlag
        {
            set
            {
                languageChangeFlag = value;
                currentCategory = new categoryClass("Home", 18);
                currentCategory.id = 18;
            }
            get { return languageChangeFlag; }
        }

        // for temporarily saving bundles
        private AssetBundle languageBundle;

        public AssetBundle LanguageBundle
        {
            get { return languageBundle; }
        }

        public void setLanguageBundle(AssetBundle bundle)
        {
            languageBundle = bundle;
        }

        public bool configLoadedStatus()
		{
			return isConfigLoaded;
		}

		public void setConfigLoadedStatus(bool status)
		{
			isConfigLoaded= status;
		}

	}

}
