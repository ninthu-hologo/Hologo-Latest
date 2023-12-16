using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 10-07-2018
    /// Modified by:
    /// iOs context menu.
    /// </summary>
    public class iOS_ContextMenu : MonoBehaviour
    {
        public RectTransform Window;
        public RectTransform ContextMenu;
        public Button Delete;
        public Button OptionOne;
        public Button OptionTwo;
        public Button OptionThree;
        public GameObject Notification;
        public TextMeshProUGUI TextOne;
        public TextMeshProUGUI TextTwo;
        public TextMeshProUGUI TextThree;
        public float ScreenBottomPadding;
        public float ScreenRightPadding;
        public float XOffset;
        public float YOffset;


        private iOS_UIModalWindowMain windowMain;

        void Start()
        {
            windowMain = iOS_UIModalWindowMain.Instance();
            // Debug.Log("dod this");

        }



        public void DeleteWithOptions(bool update, Vector2 Position, string textOne, string textTwo, string textThree, UnityAction DeleteEvent = null,
                                               UnityAction optionOne = null, UnityAction optionTwo = null, UnityAction optionThree = null)
        {


            Delete.gameObject.SetActive(false);
            OptionOne.gameObject.SetActive(false);
            OptionTwo.gameObject.SetActive(false);
            OptionThree.gameObject.SetActive(false);


            Delete.onClick.RemoveAllListeners();
            OptionOne.onClick.RemoveAllListeners();
            OptionTwo.onClick.RemoveAllListeners();
            OptionThree.onClick.RemoveAllListeners();

            if (DeleteEvent != null)
            {
                Delete.onClick.AddListener(DeleteEvent);
                Delete.gameObject.SetActive(true);
            }

            if (optionOne != null)
            {
                OptionOne.gameObject.SetActive(true);
                OptionOne.onClick.AddListener(optionOne);
                TextOne.text = textOne;
            }
            if (optionTwo != null)
            {
                OptionTwo.gameObject.SetActive(true);
                OptionTwo.onClick.AddListener(optionTwo);
                TextTwo.text = textTwo;
            }
            if (optionThree != null)
            {
                
                OptionThree.gameObject.SetActive(true);
                OptionThree.onClick.AddListener(optionThree);
                TextThree.text = textThree;
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
            DetermineScreenPosition(Position);
            OpenWindow();
        }



        void DetermineScreenPosition(Vector2 Position)
        {
            float x;
            float y;
            if (Position.x - Window.sizeDelta.x <= 0)
            {
                //imageRect.position = new Vector2(buttonRect.position.x + imageRect.sizeDelta.x + 10f, buttonRect.position.y - 15f);
                x = Position.x + Window.sizeDelta.x + ScreenRightPadding + XOffset;
                Debug.Log("x is less");
            }else
            {
                x = Position.x + XOffset;
            }

            if(Position.y - Window.sizeDelta.y <=0)
            {
                y = Position.y + ContextMenu.sizeDelta.y+ ScreenBottomPadding + YOffset;

            }else
            {
                y = Position.y + YOffset;
            }

            Window.position = new Vector2(x, y);

        }



        void AddCloseWindowFunctionToAllButtons()
        {
            Delete.onClick.AddListener(closeWindow);
            OptionOne.onClick.AddListener(closeWindow);
            OptionTwo.onClick.AddListener(closeWindow);
            OptionThree.onClick.AddListener(closeWindow);
            windowMain.ClearBackground.onClick.AddListener(closeWindow);
        }

        //shariz new getting pop up info
        public void OpenInfoWindowContext(){
            windowMain.ModalWindowD.ShowInfo(false, false, "Experience Update Available!", "An updated version of this experience is available. Press update experience to delete the current experience and download an updated version. Your Progress will not be lost.", "Ok");

        }

        public void OpenWindow()
        {
            windowMain.EnableClearBG();
            Window.gameObject.SetActive(true);

        }
        public void closeWindow()
        {
            
            windowMain.CloseClearBG();
            CleanWindow();

        }

        public void CleanWindow()
        {
            Window.gameObject.SetActive(false);
        }


    }
}
