using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 september 2019
    /// Modified by: 
    /// assetbundle data connector
    /// </summary>
    public class assetBundleServerConnect
    {

        // loading data from storage
        public string loadAssetBundleDataFromLocalStorage(params string[] pathStrings)
        {
            return readWriteData.ReadFileFromDisk(pathStrings);
        }

        // writing data to storage
        public bool writeAssetBundleDataToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson, checkFileExits, pathStrings);
        }

        // delete library
        public bool deleteAssetBundleData(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }


        /// <summary>
        /// download asset bundle from server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> downloadAssetBundle(string url, string folderName, string fileName)
        {
            return await downloadHelper.downloadToStorage(url, folderName, fileName, false);
        }

        public async Task<AssetBundle> loadAssetBundleFromCache(string folderName, string bundleName)
        {
            return null;// await assetBundleHelper.LoadAssetBundle(bundleName, crc);
        }
        // unloading the loaded assetbundle
        public void unloadAssetBundle(AssetBundle bundle)
        {
            if (bundle == null)
                return;

            bundle.Unload(true);
            Resources.UnloadUnusedAssets();
        }

        // loading asset from assetbundle
        public experienceData_SO loadExperienceAsset(string assetName, AssetBundle ab)
        {
            //ab.LoadAllAssets();
            return (ab.LoadAsset(assetName) as experienceData_SO);
        }

        // loading localized asset from assetbundle
        public experienceLocalizationData_SO loadExperienceLocalizationAsset(string assetName, AssetBundle ab)
        {
            //ab.LoadAllAssets();
            return (ab.LoadAsset(assetName) as experienceLocalizationData_SO);
        }

    }


}
