using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// user profile manager monobehavior 
    /// </summary>
    public class profileManager : messageLogging
    {
        // scriptable object for storing user data
        public userData_SO userData;
        // paths and names
        public dataPaths_SO dataPath;


        // user login 
        public async Task<bool> getProfile()
        {
            bool success = false;
            string result;
            serverProfileConnect profileConnect = new serverProfileConnect();
            var task = profileConnect.getProfile(dataPath.getUrl(0), userData.requestMyToken());

            try
            {
                result = await task;
                GenericObjectAndStatus<HologoAPI> profileAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                Debug.Log(result);
                if (profileAndStatus.GenericObject.success)
                {
                    GenericObjectAndStatus<HologoWebAPIGeneric<UserData>> userProfileAndStatus = jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserData>>(result);
                   // userData.setUserProfile(userProfileAndStatus.GenericObject.data.profile);
                    saveProfileToStorage();
                    success = true;
                }
                else
                {
                    createMessage("error");
                }

            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }

            return success;
          
        }

      
        public async Task<bool> updateProfileToServer(string fullName, string phoneNumber, int lanId, int countryId, string device)
        {
            bool success = false;
            string result;
            serverProfileConnect profileConnect = new serverProfileConnect();
            var task = profileConnect.updateProfile(dataPath.getUrl(1), userData.requestMyToken(), fullName, phoneNumber, lanId, countryId, device);

            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoAPI> userProfileAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                if(userProfileAndStatus.GenericObject.success)
                {
                    GenericObjectAndStatus<HologoWebAPIGeneric<UserDetail>> ProfileAndStatus = jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserDetail>>(result);
                    userData.setUserProfile(ProfileAndStatus.GenericObject.data);
                    saveProfileToStorage();
                    success = true;
                }
                else
                {
                    createMessage("error");
                }
            }
            catch (HologoInternetException ex)
            {

                createMessage(ex.Message);
            }
            return success;
        }


        //save library data to storage
        public bool saveProfileToStorage()
        {
            serverUserConnect userConnect = new serverUserConnect();
            return userConnect.writeUserDataToStorage(userData, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }


    }

}