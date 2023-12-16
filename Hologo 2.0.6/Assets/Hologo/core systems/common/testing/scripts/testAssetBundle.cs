using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2;
using System;

public class testAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    public string asset_name;
    public string folderName;
    AssetBundleCreateRequest request;

    void Start()
    {
      //loadAssetBundle();
      StartCoroutine(StartAssetBundleAsyncLoad(asset_name));
    }




    
    public async void loadAssetBundle()
    {
        //bool success = false;
        string filePath = readWriteData.GetPath(folderName,asset_name);
        //request = AssetBundle.LoadFromFileAsync(filePath);
        var task = assetBundleHelper.LoadAssetBundle(folderName,asset_name);
        try
        {
            AssetBundle ab = await task;
            if (ab != null)
            {
                Debug.Log("done");
            }
            else
            {
                Debug.Log("error");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

   

    private void Update()
    {
       // if(request !=null)
        //{
          //  Debug.Log(request.progress);
        //}
    }


    public IEnumerator StartAssetBundleAsyncLoad(string bundleName)
    {
        string filePath = readWriteData.GetPath(folderName,asset_name);
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
        while (!request.isDone)
        {
            Debug.Log(string.Format("Downloaded {0:P1}", request.progress));
            yield return null;
        }
        yield return request.assetBundle;
    
    }

}
