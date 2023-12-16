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
    /// this modal window displays a list of buttons three options/with text and cancel and delete button
    /// </summary>
    public class IOS_ModalWindowA : MonoBehaviour, IPrefabLocalize
    {


        public RectTransform Window;
        public Button Cancel;
        public Button Delete;
        public Button OptionOne;
        public Button OptionTwo;
        public Button OptionThree;
        public GameObject Notification;
        public TextMeshProUGUI TextOne;
        public TextMeshProUGUI TextTwo;
        public TextMeshProUGUI TextThree;

        public TextMeshProUGUI CancelText;
        public TextMeshProUGUI DeleteText;

        private float height;

        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
           // Debug.Log("dod this");


        }


        //shariz new getting pop up info
        public void OpenInfoWindowContext()
        {
            windowMain.ModalWindowD.ShowInfo(false, false, "Experience Update Available!", "An updated version of this experience is available. Press update experience to delete the current experience and download an updated version. Your Progress will not be lost.", "Ok");

        }


        public void CancelAndDelete(UnityAction CancelEvent = null, UnityAction DeleteEvent = null)
        {
            windowMain.EnableBG();
            height = 0;
            Cancel.gameObject.SetActive(true);
            Delete.gameObject.SetActive(false);
            OptionOne.gameObject.SetActive(false);
            OptionTwo.gameObject.SetActive(false);
            OptionThree.gameObject.SetActive(false);

            Cancel.onClick.RemoveAllListeners();
            Cancel.onClick.AddListener(CancelEvent);
            Delete.onClick.RemoveAllListeners();
            height += 32;
            height += 62;

            if (CancelEvent != null)
            {
                Cancel.onClick.AddListener(CancelEvent);
                windowMain.Background.onClick.AddListener(CancelEvent);

            }

            if (DeleteEvent != null)
            {
                Delete.onClick.AddListener(DeleteEvent);
                Delete.gameObject.SetActive(true);
                height += 62 + 22;
            }
            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }
        //shariz added update bool
        public void CancelAndDeleteWithOptions(bool update, string textOne, string textTwo, string textThree, UnityAction CancelEvent = null, UnityAction DeleteEvent = null,
                                               UnityAction optionOne = null, UnityAction optionTwo = null, UnityAction optionThree = null)
        {

            windowMain.EnableBG();
            height = 0;
            Cancel.gameObject.SetActive(true);
            Delete.gameObject.SetActive(false);
            OptionOne.gameObject.SetActive(false);
            OptionTwo.gameObject.SetActive(false);
            OptionThree.gameObject.SetActive(false);

            Cancel.onClick.RemoveAllListeners();
            Delete.onClick.RemoveAllListeners();

            height += 0;
            height += 115;

            if (CancelEvent != null)
            {
                Cancel.onClick.AddListener(CancelEvent);
                windowMain.Background.onClick.AddListener(CancelEvent);
            }
            if (DeleteEvent != null)
            {
                Delete.onClick.AddListener(DeleteEvent);
                Delete.gameObject.SetActive(true);
                height += 57;
            }
            if (optionOne != null)
            {
                OptionOne.gameObject.SetActive(true);
                OptionOne.onClick.RemoveAllListeners();
                OptionOne.onClick.AddListener(optionOne);
                TextOne.text = textOne;
                height += 57;
            }
            if (optionTwo != null)
            {
                OptionTwo.gameObject.SetActive(true);
                OptionTwo.onClick.RemoveAllListeners();
                OptionTwo.onClick.AddListener(optionTwo);
                TextTwo.text = textTwo;
                height += 57;
                height += 5;

            }
            if (optionThree != null)
            {
                OptionThree.gameObject.SetActive(true);
                OptionThree.onClick.RemoveAllListeners();
                OptionThree.onClick.AddListener(optionThree);
                TextThree.text = textThree;
                height += 57;
                height += 5;
            }
            //shariz update
            if (update == true)
            {
                Notification.gameObject.SetActive(true);

            }
            if (update == false)
            {
                Notification.gameObject.SetActive(false);
            }

            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }


        void AddCloseWindowFunctionToAllButtons()
        {
            Cancel.onClick.AddListener(closeWindow);
            Delete.onClick.AddListener(closeWindow);
            OptionOne.onClick.AddListener(closeWindow);
            OptionTwo.onClick.AddListener(closeWindow);
            OptionThree.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
        }



        public void OpenWindow()
        {
            windowMain.EnableBG();
            Window.gameObject.SetActive(true);
            LeanTween.moveY(Window, height, 0.3f).setEase(LeanTweenType.easeInBack);
            Debug.Log(height);
        }
        public void closeWindow()
        {
            LeanTween.moveY(Window, 0, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            windowMain.CloseBG();
        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(CancelText, language.getAFont(8), language.getAButtonText(1));
            localizeSetting.setTextConfig(DeleteText, language.getAFont(8), language.getAButtonText(35));
            localizeSetting.setTextConfig(TextOne, language.getAFont(29));
            localizeSetting.setTextConfig(TextTwo, language.getAFont(29));
            localizeSetting.setTextConfig(TextThree, language.getAFont(29));
        }
    }
}
