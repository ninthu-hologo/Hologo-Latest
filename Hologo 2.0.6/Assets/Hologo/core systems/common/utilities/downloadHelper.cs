using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


namespace Hologo2
{
    public static class downloadHelper
    {
        public static string internetError = "error";
        public static float progress = 0;
        public static float previouseProgress = 0f;
        public static bool isDone = true;

        // download and save used for small size files. file extensions should be provided
        public static async Task<bool> downloadToBufferAndSave(string url, string folder, string fileName, int timeOut = 10)
        {
            bool success = true;
            url = System.Uri.EscapeUriString(url);

            UnityWebRequest fileGet = new UnityWebRequest(url);
            fileGet.downloadHandler = new DownloadHandlerBuffer();
            fileGet.timeout = timeOut;
            isDone = false;
            await fileGet.SendWebRequest();
            isDone = true;
            // check to see if any errors occured
            if (InternetfetchData.NetworkOrServerError(fileGet))
            {
                success = false;

                //                throw new HologoInternetException(internetError);
            }
            else
            {
                //success and write the file to storage
                string filepath = readWriteData.GetPath(folder, fileName);
                //  Utilities.Utilities.IosNoBackupFlagging(filepath);
                if (fileGet.downloadHandler.data.Length > 100)
                {
                    //Debug.Log("file downloaded >" + fileName);
                    // Hamid 1st Nov fixing image download caution
                    try
                    {
                        File.WriteAllBytes(filepath, fileGet.downloadHandler.data);
                    }
                    catch (System.Exception ex)
                    {
                        success = false;
                    }
                }
                else
                {
                    success = false;
                }
            }
            if (success)
            {
                Debug.Log("success");
            }
            else
            {
                Debug.Log("false");
            }
            // fileGet.Dispose();
            return success;
        }


        // download file to storage. for big files 
        public static async Task<bool> downloadToStorage(string url, string folder, string fileName, bool combineFileNameAndUrl, int timeOut = 120)
        {
            bool success = true;
            if (combineFileNameAndUrl)
            {
                url = System.Uri.EscapeUriString(url + fileName);
            }
            else
            {
                url = System.Uri.EscapeUriString(url);
            }

            string filepath = readWriteData.GetPath(folder, fileName);
            //  Utilities.Utilities.IosNoBackupFlagging(filepath);
            UnityWebRequest fileDownloadRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            fileDownloadRequest.downloadHandler = new DownloadHandlerFile(filepath);

            // even though this has a warning the execution will await because of task.yield()
            fileDownloadRequest.SendWebRequest();
            // time our faile safe
            bool run = true;
            int counter = 0;
            int progressing = 0;
            isDone = false;
            while (run)
            {

                if (!fileDownloadRequest.isDone)
                {
                    if (progressing == Mathf.RoundToInt(fileDownloadRequest.downloadProgress * 100))
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }

                    progress = fileDownloadRequest.downloadProgress;
                    // Debug.Log(progress);
                    progressing = Mathf.RoundToInt(fileDownloadRequest.downloadProgress * 100);
                    await Task.Yield();
                }
                else
                {
                    run = false;
                }

                // if download is not progressing we just cancel it.
                if (counter > 1000)
                {
                    fileDownloadRequest.Abort();
                    run = false;
                    success = false;
                    counter = 0;
                }
            }

            isDone = true;
            //check to see if any errors occured
            if (InternetfetchData.NetworkOrServerError(fileDownloadRequest))
            {
                success = false;
                throw new HologoInternetException(internetError);
            }

            previouseProgress = progress;
            progress = 0;


            fileDownloadRequest.Dispose();

            return success;
        }

        public static void resetProgress()
        {
            previouseProgress = 0f;
            progress = 0f;
        }


        public static async Task<AssetBundle> loadAssetBundleFromStorage(string path)
        {
            AssetBundle ab = null;
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            //  isDone = false;
            while (!request.isDone)
            {
                // progress = request.progress;
                await Task.Yield();
            }
            //  isDone = true;
            ab = request.assetBundle;
            // progress = 0;

            return ab;
        }

    }
}
