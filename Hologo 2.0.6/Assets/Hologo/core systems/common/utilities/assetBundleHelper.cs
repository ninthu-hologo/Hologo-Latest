using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2;
using UnityEngine.Networking;
using System.IO;

public static class assetBundleHelper
{
    public static string internetError = "error";
    public static float progress;
    public static bool isDone = true;

    //async asset bundle load
    public static async Task<AssetBundle> LoadAssetBundle(params string[] pathStrings)
    {
        string filePath = readWriteData.GetPath(pathStrings);
        
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
        while (!request.isDone)
        {
            isDone = false;
            progress = request.progress;
            // yeilding the await so progress can be querid
            // dont know if its the best practise
            await Task.Yield();
        }
        isDone = true;
        return request.assetBundle;
    }

    //async asset bundle load
    public static async Task<AssetBundle> LoadAssetBundle(string fileName , uint crc)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(fileName, crc);
        while (!request.isDone)
        {
            isDone = false;
            progress = request.progress;
            // yeilding the await so progress can be querid
            // dont know if its the best practise
            await Task.Yield();
        }
        isDone = true;
        return request.assetBundle;
    }

    // loading asset bundle from server or cache
    public static async Task<AssetBundle> getAssetBundle(string url,string bundleName, uint version, uint crc)
    {
        Debug.Log("get assetbundle>>");
        url = System.Uri.EscapeUriString(url);
        // this will load from cache if crc and version is same . otherwise will download from server
        UnityWebRequest assetbundleGet = UnityWebRequestAssetBundle.GetAssetBundle(url,version,0);
        //UnityWebRequest assetbundleGet = UnityWebRequestAssetBundle.GetAssetBundle(url);

        assetbundleGet.SendWebRequest();
        while (!assetbundleGet.isDone)
        {
            isDone = false;
            progress = assetbundleGet.downloadProgress;
            Debug.Log("dl>"+progress);

            await Task.Yield();
        }
        isDone = true;

        if (InternetfetchData.NetworkOrServerError(assetbundleGet))
        {
            throw new HologoInternetException(internetError);
        }
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(assetbundleGet);
        return ab;
    }

    // for saving to customized path
    public static async Task<AssetBundle> getAsseBundle(Cache cache,string url,string folder, string hash)
    {
        url = System.Uri.EscapeUriString(url);

        readWriteData.Createfolder(folder);
        if(cache.valid)
        {
            Caching.currentCacheForWriting = cache;
        }
        else
        {
            cache = Caching.AddCache(readWriteData.GetPath(folder));
        }
        Hash128 hash128 = Hash128.Parse(hash);
        UnityWebRequest assetbundleGet = UnityWebRequestAssetBundle.GetAssetBundle(url, hash128, 0);
        assetbundleGet.SendWebRequest();
        while (!assetbundleGet.isDone)
        {
            isDone = false;
            progress = assetbundleGet.downloadProgress;
            Debug.Log(progress);

            await Task.Yield();
        }
        isDone = true;

        if (InternetfetchData.NetworkOrServerError(assetbundleGet))
        {
            throw new HologoInternetException(internetError);
        }
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(assetbundleGet);
        return ab;
    }

    // clears all cached assetbundles . to free up storage
    public static bool clearAssetBundleCache()
    {
        Cache mycache = new Cache();
        return mycache.ClearCache();
    }

    ////coroutine load assetbundle async
    //public static IEnumerator StartAssetBundleAsyncLoad(string bundleName, string folderName)
    //{
    //    string filePath = readWriteData.GetPath(folderName,bundleName);
    //    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
    //    while (!request.isDone)
    //    {
    //        progress = request.progress;
    //       // Debug.Log(string.Format("Downloaded {0:P1}", request.progress));
    //        yield return null;
    //    }
    //    yield return request.assetBundle;
    //}
}
