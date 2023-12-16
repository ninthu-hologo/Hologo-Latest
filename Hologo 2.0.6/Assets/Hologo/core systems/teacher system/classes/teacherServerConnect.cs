using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 31 july 2019
    /// Modified by: 
    /// this class helpo teacher functions
    /// </summary>
    public class teacherServerConnect
    {



        // loading students from server
        public async Task<string> loadTeachersFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> loadPendingTeachers(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        // returns a success string and token if user was able to log in 
        public async Task<string> addTeacher(string url, string email)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, form);
        }

        // returns a success string and token if user was able to log in 
        public async Task<string> removeTeacher(string url)
        {
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> acceptTeacher(string url)
        {
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.fetchDataFromServer(url);
        }

    }
}
