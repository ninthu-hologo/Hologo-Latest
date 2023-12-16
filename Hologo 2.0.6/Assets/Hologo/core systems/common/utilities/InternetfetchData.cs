using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;
//using Hologo;


namespace Hologo2
{
    public static class InternetfetchData
    {
        public static string internetError = "error";


        // fetching resApi data from server
        public static async Task<string> fetchDataFromServer(string url,bool loadScreenState = true)
        {
            string address = System.Uri.EscapeUriString(url);
            string p = "";
            UnityWebRequest uwr = UnityWebRequest.Get(address);
            uwr.timeout = appEVars.WebTimeOut;
            enableLoadingScreen(loadScreenState);
            await uwr.SendWebRequest();
            disableLoadingScreen(loadScreenState);
            if(NetworkOrServerError(uwr)/*uwr.isNetworkError || uwr.isHttpError*/)
            {
                throw new HologoInternetException(internetError);
            }

            p = uwr.downloadHandler.text;
            return p;
        }


        public static async Task<string> deleteDataFromServer(string url, bool loadScreenState = true)
        {
            string address = System.Uri.EscapeUriString(url);
            string p = "";
            UnityWebRequest uwr = UnityWebRequest.Delete(address);
            uwr.timeout = appEVars.WebTimeOut;
            enableLoadingScreen(loadScreenState);
            await uwr.SendWebRequest();
            disableLoadingScreen(loadScreenState);
            //if (NetworkOrServerError(uwr)/*uwr.isNetworkError || uwr.isHttpError*/)
            //{
            //    throw new HologoInternetException(internetError);
            //}

            p = uwr.downloadHandler.text;
            Debug.Log(p);
            return p;
        }



        // submit form and fetch response
        public static async Task<string> submitFormToServer(string url, WWWForm form, bool loadScreenState = true)
        {
            string address = System.Uri.EscapeUriString(url);
            string p = "";

            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.timeout = appEVars.WebTimeOut;
            enableLoadingScreen(loadScreenState);
            uwr.SetRequestHeader("Accept", "application/json"); //SHARIZ 28th Oct 2019 2.00
            await uwr.SendWebRequest();
            disableLoadingScreen(loadScreenState);
            Debug.Log("from soruce"+uwr.downloadHandler.text);

            if (NetworkOrServerError(uwr))
            {
                throw new HologoInternetException(internetError);
            }

            p = uwr.downloadHandler.text;
            Debug.Log(p);
            return p;

        }



        // getting network error messages
       public static bool NetworkOrServerError(UnityWebRequest uwr)
        {
            bool error = false;
            string serror = "";

            if (!string.IsNullOrEmpty(uwr.error))
            {
                if (uwr.error.Equals("Request timeout", System.StringComparison.Ordinal))
                {
                    serror = "Request timeout";
                    error = true;
                }
                else
                {
                    if (uwr.isNetworkError)
                    {
                        serror = "Something is wrong with \n the internet!";
                        error = true;
                    }
                    if (uwr.isHttpError)
                    {
                        serror = "There seems to be a problem, \n please try again later!";
                        error = true;
                    }
                }
            }        

            internetError = serror;
            return error;
        }



        static void enableLoadingScreen(bool state)
        {
            if (!state)
                return;

            if (Application.isPlaying)
            {
                commonLoadingScreenSingleton.Singleton.enableLoadingScreen();
            }
        }

        static void disableLoadingScreen(bool state)
        {
            if (!state)
                return;

            if (Application.isPlaying)
            {
                commonLoadingScreenSingleton.Singleton.disableLoadingScreen();
            }
        }
    }
}