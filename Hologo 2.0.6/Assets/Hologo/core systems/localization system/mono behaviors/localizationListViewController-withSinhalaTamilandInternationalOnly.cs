using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 August 2019
    /// Modified by: Shariz - 15 Feb 2020 v2.0.4 : Please replace the whole script file
    /// This script has Sinhala and Tamil and International only
    /// this script controlls the library view and its functions
    /// </summary>
    public class localizationListViewControllerWithSinhalaTamilandInternationalOnly : MonoBehaviour
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
            populateCountriesDropDown();
        }

        public void populateCountriesDropDown()
        {
            localizeUI.getlocalizeUI().populateCountries(localizationList.getAllCountries(), countryId);
            populateLanguagesDropDown();
        }

        public void populateLanguagesDropDown()
        {
            localizeUI.getlocalizeUI().populateLangauges(localizationList.getLanguages(countryId), languageId);
        }

        public void onCountrySelected()
        {
            Debug.Log("country value changed");
            countryId = localizeUI.getlocalizeUI().getChoosenCountryId();
            Debug.Log(countryId);

            if (countryId != 3)
            {
                countryId = 0;
                languageId = 0;
                populateLanguagesDropDown();
            }
            else
            {
                populateLanguagesDropDown();
                onLangaugeSelected();                

            }

            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            // syllabusId = 0;
           // curriculumId = 0;
            
        }

        public void onLangaugeSelected()
        {
            languageId = localizeUI.getlocalizeUI().getChoosenLanguageId();
            localizationList.getSyllabi(languageId);
            syllabusId = 0;
            localizationList.getCurricula(syllabusId);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
        }



        public void doOrSkipAhead()
        {
        
            commonLoadingScreenSingleton.Singleton.enableLoadingScreen();// Shariz 12th Nov 2019 2.01            

            if (countryId == 3)
            {
               settings.setLocalizationSettings(localizationList.getSelectedCountry(countryId), localizationList.getSelectedLanguage(languageId),
                                              localizationList.getSelectedSyllabus(0), localizationList.getSelectedCurriculum(0));
                                                  }
            else
            {
                countryId = 0;
                localizationList.getLanguages(countryId);
                settings.setLocalizationSettings(localizationList.getSelectedCountry(countryId), localizationList.getSelectedLanguage(0),
                                                  localizationList.getSelectedLanguage(languageId).syllabi[0], localizationList.getSelectedLanguage(languageId).syllabi[0].curricula[0]);
            }


            // raise event if change language is triggered
            if(settings.LanguageChangeFlag)
            {
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

    }
}
