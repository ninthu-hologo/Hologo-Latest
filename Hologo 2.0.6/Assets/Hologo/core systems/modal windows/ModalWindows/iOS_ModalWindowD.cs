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
    /// iOs modal window D.this is an alert window. displays a success bool icon. title and description.
    /// </summary>
    public class iOS_ModalWindowD : MonoBehaviour, IPrefabLocalize
    {


        public RectTransform Window;
        public GameObject Icon;
        public Button State;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI StateText;
        public Image IconSprite;
        // public Sprite Warning;
        // public Sprite Success;
        private Color warningColor = new Color32(224, 32, 32, 255);
        private Color successColor = new Color32(73, 197, 49, 255);


        private CanvasGroup windowCanvas;


        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();
        }

        public void ShowInfo(bool showIcon,bool success,string title, string description, string stateText, UnityAction state = null)
        {
            Icon.SetActive(showIcon);
            windowMain.EnableBG();
            InfoDisplay(title, description);

            if(success)
            {
                IconSprite.color = successColor;
                //IconSprite.color = Color.blue; 
                //changing to orange
              //  IconSprite.color = new Color32(255, 111, 0, 255);;
              Debug.Log("this is a success");
            }
            else
            {
                //IconSprite.sprite = Warning;
                IconSprite.color = warningColor;
              Debug.Log("this is a warning");
            }

            State.gameObject.SetActive(true);
            State.onClick.RemoveAllListeners();

            if(state != null)
            {
                State.onClick.AddListener(state); 
            }

            State.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
            OpenWindow();

        }

        void InfoDisplay(string title, string description)
        {
            Title.text = title;
            Description.text = description;
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
            LeanTween.scale(Window, new Vector3(1.4f, 1.4f, 1), 0.3f).setEase(LeanTweenType.easeOutBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow); ;

            windowMain.CloseBG();

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(Title, language.getAFont(24));
            localizeSetting.setTextConfig(Description, language.getAFont(12));
            localizeSetting.setTextConfig(StateText, language.getAFont(12));
        }
    }
}
