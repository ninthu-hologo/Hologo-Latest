using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Hologo2.library
{
    public class ui_test_pre : MonoBehaviour, IPrefabLocalize
    {

        public TextMeshProUGUI heading;
        public TextMeshProUGUI subheading;

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(heading,language.getAFont(0), language.getAButtonText(0));
            localizeSetting.setTextConfig(subheading, language.getAFont(1), language.getAButtonText(1));
        }
    }
}
