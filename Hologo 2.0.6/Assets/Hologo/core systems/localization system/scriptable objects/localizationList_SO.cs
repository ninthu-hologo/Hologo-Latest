using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 june 2019
    /// Modified by: 
    /// this scriptable object contains countries and its languages
    /// </summary>
    ///
    [CreateAssetMenu(fileName = "localizationList.asset", menuName = "Hologo V2/newlocalizationList")]
    public class localizationList_SO : ScriptableObject
    {
        [SerializeField]
        private List<localizationListDataClass> CountriesAndLanguages;
        private List<allLanguagesInACountry> selectedLanguages;
        private List<syllabiDetails> selectedSyllabi;
        private List<curriculaDetails> selectedCurricula;


        // getting the selected country
        public localizationListDataClass getSelectedCountry(int id)
        {
            return CountriesAndLanguages[id];
        }

        // getting the selected language
        public languageDetails getSelectedLanguage(int id)
        {
            return selectedLanguages[id].language;
        }

        // getting the selected syllabus
        public syllabiDetails getSelectedSyllabus(int id)
        {
            return selectedSyllabi[id];
        }
        // getting the selected curriculum
        public curriculaDetails getSelectedCurriculum(int id)
        {
            return selectedCurricula[id];
        }

        public int getCountryListPosition(int id)
        {
            for (int i = 0; i < CountriesAndLanguages.Count; i++)
            {
                if(CountriesAndLanguages[i].id == id)
                {
                    selectedLanguages = CountriesAndLanguages[i].country_language;
                    return i;
                }
            }
            return 0;
        }

        public int getLanguageListPosition(int id)
        {
            for (int i = 0; i < selectedLanguages.Count; i++)
            {
               // Debug.Log(id + "hshs");
               // Debug.Log(selectedLanguages[i].language.id + "ccc");
                if (selectedLanguages[i].language.id == id)
                {
                   // Debug.Log(i + "aaaa");
                    return i;
                }
            }
            return 0;
        }

        public int getSyllabusListPosition(int id)
        {
            for (int i = 0; i < selectedSyllabi.Count; i++)
            {
                if (selectedSyllabi[i].id == id)
                {
                    return i;
                }
            }
            return 0;
        }

        public int getCurriculumListPosition(int id)
        {
            for (int i = 0; i < selectedCurricula.Count; i++)
            {
                if (selectedCurricula[i].id == id)
                {
                    return i;
                }
            }
            return 0;
        }

        // check if file is loaded
        public bool isLocalizationListLoaded()
        {
            return CountriesAndLanguages.Count > 0;
        }

        // getting the total lisy
        public List<localizationListDataClass> getCountriesAndLangues()
        {
            return CountriesAndLanguages;
        }

        // sending all the names of countries in a string
        public List<string> getAllCountries()
        {
            List<string> countries = new List<string>();
            for (int i = 0; i < CountriesAndLanguages.Count; i++)
            {
                countries.Add(CountriesAndLanguages[i].name);
            }

            return countries;
        }

        // sending all the language names in a country
        // also we temporarily save language data list for syllabi query
        public List<string> getLanguages(int countryId)
        {
            List<string> languages = new List<string>();
            localizationListDataClass lldc = CountriesAndLanguages[countryId];
            selectedLanguages = lldc.country_language;
            for (int i = 0; i < selectedLanguages.Count; i++)
            {
                languages.Add(selectedLanguages[i].language.name);
            }

            return languages;
        }

        // sending all the syllabus names in languages
        // also we temporarily save syllabi data list for curriculum query
        public List<string> getSyllabi(int languageId)
        {
            List<string> syllabi = new List<string>();

            selectedSyllabi = selectedLanguages[languageId].language.syllabi;

            for (int i = 0; i < selectedSyllabi.Count; i++)
            {
                syllabi.Add(selectedSyllabi[i].name);
            }
            return syllabi;
        }

        //Shariz 16th Feb 2020 v2.0.4
        // sending all the syllabus names in languages
        // also we temporarily save syllabi data list for curriculum query
        public List<int> getSyllabiID(int languageId)
        {
            List<int> syllabi = new List<int>();

            selectedSyllabi = selectedLanguages[languageId].language.syllabi;

            for (int i = 0; i < selectedSyllabi.Count; i++)
            {
                syllabi.Add(selectedSyllabi[i].id);
            }
            return syllabi;
        }

        // sending all curriculums in syllabi
        // also we temporarily save curriculum data list for curriculum query
        public List<string> getCurricula(int syllabiId)
        {
            List<string> curricula = new List<string>();
            selectedCurricula = selectedSyllabi[syllabiId].curricula;
            for (int i = 0; i < selectedCurricula.Count; i++)
            {
                curricula.Add(selectedCurricula[i].name);
            }

            return curricula;
        }

        public void fillData(List<localizationListDataClass> localList)
        {
            CountriesAndLanguages = localList;
        }


        public void flushData()
         {
            if(CountriesAndLanguages != null)
                CountriesAndLanguages.Clear();
            if (selectedLanguages != null)
                selectedLanguages.Clear();
            if (selectedSyllabi != null)
                selectedSyllabi.Clear();
            if (selectedCurricula != null)
                selectedCurricula.Clear();
        }
    }
}
