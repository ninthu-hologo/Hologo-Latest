using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Linq; // Shariz 22nd March 2020 v2.0.4 fixing the | issue for onboarding localizations

namespace Hologo2
{
    /// <summary>
    /// Created by: Shariz - 22 Oct 2019
    /// Modified by: 
    /// experience ui element 
    /// </summary>
    public class onboardingSlides : MonoBehaviour, IPrefabLocalize
    {
        [Header("UI ELEMENTS")]
        [SerializeField]
        private Button nextButton;        
       [SerializeField]
        private Button previousButton;        
        
        [SerializeField]
        private Button skipButton;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private TextMeshProUGUI nextButtonText;        
       [SerializeField]
        private TextMeshProUGUI previousButtonText;        
        
        [SerializeField]
        private TextMeshProUGUI skipButtonText;

        [SerializeField]
        private TextMeshProUGUI closeButtonText;

        [SerializeField]
        private TextMeshProUGUI titleText;        
        
        [SerializeField]
        private TextMeshProUGUI bodyText;


        [SerializeField]
        private int getTitleFont;    
        [SerializeField]
        private int getTitleText;    
        
        [SerializeField]
        private int getBodyFont;    
        [SerializeField]
        private int getBodyText;    
        

        public int slideID; 

        [SerializeField]
        private Image BG;         
        
        [SerializeField]
        private Image line; 


        


       
        // localizing the texts in the onboarding slides
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(nextButtonText, language.getAFont(46), language.getALabelText(64));
            localizeSetting.setTextConfig(previousButtonText, language.getAFont(46), language.getALabelText(63));
            localizeSetting.setTextConfig(skipButtonText, language.getAFont(48), language.getALabelText(62));
            localizeSetting.setTextConfig(closeButtonText, language.getAFont(46), language.getALabelText(61));

            if(getTitleText != 0){
            localizeSetting.setTextConfig(titleText, language.getAFont(getTitleFont), language.getALabelText(getTitleText));
            } else {
            titleText.gameObject.SetActive(false);
            }
            if(getBodyText != 0){
            localizeSetting.setTextConfig(bodyText, language.getAFont(getBodyFont), language.getALabelText(getBodyText));
            }

            //shariz oct 30 hologo 2 
            if(slideID==99){
                localizeSetting.setTextConfig(previousButtonText, language.getAFont(48), language.getALabelText(63));
            }

            // Shariz 22nd March 2020 v2.0.4 fixing the | issue for onboarding localizations - start
            string textD = bodyText.text;
            int labelLanguageID = language.languageId;
            string[] lineD = new string[] {};
            string[] nonEnglishLine = new string[] {};


            // first replace \n with | so we can split the string
            textD = textD.Replace("\\n", "|");

            if(labelLanguageID==6){
                // splitting the string
                lineD = textD.Split('|');
                // after that we revert back to the \n
                textD = textD.Replace("|", "\n");
            } else {


            nonEnglishLine = textD.Split('|');
                // Shariz "This is for all the other languages in case the character | is used as a letter"
                List<string> fontEnglishStrings1 = ExtractStringsFromBody(textD,"<font=English>","</font>");
                List<string> fontEnglishStrings2 = ExtractStringsFromBody(textD,"<font=\"English\">","</font>");
                List<string> combinedFontEnglishStrings = fontEnglishStrings1.Union<string>(fontEnglishStrings2).ToList<string>();
                string[] FontEnglishArray = new string[] {};
                List<string> allSplitLists = new List<string> {};
                foreach(string combinedFontEnglishString in combinedFontEnglishStrings){

                    // splitting the string
                    FontEnglishArray = combinedFontEnglishString.Split('|');

                    allSplitLists.AddRange(FontEnglishArray);
                    
                    // after that we revert back to the \n
                    textD = textD.Replace("|", "\n");
                }
                lineD = allSplitLists.ToArray(); 



            }
            // we replace back \n with acutal \n
            // so textmesh pro will properly make a new line
            textD = textD.Replace("\\n", "\n");
            bodyText.text = textD;

            // Shariz 22nd March 2020 v2.0.4 fixing the | issue for onboarding localizations - end
        }

        // Shariz 22nd March 2020 v2.0.4  this is to make sure | is used as the correct Sinhala/Tamil letter and not as a line break
        private static List<string> ExtractStringsFromBody(string body, string start, string end)
        {
            List<string> matched = new List<string>();

            int indexStart = 0;
            int indexEnd = 0;

            bool exit = false;
            while (!exit)
            {
                indexStart = body.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);

                    matched.Add(body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length));

                    body = body.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }

            return matched;
        }


        // return previous button
        public Button getPrevButton (){
            return previousButton;
        }

        // return next button
        public Button getNextButton (){
            return nextButton;
        }

        // return skip button
        public Button getSkipButton (){
            return skipButton;
        }
        // return close button
        public Button getCloseButton (){
            return closeButton;
        }
    }
}
