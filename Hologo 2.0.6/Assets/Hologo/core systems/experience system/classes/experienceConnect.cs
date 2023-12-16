using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 16 july 2019
    /// Modified by: 
    /// this class connects to server and storage to get experience associated files
    /// </summary>
    public class experienceConnect
    {
        // get asset bundle
        public async Task<AssetBundle> getAssetBundleExperience(string url, string bundleName, uint version, uint crc)
        {
            return await assetBundleHelper.getAssetBundle(url, bundleName, version, crc);
        }

        public async Task<AssetBundle> loadAssetBundleFromCache(string bundleName,uint crc)
        {
            return await assetBundleHelper.LoadAssetBundle(bundleName, crc);
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



        // get narration file
        // get translation

    }
}
