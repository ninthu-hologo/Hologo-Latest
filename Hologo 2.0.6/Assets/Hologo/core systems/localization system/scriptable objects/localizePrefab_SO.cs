using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 June 2019
    /// Modified by: 
    /// this scriptable object is a data container for all the prefabs that need to localize. so when
    /// initializing the app we can assign language changes to the prefab in scriptable object memory 
    /// and use that to instantiate so we dont have to localize everytime a prefab is to be instantiated
    /// </summary>
    [CreateAssetMenu(fileName = "localLocalizeUI.asset", menuName = "Hologo V2/new Local Localize UI")]
    public class localizePrefab_SO : ScriptableObject, ILocalize
    {
        [SerializeField]
        languageData_SO language;

        [SerializeField]
        GameObject prefabToLocalize;

        // setting the language to incoming language
        public void setLanguage(languageData_SO language)
        {
            this.language = language;
        }

        public languageData_SO getLanguage()
        {
            return language;
        }
        //
        public GameObject givePrefab()
        {
            localizePrefab();
            return prefabToLocalize;
        }

        // this method will localize the prefab when executed
        public void localizePrefab()
        {
            IPrefabLocalize ipl = prefabToLocalize.GetComponent<IPrefabLocalize>();
            ipl.localizePrefab(language, this);
        }

        //a check to see if the assigned language is null
        public bool checkLanguageExits()
        {
            return language != null;
        }

        // this method is used by the prefab to set font / language etc...
        public void setTextConfig(TextMeshProUGUI label, fontSetting font, string text = null)
        {
            if (text != null)
            {
                label.text = text;
            }
            label.font = font.font;
            label.color = font.fontColor;
            label.fontStyle = font.fontStyle;
            label.characterSpacing = font.characterSpacing;
            label.enableAutoSizing = font.AutoSize;
            if (!font.AutoSize)
            {
                if (font.MinFontSize > 0)
                {
                    label.fontSize = font.MinFontSize;
                }
            }
            else
            {
                if (font.MinFontSize > 0)
                {
                    label.fontSizeMin = font.MinFontSize;
                }
                if (font.MaxFontSize > 0)
                {
                    label.fontSizeMax = font.MaxFontSize;
                }
            }
            if (font.overrideFontDirection)
            {
                label.isRightToLeftText = !language.rightToLeft;
            }
            else
            {
                label.isRightToLeftText = language.rightToLeft;
            }
            label.alignment = font.textAlignment;
        }

        // shariz adding localization for dropdown this method is used by the prefab to set font / language etc...
        public void setDropdownTextConfig(TMP_Dropdown label, fontSetting font, string text = null)
        {
            
            if (text != null)
            {
                label.captionText.text = text;
            }
            label.captionText.font = font.font;
            label.captionText.color = font.fontColor;
            label.captionText.fontStyle = font.fontStyle;
            label.captionText.characterSpacing = font.characterSpacing;
            label.captionText.enableAutoSizing = font.AutoSize;
            if (!font.AutoSize)
            {
                if (font.MinFontSize > 0)
                {
                    label.captionText.fontSize = font.MinFontSize;
                }
            }
            else
            {
                if (font.MinFontSize > 0)
                {
                    label.captionText.fontSizeMin = font.MinFontSize;
                }
                if (font.MaxFontSize > 0)
                {
                    label.captionText.fontSizeMax = font.MaxFontSize;
                }
            }
            if (font.overrideFontDirection)
            {
                label.captionText.isRightToLeftText = !language.rightToLeft;
            }
            else
            {
                label.captionText.isRightToLeftText = language.rightToLeft;
            }
            label.captionText.alignment = font.textAlignment;


            label.itemText.font = font.font;
            label.itemText.color = font.fontColor;
            label.itemText.fontStyle = font.fontStyle;
            label.itemText.characterSpacing = font.characterSpacing;
            label.itemText.enableAutoSizing = font.AutoSize;
            if (!font.AutoSize)
            {
                if (font.MinFontSize > 0)
                {
                    label.itemText.fontSize = font.MinFontSize;
                }
            }
            else
            {
                if (font.MinFontSize > 0)
                {
                    label.itemText.fontSizeMin = font.MinFontSize;
                }
                if (font.MaxFontSize > 0)
                {
                    label.itemText.fontSizeMax = font.MaxFontSize;
                }
            }
            if (font.overrideFontDirection)
            {
                label.itemText.isRightToLeftText = !language.rightToLeft;
            }
            else
            {
                label.itemText.isRightToLeftText = language.rightToLeft;
            }
            label.itemText.alignment = font.textAlignment;

        }



    }
}