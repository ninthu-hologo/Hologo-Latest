using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by: Shariz (qdexterious@gmail.com) 10-05-2018
    /// iOs modal window G.displays a title. with two customzable optin buttons
    /// </summary>
    public class iOS_ModalWindowG : MonoBehaviour
    {

        public RectTransform Window;
        public GameObject SeparatorOne;
        public GameObject SeparatorTwo;
        public GameObject SeparatorThree;
        public Image ModalImage;
        public Button Cancel;
        public Button OK;
        public Button Delete;
        public Button Option;
        //public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Optional;
        public TextMeshProUGUI CustomDelete;

        private CanvasGroup windowCanvas;

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




        public void ChoiceMaker(ButtonOptions bo, bool delcustom, string description,
                                string deletetext, string optiontext, UnityAction ok = null
                                , UnityAction delete = null, UnityAction option = null, UnityAction cancel = null)
        {
            InfoDisplay(description);
            EnableButtons(bo);
            Cancel.onClick.RemoveAllListeners();
            OK.onClick.RemoveAllListeners();
            Delete.onClick.RemoveAllListeners();
            Option.onClick.RemoveAllListeners();

            if (delcustom)
            {
                CustomDelete.text = deletetext;
            }
            else
            {
                CustomDelete.text = "<font=English>Delete</font>";  // Shariz 23rd August 2020 v2.1.0 adding normal update notification
            }

            // if(windowImage){
            //     ModalImage.gameObject.SetActive(true);
            //     ModalImage = windowImage;
            // }
            // else {
            //     ModalImage.gameObject.SetActive(false);
            // }

            Optional.text = optiontext;


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
            Delete.onClick.AddListener(closeWindow);
            OK.onClick.AddListener(closeWindow);
            Option.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
        }



        void InfoDisplay(string description)
        {

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
            LeanTween.scale(Window, new Vector3(1.4f, 1.4f, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(CleanWindow); ;

            windowMain.CloseBG();

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
    }
}
