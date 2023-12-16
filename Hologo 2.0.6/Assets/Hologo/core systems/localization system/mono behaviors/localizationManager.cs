using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 June 2019
    /// Modified by: 
    /// localization initiation . load asset bundle with and language asset
    /// this script will be applied to a gameobject
    /// </summary>
    public class localizationManager : messageLogging
    {
        [Header("DATA")]
        [SerializeField]
        private settings_SO mainSettings;
        //localization list
        [SerializeField]
        localizationList_SO localizationList;
        //current language
        [SerializeField]
        languageData_SO currentLanguage;
        //fall back language incase language loading fails.
        [SerializeField]
        languageData_SO defaultLanguage;

        public dataPaths_SO languageDataPath;
        public dataPaths_SO localizationDataPath;

        private AssetBundle languageBundle;

        #region LOCALIZATION LIST LOADING

        /* 1. first check to see if localization list exits in the storage.
              2. if so load from storage - return true
              3. if not then try to connect to the server
              4. if cannot app cannot run and inform user to connect to the internet and launch app again - return false
              5. if can connect to server download localization list and save - return true */
        public async Task<bool> loadLocalizationListFromServerOrStorage(bool update)
        {
            bool success = false;
            bool readFromStorage = false;

            if (!update)
            {

                if (localizationList.isLocalizationListLoaded())
                    return true;

                if (checkIfFileExists(localizationDataPath.getFolder(0), localizationDataPath.getFileName(0)))
                {
                    string locaListDataString = readLocalizationListFromStorage();
                    if (string.IsNullOrEmpty(locaListDataString))
                    {
                        // failed loadind data;
                        // try to get library from internet
                        success = await loadLocalizationListFromServer();
                    }
                    else
                    {
                        //success
                        //try convert data read from file to object data
                        success = readFromStorage = convertLocalizationListData(locaListDataString);
                        //failed try to get library from internet
                        if (!success)
                        {
                            success = await loadLocalizationListFromServer();
                        }

                    }
                }
                else
                {
                    //failed try to get library from internet
                    success = await loadLocalizationListFromServer();
                }

                if (success && !readFromStorage)
                {
                    SaveLocalizationListToStorage();
                }
            }
            else
            {
                Debug.Log("Updating language list! ");
                success = await loadLocalizationListFromServer();
                if (success)
                {
                    SaveLocalizationListToStorage();
                }
            }

            return success;
        }


        //public async Task<bool> updateLocalizationList()
        //{
        //    bool success = false;
        //    success = await loadLocalizationListFromServer();
        //    success = SaveLocalizationListToStorage();
        //    return success;
        //}

        public void deleteLanguageList()
        {
            Debug.Log("deleting list");
            if (localizationList != null)
                localizationList.flushData();
            readWriteData.DeleteFileOnDisk(localizationDataPath.getFolder(0), localizationDataPath.getFileName(0));
        }


        string readLocalizationListFromStorage()
        {
            localizationServerConnect lsc = new localizationServerConnect();
            return lsc.loadLocalizationListFromLocalStorage(localizationDataPath.getFolder(0), localizationDataPath.getFileName(0));
        }

        //load library from server
        private async Task<bool> loadLocalizationListFromServer()
        {
            bool success = false;
            string result;
            localizationServerConnect lsc = new localizationServerConnect();
            var task = lsc.getCountryLanguageListFromServer(localizationDataPath.getUrl(0));

            try
            {
                result = await task;

                GenericObjectAndStatus<HologoWebAPIGeneric<List<localizationListDataClass>>> localizationAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<localizationListDataClass>>>(result);
                if (localizationAndStatus.Success)
                {
                    localizationList.fillData(localizationAndStatus.GenericObject.data);
                    success = true;
                }
                else
                {
                    createMessage("error");
                }

            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }

            return success;
        }

        //convert stored data to data object
        bool convertLocalizationListData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, localizationList))
            {
                success = true;
            }
            return success;
        }

        //save library data to storage
        public bool SaveLocalizationListToStorage()
        {
            localizationServerConnect lsc = new localizationServerConnect();
            return lsc.writeLocalizationListToStorage(localizationList, false, localizationDataPath.getFolder(0), localizationDataPath.getFileName(0));
        }
        #endregion

        #region LANGUAGE ASSET LOADING
        // first load the asset bundle that contains the language
        // if we cant, need to download it again
        // if we cannot we apply the default language
        // and try again next time we open that app
        // then try loading the language asset inside it
        // and then finaly apply it to the current langauge
        public async Task<bool> loadLanguageFromServerOrStorage(string languageName, bool update)
        {
            string languageFileName = "";
            if (mainSettings.osplatform == OSPlatform.IOS)
            {
                languageFileName = languageDataPath.getFileName(0);
            }
            else
            {
                languageFileName = languageDataPath.getFileName(1);
            }

            if(update)
            {
                return await updateLangauge(languageFileName, languageName);
               
            }
            return await loadlanguageCommon(languageFileName, languageName);
        }

        public async Task<bool> loadLanguageFromServerOrStorage(string languageFileName, string languageName)
        {

            return await loadlanguageCommon(languageFileName, languageName);
        }


        async Task<bool> updateLangauge(string languageFileName, string languageName)
        {
            Debug.Log("Updating language! ");
            bool success = false;
            string folderName = languageDataPath.getFolder(0);
            string url = languageDataPath.getUrl(0) + mainSettings.getPlatform();
            
            languageDataConnect lanDC = new languageDataConnect();
           

            try
            {
                success = await lanDC.downloadLanguage(url, folderName, languageFileName);
                Debug.Log("language updated");
            }
            catch (System.Exception)
            {

                createMessage(downloadHelper.internetError);
                Debug.Log("error");
            }

            success = await loadAssetAndLanguage(languageName, folderName, languageFileName);

            // assigning fallback language
            if (!success)
                setFallBackLanguage();


            return success;

        }

        async Task<bool> loadlanguageCommon(string languageFileName, string languageName)
        {
            Debug.Log("startLanguage .. " + languageFileName);
            bool success = false;

            // loading language from main settings. for change of language
            if (mainSettings.LanguageBundle != null)
            {
                Debug.Log("language already loaded");
                languageBundle = mainSettings.LanguageBundle;
                loadLanguageFromMemory(mainSettings.language.name);
                return true;
            }

            Debug.Log("language going on!");
            string folderName = languageDataPath.getFolder(0);

            string url = languageDataPath.getUrl(0) + mainSettings.getPlatform();
            Debug.Log("language going on! " + url);
            languageDataConnect lanDC = new languageDataConnect();

            if (isCurrentLanuageLoaded())
            {
                Debug.Log("language going loaded why???");
                return true;
            }

            // check to see if file exists in storage
            if (checkIfFileExists(folderName, languageFileName))
            {
                // load file
                success = await loadAssetAndLanguage(languageName, folderName, languageFileName);

                Debug.Log("langauge exists");
            }
            else
            {
                // get the file from server
                Debug.Log("does not exist");
                try
                {
                    success = await lanDC.downloadLanguage(url, folderName, languageFileName);
                    Debug.Log("downloaded");
                }
                catch (System.Exception)
                {

                    createMessage(downloadHelper.internetError);
                    Debug.Log("error");
                }

                success = await loadAssetAndLanguage(languageName, folderName, languageFileName);
            }

            // assigning fallback language
            if (!success)
                setFallBackLanguage();


            return success;
        }

        private void loadLanguageFromMemory(string languageName)
        {
            languageDataConnect lanDC = new languageDataConnect();
            setCurrentLanguage(lanDC.loadlanguageFromAsset(languageName, mainSettings.LanguageBundle));
        }

        /// <summary>
        /// check to see if the language current has a later version if so update
        /// </summary>
        /// <param name="url"></param>
        /// <param name="languageFileName"></param>
        /// <returns></returns>
        public async Task<bool> updateLanguage(string url, string languageFileName)
        {
            await new WaitForSeconds(0.1f);
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageFileName"></param>
        /// <param name="languageName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private async Task<bool> loadAssetAndLanguage(string languageName, string folderName, string languageFileName)
        {
            bool success = true;
            languageDataConnect lanDC = new languageDataConnect();
            // load asset bundle from storage
            languageBundle = await lanDC.loadLanguageFromStorage(folderName, languageFileName);
            // saving bundle in main setting for easy access
            mainSettings.setLanguageBundle(languageBundle);
            if (languageBundle == null)
            {
                createMessage("could not load assetbundle");
                return false;
            }
            // assigning the loaded language to current langauge
            setCurrentLanguage(lanDC.loadlanguageFromAsset(languageName, languageBundle));
            if (currentLanguage == null)
            {
                createMessage("could not load asset from assetbundle");
                return false;
            }

            return success;
        }


        public void setCurrentLanguageAfterLanguageSelection()
        {
            languageDataConnect lanDC = new languageDataConnect();
            setCurrentLanguage(lanDC.loadlanguageFromAsset(mainSettings.getCurrentLanguageName(), mainSettings.LanguageBundle));
        }

        // check to see if current library is already loaded
        bool isCurrentLanuageLoaded()
        {
            return currentLanguage.isLanguageFilled();
        }

        // if a language fails to load we switch to the default fallback language
        public void setFallBackLanguage()
        {
            currentLanguage.setlanguageData(defaultLanguage);
        }

        // set current language
        public void setCurrentLanguage(languageData_SO language)
        {
            currentLanguage.setlanguageData(language);
        }

        // method that will let access to current language
        public languageData_SO getCurrentLanguage()
        {
            return currentLanguage;
        }

        public void deleteLanguageAsset()
        {
            if (currentLanguage != null)
                currentLanguage.flushData();

            readWriteData.DeleteFileOnDisk(languageDataPath.getFolder(0), languageDataPath.getFileName(0));
            readWriteData.DeleteFileOnDisk(languageDataPath.getFolder(0), languageDataPath.getFileName(1));
        }

        #endregion

        #region HELPERS
        bool checkIfFileExists(string folderName, string fileName)
        {
            return readWriteData.CheckIfFileExists(folderName, fileName);
        }
        #endregion

    }
}
