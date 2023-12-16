using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq; // Shariz 29th Feb 2020 v2.0.4

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 16 july 2019
    /// Modified by: 
    /// class ui item to populate a label of a experience
    /// </summary>
    public class labelElement : MonoBehaviour, IPrefabLocalize
    {
        [Header("label UI ELEMENTS")]
        [SerializeField]
        private RectTransform labelCanvas;
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private BoxCollider bCollider;
        private bool isShown;
        [Header("LABEL CONFIG")]
        [SerializeField]
        private float charWidth = 7.4f;
        [SerializeField]
        private float charHeight = 13f;
        [SerializeField]
        [Tooltip("the rects x,y,width,height is right,left,up,down padding respectively")]
        private Rect padding = new Rect(31f, 31f, 15f, 15f);
        [SerializeField]
        private labelAlignment labelAlign;
        [Header("PREVIEW TESTING ONLY")]
        [SerializeField]
        [Tooltip("do not use this for final label title! this for testing only")]
        private string labelText;

        // Shariz 29th Feb 2020 v2.0.4
        [SerializeField]
        private int labelLanguageID;


        public bool isLabelActive()
        {
            return isShown;
        }

        public void fillWithData(string text, labelAlignment alignment, bool enabled)
        {

            resizeLabel(text, alignment);
            isShown = enabled;
            gameObject.SetActive(isShown);
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
          localizeSetting.setTextConfig(label, localizeSetting.getLanguage().getAFont(4));
          
          labelLanguageID = language.languageId; // Shariz 29th Feb 2020 v2.0.4
        }


        // Shariz 29th Feb 2020 v2.0.4 replace this whole method and add the ExtractStringsFromBody method
        public void resizeLabel(string text, labelAlignment alignment)
        {
            charHeight = 17.6f;
            // Shariz 29th Feb 2020 v2.0.4
            // Debug.Log("this elements language is "+labelLanguageID);
            string[] line = new string[] {};
            string[] nonEnglishLine = new string[] {};
            labelAlign = alignment;
            // first replace \n with | so we can split the string
            text = text.Replace("\\n", "|");

            

            // Shariz 29th Feb 2020 v2.0.4 "Do as before if the language is English"
            if(labelLanguageID==6){
                // splitting the string
                line = text.Split('|');
                // after that we revert back to the \n
                text = text.Replace("|", "\n");
            } else {
                nonEnglishLine = text.Split('|');
                // Shariz 29th February 2020 v2.0.4 "This is for all the other languages in case the character | is used as a letter"
                List<string> fontEnglishStrings1 = ExtractStringsFromBody(text,"<font=English>","</font>");
                List<string> fontEnglishStrings2 = ExtractStringsFromBody(text,"<font=\"English\">","</font>");
                List<string> combinedFontEnglishStrings = fontEnglishStrings1.Union<string>(fontEnglishStrings2).ToList<string>();
                string[] FontEnglishArray = new string[] {};
                List<string> allSplitLists = new List<string> {};
                foreach(string combinedFontEnglishString in combinedFontEnglishStrings){

                    // splitting the string
                    FontEnglishArray = combinedFontEnglishString.Split('|');

                    allSplitLists.AddRange(FontEnglishArray);
                    
                    // after that we revert back to the \n
                    text = text.Replace("|", "\n");
                }
                line = allSplitLists.ToArray(); 

                
                
                
                
            }

            // we replace back \n with acutal \n
            // so textmesh pro will properly make a new line
            text = text.Replace("\\n", "\n");

            int longestLine = 0;

            int nonEnglishLongestLine = 0;

            if(labelLanguageID!=6){
                // we get the char count of the longest line
                // to use it in determining the width of the label
                // but this one is for Languages other than English
                for (int i = 0; i < nonEnglishLine.Length; i++)
                {
                    // Shariz 24th Feb 2020 v2.0.4
                    nonEnglishLine[i] = nonEnglishLine[i].Replace("<font=\"English\">","");
                    nonEnglishLine[i] = nonEnglishLine[i].Replace("<font=English>","");
                    nonEnglishLine[i] = nonEnglishLine[i].Replace("</font>","");
                    
                    if (nonEnglishLongestLine == 0)
                    {
                        nonEnglishLongestLine = nonEnglishLine[i].Length;
                    }
                    else
                    {
                        if (nonEnglishLine[i].Length > nonEnglishLongestLine)
                        {
                            nonEnglishLongestLine = nonEnglishLine[i].Length;
                        }
                    }
                }
            }
            
           

            // we get the char count of the longest line
            // to use it in determining the width of the label
            for (int i = 0; i < line.Length; i++)
            {
                // Shariz 24th Feb 2020 v2.0.4
                line[i] = line[i].Replace("<font=\"English\">","");
                line[i] = line[i].Replace("<font=English>","");
                line[i] = line[i].Replace("</font>","");
                
                if (longestLine == 0)
                {
                    longestLine = line[i].Length;
                }
                else
                {
                    if (line[i].Length > longestLine)
                    {
                        longestLine = line[i].Length;
                    }
                }
            }

            

            
            float width = 0;
            // Changing line break length depending on Language
            if(labelLanguageID==6){
                // width is calculated using the longestline and
                // right and left padding
                width = (longestLine * charWidth) + padding.x + padding.y;
            }
            else {
                // width is calculated using the longestline and
                // right and left padding
                width = (nonEnglishLongestLine * charWidth) + padding.x + padding.y;
            }
            // height is determined by the number of new lines and
            // upper and lower padding
            float height = (line.Length * charHeight) + padding.width + padding.height;
            labelCanvas.sizeDelta = new Vector2(width, height);
            bCollider.size = new Vector3(width, height, 1f);
            bCollider.center = Vector3.zero;
            label.text = text;
            alignLabel(width, height);
        }

        // Shariz 29th Feb 2020 v2.0.4 this is to make sure | is used as the correct Sinhala/Tamil letter and not as a line break
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


        void alignLabel(float width, float height)
        {
            switch (labelAlign)
            {
                case labelAlignment.RIGHT:
                    labelCanvas.anchoredPosition = new Vector3(width / 2, 0, 0);
                    break;
                case labelAlignment.LEFT:
                    labelCanvas.anchoredPosition = new Vector3((width / 2) * -1, 0, 0);
                    break;
                case labelAlignment.TOP:
                    labelCanvas.anchoredPosition = new Vector3(0, (height / 2) * -1, 0);
                    break;
                case labelAlignment.BOTTOM:
                    labelCanvas.anchoredPosition = new Vector3(0, height / 2, 0);
                    break;
                default:
                    break;
            }
        }

        public void previewLabel()
        {
            fillWithData(labelText, labelAlign, true);
        }
    }

    

    public enum labelAlignment { RIGHT, LEFT, TOP, BOTTOM }
}
