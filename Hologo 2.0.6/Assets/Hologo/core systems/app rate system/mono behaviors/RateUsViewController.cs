using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;
using System.Threading.Tasks;
using UnityEngine.UI;


namespace Hologo2
{
    /// <summary>
    /// Created by: Shariz - 16 October 2019
    /// Modified by: 
    /// view controller for Rate Us 
    /// </summary>

    public class RateUsViewController : MonoBehaviour
    {


        [Header("SCRIPTS")]

        [SerializeField]
        private settings_SO MainSettings;
        [Header("Scene Objects")]
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;

        [Header("Data")]
        
        public int countReached;

        public Sprite rateImage;

        public static RateUsViewController instance = null; //SHARIZ 29th Oct 2019 2.00

        //SHARIZ 29th Oct 2019 2.00
        void Awake()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy(gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
        }

        public void Start (){
            initiateRateUsController();
        }

        public void initiateRateUsController()
        {
            initialize();
        }


        public void initialize()
        {
            if(MainSettings.appRated != true){
                if(MainSettings.AppCount % countReached == 0) {
                Invoke("openRatingWindow", 7);
                    // openRatingWindow();
                }
            }
        }

        public void openRatingWindow(){ 
            string title = localizationProvider.provide.getLanguage().getALabelText(73);
            // string description = localizationProvider.provide.getLanguage().getALabelText(74);
            string description = localizationProvider.provide.getLanguage().getALabelText(74);// Shariz 22nd Feb 2020 2.0.4
            windowMain.ModelWindowB.ChoiceMaker(true,iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", rateImage, RateApp);// Shariz 29th Oct 2019 2.00

        }



        void RateApp()
        {
            MainSettings.appRated = true; // TURN THIS ON FOR PRODUCTION
            Debug.Log("go to rating native window");

            #if UNITY_ANDROID
            Application.OpenURL("market://details?id=com.theloopcraft.TeachingTube");
            
            #elif UNITY_IPHONE
            // Application.OpenURL("itms-apps://itunes.apple.com/app/id1293300157?action=write-review");
            IOSReviewRequest.RequestReview();
            #endif

        }
    }

}