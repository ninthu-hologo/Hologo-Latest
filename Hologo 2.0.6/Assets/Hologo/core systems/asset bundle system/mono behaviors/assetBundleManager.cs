using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hologo2.library;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 September 2019
    /// Modified by: 
    /// manages all the data of downloaded asset bundles
    /// </summary>
    public class assetBundleManager : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private assetBundelData_SO assetBundles;
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private dataPaths_SO dataPath;

        AssetBundle currentExpAssetBundle;
        AssetBundle currentLocExpAssetBundle;
        public float progress = 0f;

        // load user from storage or make a new user if cant
        public bool loadAssetBundleDataFromStorage()
        {
            bool success = false;
            string result;
            if (assetBundles.isDataFilled())
                return true;

            if (checkIfAssetBundleDataExists())
            {
                result = readAssetBundleDataFromStorage();
                if (!string.IsNullOrEmpty(result))
                {
                    convertToAssetBundleData(result);

                    success = true;
                }
            }

            return success;
        }

        // read asset bundles data from storage
        string readAssetBundleDataFromStorage()
        {
            assetBundleServerConnect absc = new assetBundleServerConnect();
            return absc.loadAssetBundleDataFromLocalStorage(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // event raised from localization view controller in init scene .after changing language
        public void clearAssetBundleData(GameObject go)
        {
            assetBundles.ClearList();
        }

        //convert stored data to data object
        bool convertToAssetBundleData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, assetBundles))
            {
                success = true;
            }
            return success;
        }

        // this will clear all the asset bundles and data
        public void ClearCache()
        {
            // delete all asset bundles in assetbundle folder
            string[] files = Directory.GetFiles(readWriteData.GetPath(dataPath.getFolder(0)));
            if (files.Length == 0)
                return;

            for (int i = 0; i < files.Length; i++)
            {
                Debug.Log(files[i] + " is deleted!");
                File.Delete(files[i]);
            }
            assetBundles.ClearList();
            saveAssetBundelDataToStorage();
        }


        // check if asset bundles data file exists
        // helper functions
        bool checkIfAssetBundleDataExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //save asset data to storage
        public bool saveAssetBundelDataToStorage()
        {
            serverUserConnect userConnect = new serverUserConnect();
            return userConnect.writeUserDataToStorage(assetBundles, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }



        #region ASSETBUNDLE LOADING METHODS



        public async Task<AssetBundle> getMainExperience(attachablesClass experience, string url)
        {

            currentExpAssetBundle = await loadExperience(experience);

            if (currentExpAssetBundle != null)
            {
                return currentExpAssetBundle;
            }
           
                Debug.Log("loading experience from server");
                bool success = await downloadHelper.downloadToStorage(url, dataPath.getFolder(0), experience.file_name, false);
                if (success)
                {
                    currentExpAssetBundle = await loadAssetBundle(experience);
                    assetBundles.addAssetBundleToList(experience);
                    saveAssetBundelDataToStorage();
                }
                else
                {
                    success = false;
                }
            
            return currentExpAssetBundle;
        }

        public async Task<AssetBundle> getLocalizedExperience(attachablesClass experience, string url)
        {

            currentLocExpAssetBundle = await loadExperience(experience);

            if (currentLocExpAssetBundle != null)
            {
                return currentLocExpAssetBundle;
            }
            Debug.Log("loading localized file from server");
            bool success = await downloadHelper.downloadToStorage(url, dataPath.getFolder(0), experience.file_name, false);
            if (success)
            {
                currentLocExpAssetBundle = await loadAssetBundle(experience);
                assetBundles.addAssetBundleToList(experience);
                saveAssetBundelDataToStorage();
            }
            else
            {
                success = false;
            }
            return currentLocExpAssetBundle;
        }

        async Task<AssetBundle> loadExperience(attachablesClass experience)
        {
            Debug.Log("check");
            // here we check if the version of current experience is equal to the downloaded version
            // if not we download again else we load the exisitng one. also if no internet is available we load the old version.
            // if old version  cannot be loaded delete and download again.
            AssetBundle ab = null ;
            attachablesClass adb = assetBundles.getAssetBundleData(experience);
            if(adb != null)
            {
                Debug.Log(adb.file_name);
                if(!adb.versionCheck(experience))
                {
                    Debug.Log("loadind from storage");
                    // not a new version so no need to download
                    // load assetbundle from cache folder
                    ab = await loadAssetBundle(adb);
                    if(ab==null)
                    {
                        Debug.Log("failed to load the experience");
                    }
                    return ab;
                }
                Debug.Log("this experience has been updated and is redownloading");
            }
            return null;
        }

        async Task<AssetBundle> loadAssetBundle(attachablesClass experience)
        {
            string path = readWriteData.GetPath(dataPath.getFolder(0), experience.file_name);
            return await downloadHelper.loadAssetBundleFromStorage(path);
        }


        #endregion
    }
}
