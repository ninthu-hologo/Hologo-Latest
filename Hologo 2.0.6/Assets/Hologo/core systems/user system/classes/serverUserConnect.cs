using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// this handles user log-in
    /// </summary>
    public class serverUserConnect
    {

        // returns a success string and token if user was able to log in 
        public async Task<string> logInUser(string url,string email, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url,form);
        }


        // create account
        public async Task<string> createAccount(string url,string userName, string email, string password, string userType , int languageId, int countryId, string device, bool usePhone,int phoneNumber =0)
        {
            WWWForm signUpForm = new WWWForm();
            signUpForm.AddField("name", userName);
            signUpForm.AddField("email", email);
           // signUpForm.AddField("contact_number", 0);
            signUpForm.AddField("password", password);
            signUpForm.AddField("language_id", languageId);
            signUpForm.AddField("country_id", countryId);
            signUpForm.AddField("user_type", userType);
            signUpForm.AddField("current_login_device", device);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, signUpForm);
        }

        // send new password to user email
        public async Task<string> ForgotPassword(string url,string email)
        {
            WWWForm passwordForm = new WWWForm();
            passwordForm.AddField("email", email);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, passwordForm);
        }

        //change password
        public async Task<string> updatePassword(string url, string newPassword , string oldPassword,string token)
        {
            WWWForm ChangepasswordForm = new WWWForm();
            ChangepasswordForm.AddField("old_password", oldPassword); // Shariz hologo 2.0.2 10th dec 2019
            ChangepasswordForm.AddField("new_password", newPassword); // Shariz hologo 2.0.2 10th dec 2019
            url = url + "?token=" + token;
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, ChangepasswordForm);
        }


        // writing data to storage
        public bool writeUserDataToStorage<T>(T jsonObject,bool checkFileExits, params string[] pathStrings)
        {
            string expToJson = JsonUtility.ToJson(jsonObject, true);
            return readWriteData.WriteFileToDisk(expToJson,checkFileExits, pathStrings);
        }


        public string loadUserDataFromStorage(params string[] pathStrings)
        {
            string dataString = readWriteData.ReadFileFromDisk(pathStrings);
            return dataString;
        }

        public bool deleteUserData(params string[] pathStrings)
        {
            return readWriteData.DeleteFileOnDisk(pathStrings);
        }

    }


    

}
