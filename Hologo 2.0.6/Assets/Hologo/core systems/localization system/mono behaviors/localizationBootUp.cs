using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// localization bootup script
    /// </summary>
    public class localizationBootUp : MonoBehaviour, IBootUp
    {
        [SerializeField]
        private settings_SO settings;


        [SerializeField]
        private localizationManager lManager;
        [SerializeField]
        private prefabsLocalizationManager pLManager;

        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;
        }

        public void runBootSequence()
        {
           // bootLocalization();
        }

        public async Task runBootUp()
        {
            Debug.Log("running localization initialization");
            bootUpSuccess = await getUpdatedLocalizationListOrLoad(settings.languageUpdate);
            Debug.Log("language list is loaded");
            bootUpSuccess = await updateLanguageAssetBundleOrLoad(settings.languageUpdate);
            Debug.Log("language file is loaded");
        }

        // here all prefabs are localized to selected language
        public IEnumerator localizeAllPrefabs()
        {
            yield return StartCoroutine(pLManager.localizeUIPrefabs());
        }

        async Task<bool> getUpdatedLocalizationListOrLoad(bool update)
        {
            //Debug.Log("update or load lan list");
            //if (update)
            //{
            //    Debug.Log("Delete list + update");
            //    lManager.deleteLanguageList();
            //}
            Debug.Log("get list");
            return await lManager.loadLocalizationListFromServerOrStorage(update);
        }

        async Task<bool> updateLanguageAssetBundleOrLoad(bool update)
        {
            //Debug.Log("update or load lan");
            //if (update)
            //{
            //    Debug.Log("Delete lan asset + update");
            //    lManager.deleteLanguageAsset();
            //}

            Debug.Log("get lan");
            return await lManager.loadLanguageFromServerOrStorage(settings.getCurrentLanguageName(), update);
        }


    }
}
