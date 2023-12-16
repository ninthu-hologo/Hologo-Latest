using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hologo2
{

    public class lessonDisplayUICanvas : iUILayoutBase, IPrefabLocalize
    {
        //shariz v2 1/9/2019
        public TextMeshProUGUI title;
    
    
         //shariz v2 1/9/2019
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(title, language.getAFont(16), language.getAButtonText(12));
        }
    }
}
