using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

namespace Hologo2
{

     /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: Shariz 1st september 2019 for localizing stuff Hologo v2
    /// experience ui element 
    /// </summary>

    public class subcategoryUIElement : MonoBehaviour, IPrefabLocalize
    {
        [SerializeField]
        private Transform contentParent;
        [SerializeField]
        private TextMeshProUGUI subheading;
        [SerializeField]
        private MyScrollConflictManager scrollConflictManager;
        [SerializeField]
        private int id;



        public void fillData(int myId, string text,MyScrollRect scrollRect)
        {
            subheading.text = text;
            id = myId;
            scrollConflictManager.ParentScrollRect = scrollRect;
        }

        public Transform getContentParent()
        {
            return contentParent;
        }

        //shariz v2 1/9/2019
        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(subheading, language.getAFont(20));
        }


    }
}
