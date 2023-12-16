using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 August 2019
    /// Modified by: Shariz - 22 Feb 2020 v2.0.4 : Please replace the whole script file
    /// This script is for all countries selectable but all shows English Only!
    /// this script controlls the library view and its functions
    /// </summary>
    public class localizationListViewControllerWithAllEnglish : MonoBehaviour
    {

        [Header("DATA")]
        [SerializeField]
        private settings_SO settings;
        [SerializeField]
        private localizationList_SO localizationList;
        [Header("UI ELEMENT")]
        [SerializeField]
        private localizationUISwitcher localizeUI;
        [Header("EVENT")]
        [SerializeField]
        private event_SO eventForLocalizationSelection;
        [SerializeField]
        private event_SO eventForChangeLocalizationSelection;
        [Header("SCRIPTS")]
        [SerializeField]
        private localizationManager lManager;

        [SerializeField]
        private GameObject AppBootUI; // Shariz 12th Nov 2019 2.01


        // Shariz 2.0.4 17th Feb 2020
        [SerializeField]
        private userManager uManager;
        [SerializeField]
        private profileManager pManager;

        

        int countryId = 0;
        int languageId = 0;
        int syllabusId = 0;

        public void setLocalizationViewActive(int id = 0)
        {
            countryId = id;
            localizeUI.getlocalizeUI().gameObject.SetActive(true);
            setDropDownDefaults();
            AppBootUI.SetActive(false); // Shariz 12th Nov 2019 2.01
        }



        void setDropDownDefaults()
        {

            


            if (settings.country.id >0)
            {
                countryId = localizationList.getCountryListPosition(settings.country.id);
            }
            if (settings.language.id >0)
            {
                languageId = localizationList.getLanguageListPosition(settings.language.id);
            }

            /* Comment this block when making Sinhala Tamil Live - Shariz 2.0.4 17th Feb 2020 */
            /* Block Start */
            // if(settings.country.id != 0)
            // {

            //     countryId = localizationList.getCountryListPosition(settings.country.id);
            //     languageId = localizationList.getLanguageListPosition(settings.language.id);
            //     localizeUI.getlocalizeUI().populateCountries(localizationList.getAllCountries(), countryId);
            //     List<string> langs = new List<string>();
            //     langs.Add(localizationList.getLanguages(0)[0]);
            //     localizeUI.getlocalizeUI().populateLangauges(langs, 0);
            //     return;
            // }
            /* Block End */

            populateCountriesDropDown();
        }

        public void populateCountriesDropDown()
        {
            localizeUI.getlocalizeUI().populateCountries(localizationList.getAllCountries(), countryId);
            populateLanguagesDropDown();
        }

        public void populateLanguagesDropDown()
        {
            // Shariz 16th Feb 2020 v2.0.4 comment this out and comment below for Sinhala Tamil live
            // localizeUI.getlocalizeUI().populateLangauges(localizationList.getLanguages(countryId), languageId); 
            localizeUI.getlocalizeUI().populateLangauges(localizationList.getLanguages(0), 0);
            
        }

        public void onCountrySelected()
        {
            Debug.Log("country value changed");
            countryId = localizeUI.getlocalizeUI().getChoosenCountryId();
            Debug.Log(countryId);

            if (countryId == 0)
            {
                countryId = 0;
                languageId = 0;
                populateLanguagesDropDown();
            }
            else
            {
                /* Comment out this block when making Sinhala Tamil Live - Shariz 2.0.4 17th Feb 2020 */
                /* Block Start */
                populateLanguagesDropDown();
                onLangaugeSelected();
                /* Block End */

                /* Comment this block when making Sinhala Tamil Live - Shariz 2.0.4 17th Feb 2020 */
                /* Block Start */
                // localizationList.getLanguages(0);
                // List<string> langs = new List<string>();
                // // separating english and showing it only for language selection in sri lanka
                // langs.Add(localizationList.getLanguages(0)[0]);
                // localizeUI.getlocalizeUI().populateLangauges(langs, 0);
                /* Block End */
            }
            Debug.Log("on Country Selected countryId ID is "+countryId+ "selected language id is"+languageId);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            // syllabusId = 0;
           // curriculumId = 0;
            
        }

        //Shariz 16th Feb 2020 v2.0.4
        public void onLangaugeSelected()
        {
            languageId = localizeUI.getlocalizeUI().getChoosenLanguageId();
            syllabusId = 0;
            localizationList.getSyllabi(languageId);
            List<int> syllabi = localizationList.getSyllabiID(languageId);
                foreach(int asyllabi in syllabi){
                    int syllabusCurID = localizationList.getSyllabusListPosition(asyllabi);
                    int syllabiCountryID = localizationList.getSelectedSyllabus(syllabusCurID).country_id;
                    int selectedCountryID = localizationList.getSelectedCountry(countryId).id;
                    if(syllabiCountryID==selectedCountryID){
                        syllabusId = syllabusCurID;
                    }
                }
            

            


            Debug.Log("Selected syllabus ID is "+syllabusId);
            localizationList.getCurricula(syllabusId);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
        }



        public void doOrSkipAhead()
        {
            //Shariz 16th Feb 2020 v2.0.4 (Need to remove the if and keep only the first part when making Sinhala Tamil live)
            commonLoadingScreenSingleton.Singleton.enableLoadingScreen();// Shariz 12th Nov 2019 2.01            
            // if (countryId == 0)
            // {
               settings.setLocalizationSettings(localizationList.getSelectedCountry(countryId), localizationList.getSelectedLanguage(languageId),
                                              localizationList.getSelectedSyllabus(syllabusId), localizationList.getSelectedCurriculum(0));
            //                                       }
            // else
            // {
                // int countryIdDummy = 0; 
                // localizationList.getLanguages(countryIdDummy);
                // settings.setLocalizationSettings(localizationList.getSelectedCountry(countryId), localizationList.getSelectedLanguage(0),
                //                                   localizationList.getSelectedLanguage(0).syllabi[0], localizationList.getSelectedLanguage(languageId).syllabi[0].curricula[0]);
            // }


            // raise event if change language is triggered
            if(settings.LanguageChangeFlag)
            {
                //Shariz 17th Feb 2020 v2.0.4
                if(uManager.userData.isUserSignedIn()){
                    Debug.Log("User is signed in so country is set");
                    // asyncUpdateCountry();
                }


                Debug.Log("Language change is triggered");
                eventForChangeLocalizationSelection.raise(this.gameObject);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                return;
            }

            

            //need to load the language selected to the currentlanguage
            lManager.setCurrentLanguageAfterLanguageSelection();

            // raising event for localization selection
            eventForLocalizationSelection.raise(this.gameObject);
            // commonLoadingScreenSingleton.Singleton.disableLoadingScreen();// Shariz 12th Nov 2019 2.01
        }

        //Shariz 17th Feb 2020 v2.0.4
        async void asyncUpdateCountry(){
            Debug.Log("Async Country update is called!");
            bool success = await pManager.updateProfileToServer(pManager.userData.requestUserDetail().name,
                pManager.userData.requestUserDetail().contact_number, settings.language.id,
                localizationList.getSelectedCountry(countryId).id, settings.getPlatform());

            Debug.Log("The update details are  Name: "+pManager.userData.requestUserDetail().name+" and Number: "
            +pManager.userData.requestUserDetail().contact_number+" and language id is "+settings.language.id+" and Country ID is "+localizationList.getSelectedCountry(countryId).id+" and platform as "+settings.getPlatform());
            if (success)
            {
            Debug.Log("Country updated succesfuly!");
            }
            else
            {
            Debug.Log("Country has not been set!");
            }
        }

    }
}
