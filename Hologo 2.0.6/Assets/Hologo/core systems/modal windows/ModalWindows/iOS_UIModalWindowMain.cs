using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Hologo2;

namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// iOs modal window controller.
    /// </summary>

    public class iOS_UIModalWindowMain : iUILayoutBase,IPrefabLocalize
    {

        #region Public Variables
        public Button Background;
        public Button BackgroundWhite; //Shariz making the bg for exp popup white
        public Button ClearBackground;
        public GameObject LoadingBackground;
        public IOS_ModalWindowA ModelWindowA;
        public iOS_ModalWindowB ModelWindowB;
        public iOS_ModalWindowC ModalWindowC;
        public iOS_ModalWindowD ModalWindowD;
        public expWindowSwitcher ModalWindowE;
        public expWindowSwitcher ModalWindowF;
        public iOS_ModalWindowExp ModalWindowExp;
        public iOS_InforToast InfomationToast;
        public iOS_ModalWindowG  ModelWindowG;
        public iOS_ContextMenu ContextMenu;
        public expWindowSwitcher experienceDetailWindow;

        #endregion


        #region Private Variables
        static iOS_UIModalWindowMain iOSmodalPanel;
        #endregion

        #region Modal Window Methods
        public static iOS_UIModalWindowMain Instance()
        {
            if (!iOSmodalPanel)
            {
                iOSmodalPanel = FindObjectOfType(typeof(iOS_UIModalWindowMain)) as iOS_UIModalWindowMain;
                if (!iOSmodalPanel)
                {
                    //Debug.LogError("There needs to be one active ModalWindowScript on a GameObject in your scene.");
                }
            }
            return iOSmodalPanel;
        }



        public void CloseBG()
        {
            Background.onClick.RemoveAllListeners();
            Background.gameObject.SetActive(false);
        }


        public void EnableBG()
        {
            Background.gameObject.SetActive(true);
        }

        //Shariz white bg
        public void CloseWBG()
        {
            BackgroundWhite.onClick.RemoveAllListeners();
            BackgroundWhite.gameObject.SetActive(false);
        }

        //Shariz white bg
        public void EnableWBG()
        {
            BackgroundWhite.gameObject.SetActive(true);
        }

        public void LoadingWindowStatus(bool activiate)
        {
            LoadingBackground.SetActive(activiate);
        }

        public void EnableClearBG()
        {
            ClearBackground.gameObject.SetActive(true);
        }

        public void CloseClearBG()
        {
            ClearBackground.onClick.RemoveAllListeners();
            ClearBackground.gameObject.SetActive(false);
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            experienceDetailWindow.getExpDetailWindow().localizePrefab(language, localizeSetting);
            InfomationToast.localizePrefab(language, localizeSetting);
            ModalWindowF.getiOS_ModalWindowF().localizePrefab(language, localizeSetting);
            ModalWindowE.getiOS_ModalWindowE().localizePrefab(language, localizeSetting);
            ModelWindowA.localizePrefab(language, localizeSetting);
            ModelWindowB.localizePrefab(language, localizeSetting);
            ModalWindowD.localizePrefab(language, localizeSetting);
        }


        #endregion
    }
}
