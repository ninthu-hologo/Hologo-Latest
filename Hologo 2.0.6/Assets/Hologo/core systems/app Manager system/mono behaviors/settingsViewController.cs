using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// settings view controller
    /// </summary>
    public class settingsViewController : MonoBehaviour
    {

        [Header("UI ELEMENTS")]
        [SerializeField]
        private settingsUIConnect sConnect;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;
        [Header("VIEW CONTROLLERS")]
        [SerializeField]
        private subscriptionsViewController subsVController;
        [SerializeField]
        private recordedLessonViewControllerSettings rLVController;
        [SerializeField]
        private userViewController uVController;
        [SerializeField]
        private settingsSwitcher settingsUI;
        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private userData_SO userData;
        [SerializeField]
        private userManager uManager;
        [SerializeField]
        private subscriptionManager subsManager;
        [SerializeField]
        private settingsManager sManager;
        [SerializeField]
        private profileManager pManager;
        [SerializeField]
        private assetBundleManager aBManager;
        [SerializeField]
        private bottomBarNavigation bbNavigation;


        private settingsUIEnum currentEnum;

        [SerializeField]
        private loadScene sceneLoader;

        //Shariz - 23 Jan 2020 v2.0.4   Reset Library scene
        [SerializeField]
        private loadScene resetSceneLoader;



        // [Header("EVENTS")]
        //[SerializeField]
        // private event_SO hideNavigation;



        private void Start()
        {
            mainSettings.setConfigLoadedStatus(true);
            // Shariz 23rd August 2020 v2.1.0 adding normal update notification
            if (mainSettings.requireNormalUpdate)
            {
                CheckForNormalUpdate();
            }
        }

        // event raised by library
        public void GotoSignInEvent(GameObject go)
        {
            //setSettingsViewActive();
            bbNavigation.gotoSettings();
        }

        // Shariz 23rd August 2020 v2.1.0 adding normal update notification
        public void CheckForNormalUpdate()
        {
            string title = "<font=English>An update available</font>";
            string message = "<font=English>Please download the update from the Appstore</font>";
            string deletetext = "<font=English>Not Now</font>";
            string optiontext = "<font=English>Take me there</font>";
            //windowMain.ModelWindowB.ChoiceMaker(false, iOS_ModalWindowB.ButtonOptions.OptDel, title, message, deletetext, optiontext, null, null, null, GoToAppUpdatePage, null);
        }

        // Shariz 23rd August 2020 v2.1.0 Go to update page
        public void GoToAppUpdatePage()
        {
            Debug.Log("Go to App Store update");
        }


        // used by main nav buttons to activate main settings ui
        public void setSettingsViewActive()
        {
            uVController.showUserNameAndType();
            sConnect.setSettingsUIState(true, userData.isUserSignedIn(), userData.getUserType());

            settingsUI.getSettingsUI().getArModeButton().setToggleDefault(mainSettings.startInArMode);
            settingsUI.getSettingsUI().getSoundOnOffButton().setToggleDefault(mainSettings.isAppSoundsOn);
            settingsUI.getSettingsUI().getLabelsOnOffButton().setToggleDefault(mainSettings.labelsAlwaysOn);
            settingsUI.getSettingsUI().getOnboardingOnOffButton().setToggleDefault(mainSettings.onboardingOn);// Shariz 25th Oct 2019 2.00

            // or activate sign in view.
        }

        // used by main nav buttons to de-activate main settings ui
        public void setSettingsViewToFalse()
        {
            sConnect.setSettingsUIState(false, userData.isUserSignedIn(), userData.getUserType());

        }

        // back button of settings
        public void goBack()
        {
            uVController.showUserNameAndType();
            // if in edit profile or chnage password ui we send back to user as thats these uis parent
            // else just sent to main settings button ui
            // stoping audio when going back to settings main
            if(currentEnum == settingsUIEnum.LESSONS)
                rLVController.stopAudioClip();

            if (currentEnum == settingsUIEnum.PROFILE || currentEnum == settingsUIEnum.CHANGEPASSWORD)
            {
                currentEnum = settingsUIEnum.SETTINGSMAIN;
                
            }
            else
            {
                currentEnum = settingsUIEnum.SETTINGSMAIN;
            }
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            changeUI();
            Debug.Log(currentEnum);

            sManager.SaveSettingsToStorage();// Shariz 2nd Nov 2019 2.00
            Debug.Log("Settings saved to storage");

        }


        #region APP SETTINGS

        public void toggleSound(bool state)
        {
            mainSettings.toggleSound(state);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        public void toggleLabels(bool state)
        {
            mainSettings.toggleLabels(state);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        public void toggleARMode(bool state)
        {
            mainSettings.toggleARMode(state);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }
        public void toggleOnboarding(bool state)// Shariz 25th Oct 2019 2.00
        {
            mainSettings.toggleOnboarding(state);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
            Debug.Log("Toggling onboarding from settings view controller" + state);
        }


        #endregion


        #region BUTTON METHODS TO ACTIVATE UIS


       

        public void changeLocalization()
        {
            mainSettings.LanguageChangeFlag = true;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            sceneLoader.goToScene();
        }

        public void gotoSettingsMain()
        {
            currentEnum = settingsUIEnum.SETTINGSMAIN;
            changeUI();
           
        }

        /* shariz removing subscriptions 
        public void gotoSubscriptions()
        {
           // currentEnum = settingsUIEnum.SUBSCRIPTIONS;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            subsVController.checkSubscriptionStatusEvent(this.gameObject);
            sConnect.EnableSubscriptionsUI(); // Shariz 12th Nov 2019 2.01  
            
          //  hideNavigation.raise(this.gameObject);
            //generate subscriptions ui elements
           // changeUI();
        } */
        public void gotoTeachers()
        {
            currentEnum = settingsUIEnum.TEACHERS;
            // generate teachers ui elements
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            changeUI();
        }
        public void gotoStudents()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            currentEnum = settingsUIEnum.STUDENTS;
            // generate students
            changeUI();
        }
        public void gotoLessons()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            currentEnum = settingsUIEnum.LESSONS;
            rLVController.initiateRecordedViewController();
            // generate lessons
            changeUI();
        }
        public void gotoUser()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            currentEnum = settingsUIEnum.USER;
            uVController.showUserNameAndType();
            changeUI();
        }
        public void gotoProfile()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            currentEnum = settingsUIEnum.PROFILE;
            changeUI();
        }
        public void gotoChangePassword()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            currentEnum = settingsUIEnum.CHANGEPASSWORD;
            changeUI();
        }

        //Shariz 13th April 2020 v2.0.5 setting a confirm before clearing cache
        public void clearCacheConfirmation()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
            string title = localizationProvider.provide.getLanguage().getAButtonText(13);
            string description = localizationProvider.provide.getLanguage().getALabelText(101); // Remember this remove comment and comment out the below line
            //string description = "Are you sure you want to refresh all the experiences. This will not delete your user data.";
            windowMain.ModelWindowB.ChoiceMaker(false, iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", null, resetLibraryData);// Shariz 29th Oct 2019 2.00
        }

        public void clearAssetBundleCache()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            Caching.ClearCache();
            aBManager.ClearCache();
            // string title = localizationProvider.provide.getLanguage().getAButtonText(13);
            string title = "Your cached experiences have been removed";
            windowMain.InfomationToast.ShowToast(title, 2f);
        }

        void changeUI()
        {
            settingsUI.getSettingsUI().changeSettingsView(currentEnum);
        }


        //Shariz - 22 Jan 2020 v2.0.4   Reseting the library data
        public void resetLibraryData(){

            mainSettings.resetLibraryData = true;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
            SoundManager.instance.PlaySoundFromList(0);
            sceneLoader.goToScene();
        }
        #endregion

    }

    public enum userUIEnum
    {
        SIGNIN,
        SIGNUP,
    }


    public enum settingsUIEnum
    {
        SETTINGSMAIN,
        SUBSCRIPTIONS,
        TEACHERS,
        STUDENTS,
        LESSONS,
        USER,
        PROFILE,
        CHANGEPASSWORD,
    }
}
