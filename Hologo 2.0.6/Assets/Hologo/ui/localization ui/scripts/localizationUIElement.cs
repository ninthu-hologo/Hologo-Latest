using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; //Shariz 13th April 2020 v2.0.5

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 june 2019
    /// Modified by: Shariz 13th April 2020 v2.0.5
    /// this scriptable object contains countries and its languages
    /// </summary>

    public class localizationUIElement : iUILayoutBase, IPrefabLocalize
    {
        [Header("UI ELEMENTS")]
        [SerializeField]
        private TMP_Dropdown country;
        [SerializeField]
        private TMP_Dropdown language;

        [SerializeField]
        private GameObject backButton;// Shariz 1st Nov 2019 2.00

        [SerializeField]
        private settings_SO mainSettings;// Shariz 1st Nov 2019 2.00
        //[SerializeField]
        //private TMP_Dropdown syllabi;
        //[SerializeField]
        //private TMP_Dropdown curricula;

        [SerializeField]
        private Button submitButton; //Shariz 13th April 2020 v2.0.5



        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            //throw new System.NotImplementedException();
        }

        public void populateCountries(List<string> countries, int id)
        {
            populateDropDown(countries, country, id, true); //Shariz 12th April 2020 v2.0.5
        }

        public void populateLangauges(List<string> languages, int id)
        {
            if (languages.Count <= 0)
                return;

            populateDropDown(languages, language, id, false); //Shariz 12th April 2020 v2.0.5
        }

        public override void setMargins(float titleMargin, float bodyHeight, float footerMargin, float rightMargin, float leftMargin){
            if(footerMargin >0 && footerPanel != null)
            {
                footerPanel.sizeDelta = new Vector2(footerPanel.sizeDelta.x, footerPanel.sizeDelta.y + footerMargin);
            }
            if(mainSettings.firstTime == true){// Shariz 1st Nov 2019 2.00
                backButton.SetActive(false);
            }
        }
       
        //public void populatesyllabi(List<string> syllabi, int id)
        //{
        //    if (syllabi.Count <= 0)
        //        return;
        //    populateDropDown(syllabi, this.syllabi, id);
        //}

        //public void populatecurricula(List<string> curricula, int id)
        //{
        //    if (curricula.Count <= 0)
        //        return;
        //    populateDropDown(curricula, this.curricula, id);
        //}

        public int getChoosenCountryId()
        {
            return country.value - 1; //Shariz 12th April 2020 v2.0.5
        }

        public int getChoosenLanguageId()
        {
            return language.value;
        }

        //public int getChoosenSyllabusId()
        //{
        //    return syllabi.value;
        //}

        //public int getChoosenCurriculumId()
        //{
        //    return curricula.value;
        //}

        //Shariz 12th April 2020 v2.0.5
        void populateDropDown(List<string> options, TMP_Dropdown dropdown, int id, bool isCountry)  
        {
            dropdown.options.Clear();
            

            if (isCountry)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData("Select a Country"));
            } else
            {
                dropdown.captionText.text = options[id];
            }

            for (int i = 0; i < options.Count; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(options[i]));
            }
          //  dropdown.value = id;
            dropdown.SetValueWithoutNotify(id);
        }

        //Shariz 12th April 2020 v2.0.5
        public void LanguageAndSubmit(bool isOn)
        {
            language.interactable = isOn;
            submitButton.interactable = isOn;
        }
    }
}
