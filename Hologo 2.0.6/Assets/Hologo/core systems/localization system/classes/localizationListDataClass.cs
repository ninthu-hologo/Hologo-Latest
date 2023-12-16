using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 june 2019
    /// Modified by: 
    /// this class contains country and its languages
    /// </summary>
    /// 
    [System.Serializable]
    public class localizationListDataClass
    {
        public int id;
        public string name;
        public string created_at;
        public string updated_at;
        public List<allLanguagesInACountry> country_language;

    }


    [System.Serializable]
    public class allLanguagesInACountry
    {
        public int id;
        public int country_id;
        public int language_id;
        public languageDetails language;
    }


    [System.Serializable]
    public class languageDetails
    {
        public int id;
        public string name;
        public string code;
        public List<syllabiDetails> syllabi;
    }

    [System.Serializable]
    public class syllabiDetails
    {
        public int id;
        public string name;
        public string description;
        public int country_id;
        public int language_id;
        public List<curriculaDetails> curricula;
    }

    [System.Serializable]
    public class curriculaDetails
    {
        public int id;
        public string name;
        public int syllabus_id;
    }


}
