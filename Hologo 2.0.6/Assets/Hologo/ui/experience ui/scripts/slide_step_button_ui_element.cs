using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 19 july 2019
    /// Modified by: 
    /// slide step button ui element script
    /// </summary>
    public class slide_step_button_ui_element : MonoBehaviour, IPrefabLocalize
    {
        public Button stepButton;
        public TextMeshProUGUI stepButtonText;
        public int id;

        public void fillUpButton(int myId, string text, CallbackForId callbackId = null)
        {
            id = myId;
            stepButtonText.text = text;

            stepButton.onClick.RemoveAllListeners();
            if (callbackId != null)
                stepButton.onClick.AddListener(()=> callbackId(id));
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(stepButtonText, localizeSetting.getLanguage().getAFont(6));
        }


        // Shariz 1.1.5 changed active step
        public void amIActive(int outId)
        {
            Color myColor = stepButton.image.color;
            if (outId == id)
            {
                
                //StepButton.image.sprite = ActiveSprite;
                //StepButton.image.color = new Color(1f, 0.44f, 0f, 1);
                stepButtonText.color = new Color(0, 0, 0, 1f);
                
            }
            else
            {
                //StepButton.image.sprite = DeactiveSprite;
                //StepButton.image.color = new Color(0.6f, 0.6f, 0.6f, 0.41f);
                stepButtonText.color = new Color(0, 0, 0, 0.5f);
            }
        }

    }
}
