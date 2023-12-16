using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hologo.iOSUI;
using UnityEngine.Events;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 31 August 2019
    /// Modified by:  Shariz 1st september 2019 for localizing stuff Hologo v2
    /// settings view controller
    /// </summary>

    public class experienceDetailsUIWindow : MonoBehaviour, IPrefabLocalize
    {

        public Image displayPicture;
        public TextMeshProUGUI category;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        
        //shariz v2 1/9/2019
        public TextMeshProUGUI viewDescription;
        //shariz v2 1/9/2019
        public CanvasGroup windowCanvas;


        public RectTransform Window;
        public Button close;
        public Button viewExperience;

        private iOS_UIModalWindowMain windowMain;

        public GameObject blurOverlay; //Shariz 15th September 2019 v2

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
        }


        public void FillInput(bool canView,Sprite image,string category, string title, string description, UnityAction open = null)
        {
            displayPicture.sprite = image;
            this.category.text = category;
            this.title.text = title;
            this.description.text = description;
            viewExperience.onClick.RemoveAllListeners();
            if(!canView)
            {
                viewExperience.gameObject.SetActive(false);
            }
            else
            {
                if (open != null)
                {
                    viewExperience.onClick.AddListener(open);
                }
            }
            
            AddCloseWindowFunctionToAllButtons();
            OpenWindow();
        }

        void AddCloseWindowFunctionToAllButtons()
        {
            viewExperience.onClick.AddListener(closeWindow);
            close.onClick.AddListener(closeWindow);
            windowMain.Background.onClick.AddListener(closeWindow);
        }

        public void OpenWindow()
        {
            blurOverlay.SetActive(true); //Shariz 15th September 2019 v2
            Window.gameObject.SetActive(true);
           // windowMain.EnableBG();
    
            //shariz v2 1/9/2019
            LeanTween.scale(Window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }

        public void closeWindow()
        {
             //shariz v2 1/9/2019
            LeanTween.scale(Window, new Vector3(0.6f, 0.6f, 1), 0.3f).setEase(LeanTweenType.easeOutBack);//Shariz 27th Oct 2019 v2
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            
            // Window.gameObject.SetActive(false);
            blurOverlay.SetActive(false); //Shariz 15th September 2019 v2
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        //shariz v2 1/9/2019
        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);
            

        }

        //shariz v2 1/9/2019
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(title, language.getAFont(1));
            localizeSetting.setTextConfig(category, language.getAFont(2));
            localizeSetting.setTextConfig(description, language.getAFont(14));
            localizeSetting.setTextConfig(viewDescription, language.getAFont(8), language.getAButtonText(42));
        }

    }
}