using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 June 2019
    /// Modified by: 
    /// this scriptable object is a data container all the text localization
    /// </summary>
    [CustomEditor(typeof(languageData_SO))]
    public class languageCustomInspector : Editor
	{
        languageData_SO language;
        bool showAllFields = true;
        bool showFonts = false;
        bool showButtons = false;
        bool showLabels = false;
        bool showMessages = false;
        bool showTitles = false;
        bool showDescriptions = false;
        string lan = "language";
        string gF = ".getAFont(";
        string gB = ".getAButtonText(";
        string gL = ".getALabelText(";
        string gM = ".getAMessageText(";
        string gT = ".getATitleText(";
        string gD = ".getAbodyText(";
        public override void OnInspectorGUI() 
        {
            showAllFields = GUILayout.Toggle(showAllFields, "show edit fields");
            if (!showAllFields)
            {
                showFonts = GUILayout.Toggle(showFonts, "show fonts");
                showButtons = GUILayout.Toggle(showButtons, "show buttons");
                showLabels = GUILayout.Toggle(showLabels, "show labels");
                showMessages = GUILayout.Toggle(showMessages, "show messages");
                showTitles = GUILayout.Toggle(showTitles, "show titles");
                showDescriptions = GUILayout.Toggle(showDescriptions, "show descriptions");
            }

            if (showAllFields)
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("flush data"))
                {
                    language.flushData();
                }
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL FONTS");
                //base.OnInspectorGUI();
                if (showFonts)
                {
                    for (int i = 0; i < language.fontList.Count; i++)
                    {
                        EditorGUILayout.LabelField(language.fontList[i].name);
                        if (GUILayout.Button("copy"))
                        {
                            EditorGUIUtility.systemCopyBuffer = lan + gF + i + ")";
                        }

                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL BUTTON TEXTS");
                if (showButtons)
                {
                    for (int i = 0; i < language.buttonTexts.Count; i++)
                    {
                        EditorGUILayout.LabelField(language.buttonTexts[i].title);
                        if (GUILayout.Button("copy"))
                        {
                            EditorGUIUtility.systemCopyBuffer = lan + gB + i + ")";
                        }

                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL LABEL TEXTS");
                if (showLabels)
                {
                    for (int i = 0; i < language.labelTexts.Count; i++)
                    {
                        EditorGUILayout.LabelField(language.labelTexts[i].title);
                        if (GUILayout.Button("copy"))
                        {
                            EditorGUIUtility.systemCopyBuffer = lan + gL + i + ")";
                        }

                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL MESSAGE TEXTS");
                if(showMessages)
                for (int i = 0; i < language.messageTexts.Count; i++)
                {
                    EditorGUILayout.LabelField(language.messageTexts[i].title);
                    if (GUILayout.Button("copy"))
                    {
                        EditorGUIUtility.systemCopyBuffer = lan + gM + i + ")";
                    }

                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL TITLE TEXTS");
                if (showTitles)
                {
                    for (int i = 0; i < language.titleTexts.Count; i++)
                    {
                        EditorGUILayout.LabelField(language.titleTexts[i].title);
                        if (GUILayout.Button("copy"))
                        {
                            EditorGUIUtility.systemCopyBuffer = lan + gT + i + ")";
                        }

                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ALL BODY TEXTS");
                if (showDescriptions)
                {
                    for (int i = 0; i < language.bodyTexts.Count; i++)
                    {
                        EditorGUILayout.LabelField(language.bodyTexts[i].title);
                        if (GUILayout.Button("copy"))
                        {
                            EditorGUIUtility.systemCopyBuffer = lan + gD + i + ")";
                        }

                    }
                }
            }

            

        }



        void OnEnable()
        {
            language = target as languageData_SO;
            
        }

    }


    
}

