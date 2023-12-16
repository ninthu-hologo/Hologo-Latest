using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


namespace Hologo.iOSUI
{
    public class iOS_ModalWindowExp : MonoBehaviour
    {


        public RectTransform Window;
        public Button Download;
        public Button Close;
        public TextMeshProUGUI ButtonText;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Size;
        public Image Picture;
       

        private CanvasGroup windowCanvas;

        // private float height;

        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            windowCanvas = Window.GetComponent<CanvasGroup>();

        }


        public void StoreItemShow(Sprite image,string title, string description, string size, UnityAction download =null)
        {
            windowMain.EnableWBG(); //Make bg white Shariz
            InfoDisplay(title, description,size);
            Picture.sprite = image;

            Close.onClick.RemoveAllListeners();
            Download.onClick.RemoveAllListeners();


           

            if(download !=null){
                Download.onClick.AddListener(download);
                ButtonText.text = "Download";
            }else
            {
                ButtonText.text = "close";
            }

            Close.onClick.AddListener(closeWindow);
            Download.onClick.AddListener(closeWindow);
            windowMain.BackgroundWhite.onClick.AddListener(closeWindow);
            OpenWindow();
        }


        void InfoDisplay(string title, string description,string size)
        {
            Title.text = title;
            Description.text = description;
            Size.text = "Size of the experience: "+size+"MB";
        }


        public void OpenWindow()
        {
            Window.gameObject.SetActive(true);
            windowMain.EnableWBG();
            LeanTween.scale(Window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }
        public void closeWindow()
        {
            // LeanTween.moveY(Window, 0, 0.3f).setEase(LeanTweenType.easeOutBack).setOnComplete(CleanWindow);
            LeanTween.scale(Window, new Vector3(0.6f, 0.6f, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(CleanWindow); ;

            windowMain.CloseWBG();

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);

        }
    }
}
