using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hologo2;

namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// iOs modal window A.
    /// </summary>
    public class iOS_InforToast : MonoBehaviour, IPrefabLocalize
    {

        public TextMeshProUGUI Information;
        public RectTransform Toast;
        public CanvasGroup ToastAlpha;

        public float DisplayTime = 2f;
        public float MoveTime = 0.6f;

        public void ToastTest()
        {
            ShowToast("this is a toast", 4f);
        }

        public void ToastTest2()
        {
            ShowToast("this is a toast 2", 4f);
        }

        //after 30 chars put in a new line
        public void ShowToast(string information, float displayTime)
        {
          
         // LeanTween.cancel(this.gameObject);

            Information.text = information;
            if (displayTime <= 0)
                displayTime = DisplayTime;


            var seq = LeanTween.sequence();
            seq.append(LeanTween.moveY(Toast, 70f, MoveTime));
            seq.append(displayTime);  
            seq.append(LeanTween.moveY(Toast, -1f, MoveTime));
          
        }


        //shariz v2 20/9/2019
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(Information, language.getAFont(17));
        }

    }
}
