using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2.library
{
    public class language_tests : MonoBehaviour
    {

        public localizationManager lm;
        bool displayProgress = false;

        // Start is called before the first frame update
        void Start()
        {
            loadAssetBundle();
        }


        public async void loadAssetBundle()
        {
            //  AssetBundle bundle = await lm.loadAssetBundle("dhivehilanguage.hdlc","lan");
            // languageData_SO language = lm.loadlanguageFromAsset("dhivehi language", bundle);
            // Debug.Log(language.language);
            // lm.setCurrentLanguage(language);

            //await lm.loadfromServerOrStorageLanguage("dhivehilanguage.hdlc", "dhivehi language");
            //await downloadHelper.downloadToBufferAndSave("http://hamid.faahaga.com/", "test1.txt","lan");
            //await downloadHelper.downloadToStorage("http://hamid.faahaga.com/", "tube_d_inner_abs.gcode", "lan");
            displayProgress = true;
            await lm.loadLanguageFromServerOrStorage("dhivehilanguage.hdlc", "dhivehi language");
            //lm.setFallBackLanguage();
            displayProgress = false;
        }


        private void Update()
        {
            if (displayProgress)
                Debug.Log(downloadHelper.progress * 100);
        }
    }
}
