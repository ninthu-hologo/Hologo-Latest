using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: 
    /// experience ui element 
    /// </summary>
    public class categoryUIButton : MonoBehaviour, IUIElement, IPrefabLocalize
    {
        [Header("UI ELEMENTS")]
        [SerializeField]
        private Button button;
        [SerializeField]
        private TextMeshProUGUI titleText;
        

        public int catID; // Shariz added id to gameobject

        [SerializeField]
        private Image activeBG; //Shariz added activeBG

        private categoryClass category;


        public void fillWithDataSync(IdataObject data, params CallbackGameObject[] actions)
        {
            category = data as categoryClass;
            titleText.text = category.title;
            catID = category.id; // Shariz added id to gameobject
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => actions[0](this.gameObject));
        }


        //    public async Task fillWithData(IdataObject data, params CallbackGameObject[] actions)
        //{
           
        //}

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(titleText, language.getAFont(3));
        }

        public IdataObject getData()
        {
            return category;
        }

        // shariz setting category activex
        public void deActiveCategories(bool activeness){
            activeBG.gameObject.SetActive(activeness);
        }

         
    }
}
