using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 may 2019
    /// Modified by:  //Shariz 15th September 2019 v2
    /// bottom bar navigation control
    /// </summary>
    public class bottomBarNavigation : MonoBehaviour
    {
        [SerializeField]
        GameObject bottomBar;
        [SerializeField]
        private settingsViewController sVController;
        [SerializeField]
        private libraryViewController lVController;
        [SerializeField]
        private userData_SO userDetail;
        [SerializeField]
        private settingsUIConnect sUIConnect;


         //shariz v2 11/9/2019 
        [SerializeField]
        GameObject libraryHighlight;
        //shariz v2 11/9/2019 
        [SerializeField]
        GameObject settingsHighlight;

        //Manuja 26/10/2023
        [SerializeField]
        private GameObject lensIconHighlight;
        [SerializeField]
        private GameObject lensIconInactive;


        //Manuja 2/10/2023
        [SerializeField]
        private GameObject lensUI; 

        enum naveItems
        {
            SETTINGS,
            LIBRARY,
            GURU,
            LENS,
        }

        public void Start() {
            libraryHighlight.SetActive(true); //shariz v2 11/9/2019 
            settingsHighlight.SetActive(false); //shariz v2 11/9/2019 
            lensUI.SetActive(false);
        }

        
        public void gotoLibrary()
        {
            changeNav(naveItems.LIBRARY);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        public void gotoSettings()
        {
            changeNav(naveItems.SETTINGS);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        public void gotoGuru()
        {
            changeNav(naveItems.GURU);
        }

        public void gotoLenses() //Manuja 2nd Oct 2023
        {
            changeNav(naveItems.LENS);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange); //Manuja 16/10/2023
            SoundManager.instance.PlaySoundFromList(0);
        }

        void changeNav(naveItems myNavs)
        {
            switch (myNavs)
            {
                case naveItems.SETTINGS:

					if (!userDetail.isUserSignedIn())
					{
						Debug.Log("user ui");
						//bottomBar.SetActive(false);
						sUIConnect.EnableUserUI();
					}
					else
					{
						Debug.Log("settings ui");
						//enableBottomBar();
						sVController.setSettingsViewActive();
						lVController.setLibraryViewFalse();
						libraryHighlight.SetActive(false); //shariz v2 11/9/2019  //Shariz 15th September 2019 v2
						settingsHighlight.SetActive(true); //shariz v2 11/9/2019  //Shariz 15th September 2019 v2
                        lensUI.SetActive(false); //Manuja 2/10/2023
                    }

                    lensIconHighlight.SetActive(false);
                    lensIconInactive.SetActive(true);

                    lensUI.SetActive(false);

                    break;
                case naveItems.LIBRARY:
                    Debug.Log("library");
                    enableBottomBar();
                    sVController.setSettingsViewToFalse();
                    lVController.setLibraryViewActive();
                    libraryHighlight.SetActive(true); //shariz v2 11/9/2019  //Shariz 15th September 2019 v2
                    settingsHighlight.SetActive(false); //shariz v2 11/9/2019  //Shariz 15th September 2019 v2
                    lensUI.SetActive(false); //Manuja 2/10/2023

                    lensIconHighlight.SetActive(false);
                    lensIconInactive.SetActive(true);
                    break;
                case naveItems.GURU:
                    SceneManager.LoadScene("ar portal");
                    break;
                case naveItems.LENS:
                    enableBottomBar();
                    sVController.setSettingsViewToFalse();
                    lVController.setLibraryViewFalse();
                    sUIConnect.closeUserUI();
                    libraryHighlight.SetActive(false);
                    settingsHighlight.SetActive(false);
                    lensUI.SetActive(true);

                    lensIconHighlight.SetActive(true);
                    lensIconInactive.SetActive(false);
                    break;
                default:
                    break;
            }
            
        }


        public void closeButton()
        {
            if (!userDetail.isUserSignedIn())
            {
                gotoLibrary();
            }
            else
            {
                gotoSettings();
            }
        }

        public void enableBottomBar()
        {
            bottomBar.SetActive(true);
            
        }

        public void hideBottomBar()
        {
            bottomBar.SetActive(false);
        }

    }


    
}
