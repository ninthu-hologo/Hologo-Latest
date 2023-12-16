using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 03 August 2019
    /// Modified by: 
    /// class ui item to populate a description of a experience
    /// </summary>
    public class descriptionElement : MonoBehaviour, IPrefabLocalize
    {
        public RectTransform descriptionCanvas;
        public TextMeshProUGUI description;
        public Button button;
        public bool isShown;

        public void fillWithData(string text, Vector2 size, bool enabled)
        {

            descriptionCanvas.sizeDelta = size;
            description.text = text;
            isShown = enabled;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(description, language.getAFont(4));
        }

    }
}
