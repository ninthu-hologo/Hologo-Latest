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
    /// iOs modal window B. displays a title, description and a cancel and an customizable option button
    /// </summary>
    public class iOS_ModalWindowB : MonoBehaviour, IPrefabLocalize
    {

        public RectTransform Window;
        public GameObject SeparatorOne;
        public GameObject SeparatorTwo;
        public GameObject SeparatorThree;
        public Button Cancel;
        public Button OK;
        public Button Delete;
        public Button Option;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Optional;
        public TextMeshProUGUI CustomDelete;

        public CanvasGroup windowCanvas;

        public TextMeshProUGUI OKText;
        public TextMeshProUGUI CancelText;
        public TextMeshProUGUI OptionalText;//SHARIZ 29th Oct 2019 2.00
        public Image imageIcon;//SHARIZ 29th Oct 2019 2.00
        public GameObject imageHolder;//SHARIZ 29th Oct 2019 2.00

        public enum ButtonOptions
        {
            CancelOk,
            CancelDel,
            CancelOpt,
            Ok,
            Opt,
            OptOk,
            OptDel,
            OkDel,

        }

        // private float height;

        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();
        }



        // Shariz 29th Oct 2019 2.00
        public void ChoiceMaker(bool hasImage, ButtonOptions bo, string title, string description,
                                string deletetext, string optiontext, Sprite iconSprite, UnityAction ok = null
                                , UnityAction delete = null, UnityAction option = null,UnityAction cancel = null)
        {
            InfoDisplay(title, description);
            EnableButtons(bo);
            Cancel.onClick.RemoveAllListeners();
            OK.onClick.RemoveAllListeners();
            Delete.onClick.RemoveAllListeners();
            Option.onClick.RemoveAllListeners();
            Debug.Log("Opened modal window B");

            // Shariz 23rd August 2020 v2.1.0 adding normal update notification and enabling custom delete text
            if (deletetext != null)
            {
                CustomDelete.text = deletetext;
            }
            else
            {
                CustomDelete.text = "<font=English>Delete</font>";  // Shariz 23rd August 2020 v2.1.0 adding normal update notification
            }

            Optional.text = optiontext;

            // Shariz 29th Oct 2019 2.00
            if(hasImage){
                imageHolder.SetActive(true);
                imageIcon.sprite = iconSprite;
            } else {
                imageHolder.SetActive(false);
            }


            switch (bo)
            {
                case ButtonOptions.CancelOk:
                    if (cancel != null)
                    {
                        Cancel.onClick.AddListener(cancel);
                    }
                    if (ok != null)
                    {
                        OK.onClick.AddListener(ok);
                    }
                                Debug.Log("MW B OKay or Cancel");

                    break;
                case ButtonOptions.CancelDel:
                    if (cancel != null)
                    {
                        Cancel.onClick.AddListener(cancel);
                    }
                    if (delete != null)
                    {
                        Delete.onClick.AddListener(delete);

                    }
                    break;
                case ButtonOptions.CancelOpt:
                    if (cancel != null)
                    {
                        Cancel.onClick.AddListener(cancel);
                    }
                    if (option != null)
                    {
                        Option.onClick.AddListener(option);
                    }
                    break;
                case ButtonOptions.Ok:

                    if (ok != null)
                    {
                        OK.onClick.AddListener(ok);
                    }

                    break;
                case ButtonOptions.Opt:
                    if (option != null)
                    {
                        Option.onClick.AddListener(option);
                    }
                    break;
                case ButtonOptions.OptOk:
                    if (option != null)
                    {
                        Option.onClick.AddListener(option);
                    }
                    if (ok != null)
                    {
                        OK.onClick.AddListener(ok);
                    }
                    break;
                case ButtonOptions.OptDel:
                    if (option != null)
                    {
                        Option.onClick.AddListener(option);
                    }
                    if (delete != null)
                    {
                        Delete.onClick.AddListener(delete);

                    }
                    break;
                case ButtonOptions.OkDel:
                    if (ok != null)
                    {
                        OK.onClick.AddListener(ok);
                    }
                    if (delete != null)
                    {
                        Delete.onClick.AddListener(delete);

                    }
                    break;

            }

            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }


        void AddCloseWindowFunctionToAllButtons()
        {
            Cancel.onClick.AddListener(closeWindow);
            OK.onClick.AddListener(closeWindow);
            Option.onClick.AddListener(closeWindow);
            // windowMain.Background.onClick.AddListener(closeWindow);
            // Debug.Log("Modal window B Delete on click closewindow listener");
            Delete.onClick.AddListener(closeWindow);
        }



        void InfoDisplay(string title, string description)
        {
            Title.text = title;
            Description.text = description;
        }



        public void OpenWindow()
        {
            Window.gameObject.SetActive(true);
            // windowMain.EnableBG();
            LeanTween.scale(Window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }
        public void closeWindow()
        {
            Debug.Log("Modal window B close window in process");
            // LeanTween.moveY(Window, 0, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            LeanTween.scale(Window, new Vector3(0.6f, 0.6f, 0.6f), 0.3f).setEase(LeanTweenType.easeOutBack);// Shariz 27th Oct 2019 2.00
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.2f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow); 
           
            // windowMain.CloseBG();

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }



        void EnableButtons(ButtonOptions bo)
        {
            switch (bo)
            {
                case ButtonOptions.CancelOk:
                    Cancel.gameObject.SetActive(true);
                    SeparatorOne.SetActive(true);
                    Option.gameObject.SetActive(false);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(true);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(false);
                    break;
                case ButtonOptions.CancelDel:
                    Cancel.gameObject.SetActive(true);
                    SeparatorOne.SetActive(true);
                    Option.gameObject.SetActive(false);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(false);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(true);
                    break;
                case ButtonOptions.CancelOpt:
                    Cancel.gameObject.SetActive(true);
                    SeparatorOne.SetActive(true);
                    Option.gameObject.SetActive(true);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(false);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(false);
                    break;
                case ButtonOptions.Ok:
                    Cancel.gameObject.SetActive(false);
                    SeparatorOne.SetActive(false);
                    Option.gameObject.SetActive(false);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(true);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(false);
                    break;
                case ButtonOptions.Opt:
                    Cancel.gameObject.SetActive(false);
                    SeparatorOne.SetActive(false);
                    Option.gameObject.SetActive(true);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(false);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(false);
                    break;
                case ButtonOptions.OptOk:
                    Cancel.gameObject.SetActive(false);
                    SeparatorOne.SetActive(false);
                    Option.gameObject.SetActive(true);
                    SeparatorTwo.SetActive(true);
                    OK.gameObject.SetActive(true);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(false);
                    break;
                case ButtonOptions.OptDel:
                    Cancel.gameObject.SetActive(false);
                    SeparatorOne.SetActive(false);
                    Option.gameObject.SetActive(true);
                    SeparatorTwo.SetActive(true);
                    OK.gameObject.SetActive(false);
                    SeparatorThree.SetActive(false);
                    Delete.gameObject.SetActive(true);
                    break;
                case ButtonOptions.OkDel:
                    Cancel.gameObject.SetActive(false);
                    SeparatorOne.SetActive(false);
                    Option.gameObject.SetActive(false);
                    SeparatorTwo.SetActive(false);
                    OK.gameObject.SetActive(true);
                    SeparatorThree.SetActive(true);
                    Delete.gameObject.SetActive(true);
                    break;

            }
        }


         public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(OKText, language.getAFont(8), language.getAButtonText(0));//SHARIZ 29th Oct 2019 2.00
            localizeSetting.setTextConfig(OptionalText, language.getAFont(8));//SHARIZ 29th Oct 2019 2.00
            localizeSetting.setTextConfig(CancelText, language.getAFont(19), language.getAButtonText(1));//SHARIZ 29th Oct 2019 2.00
            localizeSetting.setTextConfig(Title, language.getAFont(28));
            localizeSetting.setTextConfig(Description, language.getAFont(3));
        }
    }
}
