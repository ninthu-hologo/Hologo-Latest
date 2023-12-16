using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// this handles profile get and update
    /// </summary>
    public class serverProfileConnect
    {

        // returns a json string after quering server
        public async Task<string> getProfile(string url, string token)
        {
            string urlAndToken = url + "?token=" + token;
            urlAndToken = System.Uri.EscapeUriString(urlAndToken);
            return await InternetfetchData.fetchDataFromServer(urlAndToken);
        }

        // returns a success string after profile is updated
        public async Task<string> updateProfile(string url, string token, string fullName, string phoneNumber,int lanId,int countryId,string device)
        {
            WWWForm profileForm = new WWWForm();
            profileForm.AddField("name", fullName);
            if(!string.IsNullOrEmpty(phoneNumber))
            {
                profileForm.AddField("contact_number", phoneNumber);
            }
            profileForm.AddField("language_id", lanId);
            profileForm.AddField("country_id", countryId);
            profileForm.AddField("current_login_device", device);

            string urlAndToken = url + "?token=" + token;
            urlAndToken = System.Uri.EscapeUriString(urlAndToken);
            return await InternetfetchData.submitFormToServer(urlAndToken, profileForm);
        }


        // writing data to storage
        public bool writeUserDataToStorage<T>(T jsonObject, bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson, checkFileExits, pathStrings);
        }


    }
}
