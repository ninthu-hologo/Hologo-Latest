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
    /// iOs modal window B.recorded lessons enter details window
    /// </summary>
    public class iOS_ModalWindowF : MonoBehaviour, IPrefabLocalize
    {

        public RectTransform Window;
        public Button Cancel;
        public Button Save;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TMP_InputField TitleInput;
        public TMP_InputField DescriptionInput;
       // public TextMeshProUGUI PlaceHolderTextTitle;
      //  public TextMeshProUGUI PlaceHolderTextDes;


        public TextMeshProUGUI SaveText;
        public TextMeshProUGUI CancelText;

        private CanvasGroup windowCanvas;


        // private float height;

        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();
        }

        // WARNING. cancel and save button is switched
        public void FillInput(bool close, string title, string description,
                              UnityAction<string,string> save, UnityAction cancel = null)
        {
            TitleInput.text = "";
            DescriptionInput.text = "";
            InfoDisplay(title, description);

            Cancel.onClick.RemoveAllListeners();
            Save.onClick.RemoveAllListeners();

            if (cancel != null) // shariz
            {
                Save.onClick.AddListener(cancel);
            }
            else {
                Save.onClick.AddListener(closeWindow);
            }
            

            Cancel.onClick.AddListener(() => save( TitleInput.text, DescriptionInput.text));

            if (close)
            {

                Cancel.onClick.AddListener(closeWindow);
            }

            AddCloseWindowFunctionToAllButtons();
            OpenWindow(); 
        }

        void AddCloseWindowFunctionToAllButtons()
        {
            Cancel.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
        }



        void InfoDisplay(string title, string description)
        {
            Title.text = title;
            Description.text = description;
           // PlaceHolderTextTitle.text = placeholder;
            //PlaceHolderTextDes.text = placeholder2;
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
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }


         public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(SaveText, language.getAFont(8), language.getAButtonText(32));
            localizeSetting.setTextConfig(CancelText, language.getAFont(19), language.getAButtonText(1));
        }

    }
}
