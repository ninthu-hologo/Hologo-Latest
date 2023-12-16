using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 June 2019
    /// Modified by: 
    /// this scriptable object is a data container all the text localization
    /// </summary>
    [CreateAssetMenu(fileName = "languageData.asset", menuName = "Hologo V2/new language Data")]
    public class languageData_SO : ScriptableObject
    {
        public string country = "International";
        public string language = "English";
        public int languageId = 1;
        public bool rightToLeft = false;

        // font list
        [SerializeField]
        public List<fontSetting> fontList;

        // string lists
        [SerializeField]
        public List<languageString> buttonTexts;
        [SerializeField]
        public List<languageString> labelTexts;
        [SerializeField]
        public List<languageString> messageTexts;
        [SerializeField]
        public List<languageString> titleTexts;
        [SerializeField]
        public List<languageString> bodyTexts;


        // returns the font setting in the list corresponding to the id
        public fontSetting getAFont(int id)
        {
            return fontList[id];
        }

        // returning a string from id.
        public string getAButtonText(int id)
        {
            return buttonTexts[id].localizedPhrase;
        }
        public string getALabelText(int id)
        {
            return labelTexts[id].localizedPhrase;
        }
        public string getAMessageText(int id)
        {
            return messageTexts[id].localizedPhrase;
        }
        public string getATitleText(int id)
        {
            return titleTexts[id].localizedPhrase;
        }
        public string getAbodyText(int id)
        {
            return bodyTexts[id].localizedPhrase;
        }


        public bool isLanguageFilled()
        {
            return languageId > 0 && fontList[0].font != null;
        }


        public void setlanguageData(languageData_SO otherLanguage)
        {
            country = otherLanguage.country;
            language = otherLanguage.language;
            languageId = otherLanguage.languageId;
            rightToLeft = otherLanguage.rightToLeft;
            fontList = otherLanguage.fontList;
            buttonTexts = otherLanguage.buttonTexts;
            labelTexts = otherLanguage.labelTexts;
            titleTexts = otherLanguage.titleTexts;
            messageTexts = otherLanguage.messageTexts;
            bodyTexts = otherLanguage.bodyTexts;
        }


        public void flushData()
        {
            country = "";
            language = "";
            languageId = 0;
            rightToLeft = false;
            fontList = null;
            buttonTexts = null;
            labelTexts = null;
            titleTexts = null;
            messageTexts = null;
            bodyTexts = null;

        }

    }

    [System.Serializable]
    public class fontSetting
    {
        public string name;
        public TMP_FontAsset font;
        public TMPro.FontStyles fontStyle;
        public float characterSpacing = 0f;
        public bool AutoSize;
        [Header("FONT SIZE")]
        public int MinFontSize = 15;
        public int MaxFontSize = 32;
        [Header("ALTERNATE FONT SIZES")]
        public List<alternativeFonts> alternateFontSizes;
        public Color fontColor = Color.black;
        [Tooltip("this will reverse the font direction set globaly")]
        public bool overrideFontDirection = false;
        public TextAlignmentOptions textAlignment;
    }

    [System.Serializable]
    public class languageString
    {
        public string title;
        public string localizedPhrase;
    }

    [System.Serializable]
    public class alternativeFonts
    {
        public string title;
        public int MinFontSize;
        public int MaxFontSize;
    }
}



