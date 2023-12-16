using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 30 june 2019
    /// Modified by: 
    /// this class gets languages from server and storage
    /// </summary>
    public class languageDataConnect
    {
        // download language
        public async Task<bool> downloadLanguage(string url, string folderName,string fileName )
        {
           // return await downloadHelper.downloadToBufferAndSave(url, folderName,fileName);
            return await downloadHelper.downloadToStorage(url, folderName, fileName, false);
        }


        // load asset bundle
        public async Task<AssetBundle> loadLanguageFromStorage(params string[] pathStrings)
        {
            return await assetBundleHelper.LoadAssetBundle(pathStrings);
        }

        // delete assetbundle
        public bool deleteLangaugeAssetBundle(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }

        

        public languageData_SO loadlanguageFromAsset(string assetName, AssetBundle ab)
        {
            //ab.LoadAllAssets();
            return (ab.LoadAsset(assetName) as languageData_SO);
        }

    }
}
