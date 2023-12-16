using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 31 july 2019
    /// Modified by: 
    /// this class helpo students functions
    /// </summary>
    public class studentServerConnect
    {

       
        /// <summary>
        /// //////
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        // loading students from server
        public async Task<string> loadStudentsFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> loadPendingStudents(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        // returns a success string and token if user was able to log in 
        public async Task<string> addStudent(string url, string email)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, form);
        }

        // returns a success string and token if user was able to log in 
        public async Task<string> removeStudent(string url)
        {
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> acceptStudent(string url)
        {
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.fetchDataFromServer(url);
        }


    }
}
