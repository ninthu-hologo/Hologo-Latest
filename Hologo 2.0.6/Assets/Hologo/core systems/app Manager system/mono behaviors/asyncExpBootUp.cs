using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hologo.iOSUI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 22 july 2019
    /// Modified by: Shariz 25th Oct 2019 2.00
    /// class that boots up the the experience scene
    /// </summary>
    public class asyncExpBootUp : MonoBehaviour
    {
        [Header("DATA FIELDS")]
        [SerializeField]
        private settings_SO settings;
        [Header("SCENE SCRIPTS")]
        [SerializeField]
        private experienceAssetBootUp expAssetBootUp;
        [SerializeField]
        private experienceViewController eVController;
        [SerializeField]
        private recordedLessonsBootUp lessonsBootUp;
        [SerializeField]
        private recordedLessonViewController rlViewController;
        [SerializeField]
        private quizViewController qVController;
        [SerializeField]
        private arViewController arVController;
        [SerializeField]
        private loadScene sceneLoader;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private arSwitcher arLoading;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;

        [SerializeField]
        private arSwitcher onboardingWindow;// Shariz 25th Oct 2019 2.00

        [Header("TESTING")]
        [SerializeField]
        private bool testMode;

        /* the script will intiate the experience scene
         * first it will execute the get assetbundles from experience manager
         * if its a success then it would start the process of loading the experience
         * after that ui will follow sequentialy untill every starter script is initialized
         * */

        async void Start()
        {
            // try to load the assetbundle and localized data asset bundle of the experience;
            Debug.Log("starting to boot");
           // Application.targetFrameRate = 60;

            if(testMode)
            {
                Debug.Log("testmode boot up");
                await initLessons();
                eVController.initiateExperienceViewController();
                rlViewController.initiateRecordedViewController();
                // initiating quiz view controller
                qVController.initiateQuizViewController(eVController.getExperienceData().quiz, eVController.getExperienceLocalizationData());
                // starting the process of showing and generating the experience to the user
                arLoading.GetARLoadingUI().disableLoadingARCanvas();

                return;
            }

            await initExpAssetLoad();

            if (expAssetBootUp.didBoot())
            {
                await initLessons();

                Debug.Log("experience boot successfull");
                
                eVController.initiateExperienceViewController();
                rlViewController.initiateRecordedViewController();

                onboardingWindow.GetOnboardingWindow().initiateOnboarding();// Shariz 25th Oct 2019 2.00

                // initiating quiz view controller
                qVController.initiateQuizViewController(eVController.getExperienceData().quiz, eVController.getExperienceLocalizationData());
                // starting the process of showing and generating the experience to the user
                arLoading.GetARLoadingUI().disableLoadingARCanvas();
            }
            else
            {
                // USER INTERACTION POINT
                // could not load asset bundle
                // return to library scene
                //windowMain.ModalWindowD.ShowInfo(true, false, "Warning", "could not load the experience.please try again later!", "Back to Library", goToLibrary);
                string warning = localizationProvider.provide.getLanguage().getATitleText(18);
                string back_to_library = localizationProvider.provide.getLanguage().getAMessageText(23);
                windowMain.ModalWindowD.ShowInfo(true, false, warning, "<font=English>"+expAssetBootUp.getMessages()+"</font>", back_to_library, goToLibrary);// Shariz 22nd Feb 2020 2.0.4
                Debug.Log("failed to boot");
            }
        }


        void goToLibrary()
        {
            sceneLoader.goToScene();
        }
        // boot up settings.
        // runinng assetbundle boot up;
        async Task initExpAssetLoad()
        {
            await expAssetBootUp.runBootUp();
        }

        async Task initLessons()
        {
            await lessonsBootUp.runBootUp();
        }
    }
}
