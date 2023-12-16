using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


namespace Hologo2.library
{

    public class apply_localization : MonoBehaviour
    {

        public languageData_SO myLanguage;

        public localizePrefab_SO mylocal;

        public Transform parent;

        public TMP_InputField inputFieldUser;
        public TextMeshProUGUI placeHolderText;
        public TextMeshProUGUI inputText;

        public localizationManager lm;


        // Start is called before the first frame update
        void Start()
        {
            if (myLanguage == null || mylocal == null)
                return;

            if (!mylocal.checkLanguageExits())
            {
                Debug.Log("landguage was done");
                mylocal.setLanguage(myLanguage);
                mylocal.localizePrefab();
                
            }
           
            for (int i = 0; i < 5; i++)
            {
                GameObject go = Instantiate(mylocal.givePrefab());
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.SetParent(parent);
                
            }

            placeHolderText.text = myLanguage.getAButtonText(0);
            placeHolderText.font = myLanguage.getAFont(0).font;
            placeHolderText.alignment = myLanguage.getAFont(0).textAlignment;
            placeHolderText.isRightToLeftText = myLanguage.rightToLeft;
            inputText.font = myLanguage.getAFont(0).font;
            inputText.isRightToLeftText = myLanguage.rightToLeft;
            inputText.alignment = myLanguage.getAFont(0).textAlignment;
        }


        public void gotToNext(string scene)
        {
            SceneManager.LoadScene(scene);
        }


        public void DownloadAndLoadLanguage()
        {

        }

    }
}
