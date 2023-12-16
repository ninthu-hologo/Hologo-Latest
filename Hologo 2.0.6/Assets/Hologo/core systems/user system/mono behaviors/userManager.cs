using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// user manager monobehavior 
    /// </summary>
    public partial class userManager : messageLogging
    {
        // scriptable object for storing user data
        [Header("DATA")]
        public userData_SO userData;
        // paths and names
        public dataPaths_SO dataPath;


        public void updateUser(UserData ud)
        {
            userData.setOldUserData(ud.user, ud.user.password);
            saveUserToStorage();
        }

        public void SignOutUser()
        {
            userData.signOutUser();
            saveUserToStorage();
        }


        public bool isUserAnon()
        {
            return userData.isUserAnon();
        }
        
        // load user from storage or make a new user if cant
        public bool loadUserFromStorage()
        {
            bool success = false;
            string result;
            if (userData.isUserDataFilled())
                return true;
            Debug.Log("user load");
            if (checkIfUserDataExists())
            {
                Debug.Log("user exists");
                result = readUserDataFromStorage();
                if(string.IsNullOrEmpty(result))
                {
                    // no user was read so make new one
                    userData.makeUser();
                    //saving userData;
                    saveUserToStorage();
                }
                else
                {
                    if(convertToUserData(result))
                    {
                        // user was able to load.
                        success = true;
                    }
                    else
                    {
                        // no user was read so make new one
                        userData.makeUser();
                        //saving userData;
                        saveUserToStorage();
                    }
                }

            }
            else
            {
                Debug.Log("user doesnt exists");
                // make new demo user
                // no user was read so make new one
                userData.makeUser();
                //saving userData;
                saveUserToStorage();
            }

            return success;
        }


        public bool doesAUserAlreadyExist(string email)
        {

            if(userData.isNewUser(email))
            {
                return true;
            }

            return false;
        }

        // auto login during boot up;
        public async Task<bool> autoLogIn()
        {
            return await logInUser(userData.requestUserDetail().email, userData.requestUserDetail().password);
        }


        // user login
        //Shariz 13th April 2020 v2.0.5 checking whether user is the correct user type before letting them log in
        public async Task<bool> logInUser(string email, string password)
        {
            bool success = false;
            string result;
            serverUserConnect userConnect = new serverUserConnect();
            var task = userConnect.logInUser(dataPath.getUrl(0), email, password);

            try
            {
                result = await task;

                // serialzing data from server to object
                GenericObjectAndStatus<HologoUserAuthAndData> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoUserAuthAndData>(result);
                Debug.Log("loging result "+result);
                if (ApiAndStatus.GenericObject.success)
                {
                    UserDataAuth userDataTemp = ApiAndStatus.GenericObject.data;
                    if(ApiAndStatus.GenericObject.data != null && (ApiAndStatus.GenericObject.data.user.user_type == "teacher" || ApiAndStatus.GenericObject.data.user.user_type == "student"))
                    {
                        if (ApiAndStatus.GenericObject.data != null)
                        {
                            userData.setUserData(userDataTemp, password);
                            userData.setUserAsSignedIn(ApiAndStatus.GenericObject.data.token);
                        }

                        saveUserToStorage();
                        success = true;

                    } else
                    {
                        createMessage("Only teachers and students can log in to the Hologo app");
                    }
                    

                   // userData.setMyToken(ApiAndStatus.GenericObject.);
                    //saving userData;
                    
                    

                }
                else
                {
                    //invalid credentials
                    createMessage(ApiAndStatus.GenericObject.message);
                }
            }

            catch (HologoInternetException ex)
            {
                Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }

        public async Task<bool> createUser(string userName, string email, string password, string userType, int languageId, int countryId, string device, bool usePhone, int phoneNumber = 0)
        {
            
            bool success = false;
            string result;
            serverUserConnect userConnect = new serverUserConnect();
            var task = userConnect.createAccount(dataPath.getUrl(1), userName, email, password, userType, languageId, countryId, device, usePhone, phoneNumber);

            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoAPI> signupAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                Debug.Log("result of trying for singupandstatus: "+result);

                GenericObjectAndStatus<HologoWebAPIGeneric<UserDetail>> userdetailAndStatus =
                        jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserDetail>>(result);


                if (signupAndStatus.GenericObject.success)
                {

                    userData.setNewUserData(userdetailAndStatus.GenericObject.data, password);

                    saveUserToStorage();
                    success = true;
                }
                else
                {
                    createMessage(signupAndStatus.GenericObject.message);
                    
                }
            }
            catch (HologoInternetException ex)
            {
                
                createMessage(ex.Message);
            }

            return success;
        }

        // send a new password to user email
        public async Task<bool> forgotPassword(string email)
        {
            bool success = false;
            string result;
            serverUserConnect userConnect = new serverUserConnect();
            var task = userConnect.ForgotPassword(dataPath.getUrl(2), email);

            try
            {
                result = await task;

                GenericObjectAndStatus<HologoAPI> forgotPasswordAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                if (forgotPasswordAndStatus.GenericObject.success)
                {
                    success = true;
                }
                else
                {
                    createMessage(forgotPasswordAndStatus.GenericObject.message);
                }

            }
            catch (HologoInternetException ex)
            {

                createMessage(ex.Message);
            }

            return success;
        }

        /// <summary>
        /// this method updates the password. can be followed with a log-in 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public async Task<bool> updatePassword(string newPassword, string oldPassword)
        {
            bool success = false;

            if(!userData.CheckCurrentPassword(oldPassword))
            {
                createMessage("You entered your old password incorrectly!"); // Shariz hologo 2.0.2 10th dec 2019
                return false;
            }
             Debug.Log("old password is "+oldPassword+" and the new password is "+newPassword);   
             Debug.Log(userData.requestMyToken());   
            string result;
            serverUserConnect userConnect = new serverUserConnect();
            var task = userConnect.updatePassword(dataPath.getUrl(3), newPassword, oldPassword, userData.requestMyToken());
            try
            {
                result = await task;
                GenericObjectAndStatus<HologoAPI> changePasswordAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                if (changePasswordAndStatus.GenericObject.success)
                {
                    // set the password to the new one
                    userData.setUserPassword(newPassword);
                    //saving userData;
                    saveUserToStorage();
                    success = true;
                }
            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }
            return success;
        }

        public bool deleteUserDataFile()
        {
            userData.flushUserData();
            serverUserConnect userConnect = new serverUserConnect();
            return userConnect.deleteUserData(dataPath.getFolder(0),dataPath.getFileName(0));
        }

        //save library data to storage
        public bool saveUserToStorage()
        {
            serverUserConnect userConnect = new serverUserConnect();
            return userConnect.writeUserDataToStorage(userData, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // check if userdata file exists
        // helper functions
        bool checkIfUserDataExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        // read user data from storage
        string readUserDataFromStorage()
        {
            serverUserConnect userConnect = new serverUserConnect();
            return userConnect.loadUserDataFromStorage(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //convert stored data to data object
        bool convertToUserData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, userData))
            {
                success = true;
            }
            return success;
        }

        public bool isUserSignedIn()
        {
           return userData.isUserSignedIn();
        }

    }

}




/*{"success":true,
 * "data":{
 * "token":"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9saXRlLmhvbG9nby53b3JsZFwvYXBpXC9sb2dpbiIsImlhdCI6MTU2NzI0ODI2OCwiZXhwIjoxNTY3MjUxODY4LCJuYmYiOjE1NjcyNDgyNjgsImp0aSI6IkZ0MFdtY2pMdU4xMWhzMHYiLCJzdWIiOjI1NzQwLCJwcnYiOiI4N2UwYWYxZWY5ZmQxNTgxMmZkZWM5NzE1M2ExNGUwYjA0NzU0NmFhIn0.VaaftDyJKK984z3rasotGemp0HV6fhK6UrUOfF-7mSE",
 * "user":{
 * "id":25740,
 * "name":"testuser121",
 * "email":"testuser121@k.k",
 * "email_verified_at":null,
 * "verified":1,
 * "contact_number":null,
 * "language_id":6,
 * "country_id":1,
 * "user_type":"student",
 * "current_login_device":"ios",
 * "trial_ends_at":"2019-09-29",
 * "remember_token":null,
 * "created_at":"2019-08-30 07:44:08",
 * "updated_at":"2019-08-30 07:44:08"}},
 * "message":"authorized"}*/