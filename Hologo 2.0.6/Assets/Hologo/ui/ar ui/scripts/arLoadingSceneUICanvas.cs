using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// class that localizes the ui of experience slides and handles the functions
    /// </summary>
    public class arLoadingSceneUICanvas : iUILayoutBase, IPrefabLocalize
    {

        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public Image loadingImage;
        private float totalProgress = 0f;

        public void Start()
        {
            totalProgress = 0f;

        }

        private void Update()
        {
            if (!downloadHelper.isDone)
            {
                
               totalProgress = downloadHelper.previouseProgress + downloadHelper.progress;
               
                Debug.Log("total progress>" + totalProgress);
                
                setSlideValue(totalProgress/2);
            }
            else
            {
                setSlideValue(1f);
            }
        }

        public void disableLoadingARCanvas()
        {
            
            gameObject.SetActive(false);
            downloadHelper.resetProgress();
        }

        //shariz v2 1/9/2019
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(title, language.getAFont(41), language.getAMessageText(56));
            localizeSetting.setTextConfig(description, language.getAFont(4), language.getALabelText(88));//SHARIZ 22nd Feb 2020 2.0.4      
            // description.text = "Give us a few seconds and we’ll have your experience up and running.";//SHARIZ 29th Oct 2019 2.00

        }

        public void setSlideValue(float value)
        {
            loadingImage.fillAmount = value;
        }

    }
}