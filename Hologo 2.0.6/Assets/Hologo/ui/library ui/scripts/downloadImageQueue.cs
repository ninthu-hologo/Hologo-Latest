using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
using Hologo2.library;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.IO;

public class downloadImageQueue : MonoBehaviour
{
    [SerializeField]
    dataPaths_SO dataPath;
    [SerializeField]
    private List<experienceUIElement> imageList;
    [SerializeField]
    bool isDownloadingFinished = true;
    private bool dlSuccess;

    public void addToDownloadList(experienceUIElement image)
    {
        if (imageList == null)
            imageList = new List<experienceUIElement>();

        Debug.Log("image has been added to downloads");
        imageList.Add(image);

        if (isDownloadingFinished)
        {
            // start downloading again;
            // runDownloadImages();
            StopAllCoroutines();
            StartCoroutine(downloadImagesQueueCoroutine());
        }
    }

    private async void runDownloadImages()
    {
        await downloadImagesQueue();
    }

    private async Task downloadImagesQueue()
    {
        //if(imageList.Count>0)
        //{

        for (int i = 0; i < imageList.Count; i++)
        {
            isDownloadingFinished = false;
            experienceUIElement ee = imageList[i];
            Debug.Log("number of images left " + imageList.Count);

            if (ee != null)
            {

                experienceDataClass expD = ee.getData() as experienceDataClass;
                bool success = await downloadImage(expD.experience.image.file_name, expD.experience_id, expD.title);
                if (success)
                {
                    // update image if its on.
                    if (ee != null)
                    {
                        ee.loadImageAfterDownload();
                    }
                }
            }
            imageList.RemoveAt(i);
        }

        //}
        //else
        //{

        if(imageList.Count == 0)
        {
            Debug.Log("image downloads has finished");
            isDownloadingFinished = true;
        }else
        {
            runDownloadImages();
        }
        
        //}
    }


    private async Task<bool> downloadImage(string fileName, int id, string expName)
    {
        //if (readWriteData.CheckIfFileExists(dataPath.getFolder(0), fileName))
        //    return true;
        string url = dataPath.getUrl(1) + id + dataPath.getUrl(2);
        Debug.Log("image for " + expName + " is downloading");
        return await downloadHelper.downloadToBufferAndSave(url, dataPath.getFolder(0), fileName);
    }


    #region COROUTINE DOWNLOAND IMAGES

    private IEnumerator downloadImagesQueueCoroutine()
    {
        //if(imageList.Count>0)
        //{

        for (int i = 0; i < imageList.Count; i++)
        {
            isDownloadingFinished = false;
            experienceUIElement ee = imageList[i];
            Debug.Log("number of images left " + imageList.Count);

            if (ee != null)
            {

                experienceDataClass expD = ee.getData() as experienceDataClass;
                yield return StartCoroutine(downloadImageCoroutine(expD.experience.image.file_name, expD.experience_id, expD.title));
                if (dlSuccess)
                {
                    // update image if its on.
                    if (ee != null)
                    {
                        ee.loadImageAfterDownload();
                    }
                }
            }
            imageList.RemoveAt(i);

            yield return null;
        }

        //}
        //else
        //{

        if (imageList.Count == 0)
        {
            Debug.Log("image downloads has finished");
            isDownloadingFinished = true;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(downloadImagesQueueCoroutine());
        }

        yield return null;
        //}
    }


    private IEnumerator downloadImageCoroutine(string fileName, int id, string expName)
    {
        string url = dataPath.getUrl(1) + id + dataPath.getUrl(2);
        Debug.Log("image for " + expName + " is downloading");
        yield return StartCoroutine(DownloadAFileToBufferAndSave(url, dataPath.getFolder(0), fileName));
    }


     IEnumerator DownloadAFileToBufferAndSave(string url, string folder, string fileName, int timeOut = 10)
    {


        url = System.Uri.EscapeUriString(url);
        dlSuccess = true;

        UnityWebRequest imageGet = new UnityWebRequest(url);
        imageGet.downloadHandler = new DownloadHandlerBuffer();
        imageGet.timeout = 80;
        yield return imageGet.SendWebRequest();

        if (InternetfetchData.NetworkOrServerError(imageGet))
        {
            dlSuccess = false;

            //                throw new HologoInternetException(internetError);
        }
        else
        {
            //success and write the file to storage
            string filepath = readWriteData.GetPath(folder, fileName);
            //  Utilities.Utilities.IosNoBackupFlagging(filepath);
            if (imageGet.downloadHandler.data.Length > 100)
            {
                //Debug.Log("file downloaded >" + fileName);
                try
                {
                    File.WriteAllBytes(filepath, imageGet.downloadHandler.data);
                }
                catch (System.Exception ex)
                {
                    dlSuccess = false;
                }

            }
            else
            {
                dlSuccess = false;
            }
        }
        if (dlSuccess)
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log("false");
        }
        // fileGet.Dispose();
        yield return null;
    }
    #endregion

}
