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
    /// Modified by: Shariz 26th May 2020
    /// iOs modal window C.this modal window displays a list of buttons three options/with text and cancel and delete button and also a title and a description
    ///
    /// !IMPORTANT - Changing this moalwindow into a payment method selector
    /// </summary>
    public class iOS_ModalWindowC : MonoBehaviour
    {

        public RectTransform Window;
        public Button Cancel;
        public Button OptionOne;
        public Button OptionTwo;
        public Button OptionThree;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI OptionOneText;
        public TextMeshProUGUI OptionTwoText;
        public TextMeshProUGUI OptionThreeText;

        //shariz 2.0.6 26th May adding extra fields needed for payment methods
        public Image ImageOne;
        public Image ImageTwo;
        public Image ImageThree;
        public GameObject ImageOneArea;
        public GameObject ImageTwoArea;
        public GameObject ImageThreeArea;
        public TextMeshProUGUI OptionOneDescription;
        public TextMeshProUGUI OptionTwoDescription;
        public TextMeshProUGUI OptionThreeDescription;
        public GameObject ChevronOne;
        public GameObject ChevronTwo;
        public GameObject ChevronThree;
        public RectTransform modalWindow;
        [SerializeField]
        private settings_SO mainSettings;

        private CanvasGroup windowCanvas;


        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();

            //Shariz 2.0.6 26th May 2020 making window bigger for tablet
            if (mainSettings && mainSettings.device == deviceType.TABLET)
            {
                modalWindow.sizeDelta = new Vector2(570, modalWindow.sizeDelta.y);
            } else if(mainSettings)
            {
                modalWindow.sizeDelta = new Vector2(375, modalWindow.sizeDelta.y);
            }
        }

        //shariz 2.0.6 26th May passing additional needed data for payment choice maker
        public void ChoiceMaker(string title, string description, string optionOneText, string optionTwoText,string optionThreeText,
            bool oneChevron, bool twoChevron, bool threeChevron, string optionOneDescription = null, string optionTwoDescription = null,
            string optionThreeDescription = null, Sprite imageOne = null, Sprite imageTwo = null, Sprite imageThree = null,
            UnityAction optionOne = null,UnityAction optionTwo = null,UnityAction optionThree = null, UnityAction cancel = null)
        {
            windowMain.EnableBG();
            InfoDisplay(title, description);

            Cancel.gameObject.SetActive(true);
            OptionOne.gameObject.SetActive(false);
            OptionTwo.gameObject.SetActive(false);
            OptionThree.gameObject.SetActive(false);
            ChevronOne.SetActive(false);
            ChevronTwo.SetActive(false);
            ChevronThree.SetActive(false);
            ImageOneArea.gameObject.SetActive(false);
            ImageTwoArea.gameObject.SetActive(false);
            ImageThreeArea.gameObject.SetActive(false);
            OptionOneDescription.gameObject.SetActive(false);
            OptionTwoDescription.gameObject.SetActive(false);
            OptionThreeDescription.gameObject.SetActive(false);

            Cancel.onClick.RemoveAllListeners();


            if (cancel != null)
            {
                Cancel.onClick.AddListener(cancel);
                windowMain.Background.onClick.AddListener(cancel);
            }

            if (optionOne != null)
            {
                OptionOne.gameObject.SetActive(true);
                OptionOne.onClick.RemoveAllListeners();
                OptionOne.onClick.AddListener(optionOne);
                OptionOneText.text = optionOneText;
                if(oneChevron)
                {
                    ChevronOne.SetActive(true);
                }
                if(optionOneDescription != null)
                {
                    OptionOneDescription.gameObject.SetActive(true);
                    OptionOneDescription.text = optionOneDescription;
                }
                if(imageOne != null)
                {
                    ImageOneArea.gameObject.SetActive(true);
                    ImageOne.sprite = imageOne;
                }
            }
            if (optionTwo != null)
            {
                OptionTwo.gameObject.SetActive(true);
                OptionTwo.onClick.RemoveAllListeners();
                OptionTwo.onClick.AddListener(optionTwo);
                OptionTwoText.text = optionTwoText;
                if (twoChevron)
                {
                    ChevronTwo.SetActive(true);
                }
                if (optionTwoDescription != null)
                {
                    OptionTwoDescription.gameObject.SetActive(true);
                    OptionTwoDescription.text = optionTwoDescription;
                }
                if (imageTwo != null)
                {
                    ImageTwoArea.gameObject.SetActive(true);
                    ImageTwo.sprite = imageTwo;
                }
            }
            if (optionThree != null)
            {
                OptionThree.gameObject.SetActive(true);
                OptionThree.onClick.RemoveAllListeners();
                OptionThree.onClick.AddListener(optionThree);
                OptionThreeText.text = optionThreeText;
                if (threeChevron)
                {
                    ChevronThree.SetActive(true);
                }
                if (optionThreeDescription != null)
                {
                    OptionThreeDescription.gameObject.SetActive(true);
                    OptionThreeDescription.text = optionThreeDescription;
                }
                if (imageThree != null)
                {
                    ImageThreeArea.gameObject.SetActive(true);
                    ImageThree.sprite = imageThree;
                }
            }

            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }





        void AddCloseWindowFunctionToAllButtons()
        {
            Cancel.onClick.AddListener(closeWindow);
            OptionOne.onClick.AddListener(closeWindow);
            OptionTwo.onClick.AddListener(closeWindow);
            OptionThree.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
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
            //LeanTween.scale(Window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.moveY(modalWindow, 0f, 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }
        public void closeWindow()
        {
            //LeanTween.scale(Window, new Vector3(1.4f, 1.4f, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(CleanWindow);
            LeanTween.moveY(modalWindow, -modalWindow.sizeDelta.y, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            windowMain.CloseBG();
        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }






    }

}
