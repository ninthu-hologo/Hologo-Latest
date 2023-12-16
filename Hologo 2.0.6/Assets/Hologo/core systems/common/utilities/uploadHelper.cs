using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

namespace Hologo2
{
    public static class uploadHelper
    {
        public static string internetError = "error";
        public static float fileUploadProgress = 0;

        // upload file to server
        public static async Task<bool> uploadFileToServer(string url, string fileName, string folder, int timeOut = 120)
        {
            bool success = false;

            await new WaitForSeconds(1);

            return success;
        }

        // upload file to server
        public static async Task<System.Tuple<bool, bool,string>> uploadFileToServerWithForm(WWWForm Form, string url, string fieldName,string fileName, string folder, string mimeType, int timeOut = 120)
        {
            bool success = true;
            bool successInValidating = false;
            string t = "";

            //string filepath = "file://" + readWriteData.GetPath(folder,fileName);
            //filepath = System.Uri.EscapeUriString(filepath);
            //Debug.Log(filepath);
            //byte[] myData = File.ReadAllBytes(filepath);
            //Debug.Log("my data lenght :" + myData.Length);

            string filepath = readWriteData.GetPath(folder,fileName);
            Debug.Log(filepath);
            enableLoadingScreen();
            byte[] myData = File.ReadAllBytes(filepath);

            Form.AddBinaryData(fieldName, myData, fileName, mimeType);

            UnityWebRequest uploader = new UnityWebRequest();
            uploader = UnityWebRequest.Post(url, Form);
           

            uploader.SendWebRequest();

            while(!uploader.isDone)
            {
                fileUploadProgress = uploader.uploadProgress;
                await Task.Yield();
            }
            disableLoadingScreen();
            Debug.Log("upload text >"+ uploader.downloadHandler.text);
           // check to see if any errors occured
            if (InternetfetchData.NetworkOrServerError(uploader))
            {
               // Debug.Log()
                success = false;
                throw new HologoInternetException(internetError);
            }
            else
            {
                GenericObjectAndStatus<HologoWebAPIGeneric<string>> mystatusUpload =
                jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<string>>(uploader.downloadHandler.text);
                successInValidating = mystatusUpload.GenericObject.success;
                t = uploader.downloadHandler.text;
            }
            return new System.Tuple<bool, bool,string>(success, successInValidating, t);
        }



        static void enableLoadingScreen()
        {
            if (Application.isPlaying)
            {
                commonLoadingScreenSingleton.Singleton.enableLoadingScreen();
            }
        }

        static void disableLoadingScreen()
        {
            if (Application.isPlaying)
            {
                commonLoadingScreenSingleton.Singleton.disableLoadingScreen();
            }
        }
    }
}