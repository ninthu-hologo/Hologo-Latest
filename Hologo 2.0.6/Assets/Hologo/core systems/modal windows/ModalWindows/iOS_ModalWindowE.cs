using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Hologo2;


namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// this modal window displays a title,description. user user input field and ok or cancel buttons
    /// </summary>
    public class iOS_ModalWindowE : MonoBehaviour, IPrefabLocalize
    {

        [SerializeField]

        public RectTransform Window;
        public Button Cancel;
        public Button OK;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TMP_InputField Input;
        public TextMeshProUGUI PlaceHolderText;

        private CanvasGroup windowCanvas;


        public TextMeshProUGUI OkText;
        public TextMeshProUGUI CancelText;
        // private float height;

        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();
        }




        public void FillInput(bool close,string title, string description, string placeholder,
                              UnityAction<string> ok ,UnityAction cancel = null)
        {
            Input.text = "";
            InfoDisplay(title, description, placeholder);

            Cancel.onClick.RemoveAllListeners();
            OK.onClick.RemoveAllListeners();

            if(cancel !=null)
            {
                Cancel.onClick.AddListener(cancel); 
            }

            OK.onClick.AddListener(()=>ok(Input.text));
            if(close)
            {
                OK.onClick.AddListener(closeWindow);
            }

            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }


        void AddCloseWindowFunctionToAllButtons()
        {
            Cancel.onClick.AddListener(closeWindow);
           
            windowMain.Background.onClick.AddListener(closeWindow);
        }



        void InfoDisplay(string title, string description, string placeholder)
        {
            Title.text = title;
            Description.text = description;
            PlaceHolderText.text = placeholder;
        }



        public void OpenWindow()
        {
            Window.gameObject.SetActive(true);
            windowMain.EnableBG();
            LeanTween.scale(Window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }
        public void closeWindow()
        {
            // LeanTween.moveY(Window, 0, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            LeanTween.scale(Window, new Vector3(0.5f, 0.5f, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(CleanWindow); ;

            windowMain.CloseBG();

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(OkText, language.getAFont(19), language.getAButtonText(0));
            localizeSetting.setTextConfig(CancelText, language.getAFont(8), language.getAButtonText(1));
            localizeSetting.setTextConfig(Title, language.getAFont(20));
            localizeSetting.setTextConfig(Description, language.getAFont(17));
            localizeSetting.setTextConfig(PlaceHolderText, language.getAFont(9));
        }


    }
}
