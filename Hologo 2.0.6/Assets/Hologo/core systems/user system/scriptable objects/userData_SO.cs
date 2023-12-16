using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Hologo;
//using Hologo.Utilities;

namespace Hologo2
{
    
    [CreateAssetMenu(fileName = "userData.asset", menuName = "Hologo V2/userData")]
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// this scriptable object is used to store user data
    /// </summary>
    public class userData_SO : ScriptableObject
    {
        [SerializeField]
        private UserData userData;


        // setting the data of user after login
        public void setUserData(UserDataAuth userTempData, string password)
        {
            
            userData.token = userTempData.token;
            userData.user.email = userTempData.user.email;
            userData.user.id = userTempData.user.id;
            userData.user.user_type = userTempData.user.user_type;
            userData.user.verified = userTempData.user.verified;
            userData.user.country_id = userTempData.user.country_id;
            userData.user.language_id = userTempData.user.language_id;
            userData.user.current_login_device = userTempData.user.current_login_device;
            userData.user.user_type_id = userTempData.user.user_type_id;
            userData.user.name = userTempData.user.name;
            userData.user.contact_number = userTempData.user.contact_number;
            userData.user.password = password;
            userData.user.IsSignedIn = true;
            userData.user.IsSubscribed = false;
            userData.user.user_type_id = setInternalUserType(userTempData.user.user_type);
        }

        // setting the data of user after create new user
        public void setNewUserData(UserDetail userTempData, string password)
        {

            UserData newuser = new UserData();
            newuser.user = new UserDetail();
            newuser.profile = new UserProfile();
            newuser.user.name = userTempData.name;
            newuser.user.password = password;
            newuser.user.email = userTempData.email;
            newuser.user.user_type = userTempData.user_type;
            newuser.user.user_type_id = setInternalUserType(userTempData.user_type);
            newuser.user.language_id = userTempData.language_id;
            newuser.user.country_id = userTempData.country_id;
            newuser.user.verified = userTempData.verified;
            newuser.user.current_login_device = userTempData.current_login_device;
            newuser.user.id = userTempData.id;

            userData = newuser;
        }


        public void setOldUserData(UserDetail userTempData, string password)
        {
            UserData newuser = new UserData();
            newuser.user = new UserDetail();
            newuser.profile = new UserProfile();
            newuser.user.name = userTempData.name;
            newuser.user.password = password;
            newuser.user.email = userTempData.email;
            newuser.user.user_type = userTempData.user_type;
            newuser.user.user_type_id = setInternalUserType(userTempData.user_type);
            newuser.user.language_id = userTempData.language_id;
            newuser.user.country_id = userTempData.country_id;
            newuser.user.verified = userTempData.verified;
            newuser.user.current_login_device = userTempData.current_login_device;
            newuser.user.id = userTempData.id;
            newuser.user.IsSignedIn = userTempData.IsSignedIn;

            userData = newuser;
        }

        public void signOutUser()
        {
            deletePassword();
            userData.token = "";
            userData.user.IsSignedIn = false;
            userData.user.IsSubscribed = false;
        }

        public void deletePassword()
        {
            userData.user.password = "";
        }

        int setInternalUserType(string type)
        {
            int intype = 0;
            if(string.Equals(type,"student",System.StringComparison.Ordinal))
            {
                intype = 2;
            }else
            {
                intype = 1;
            }

            return intype;
        }

        public void setUserPassword(string password)
        {
            userData.user.password = password;
        }

        //checking if its a new user
        public bool isNewUser(string email)
        {
            if(userData.user.email == "")
            {
                return false;
            }
            return !email.Equals(userData.user.email, System.StringComparison.Ordinal);
        }


        // comparing the entered password and the saved password
        public bool CheckCurrentPassword(string password)
        {
            return password.Equals(userData.user.password, System.StringComparison.Ordinal);
        }

        //method that makes a default user-> usualy when app is opened for the first time.
        // or if user cant be loaded
        public void makeUser()
        {
            UserData newuser = new UserData();
            newuser.user = new UserDetail();
            newuser.profile = new UserProfile();
            newuser.user.name = "HologoAnonymous";
            newuser.user.email = "";
            newuser.user.password = "";
            newuser.user.user_type = "";
            newuser.user.user_type_id = appEVars.AnonymouseTypeId;
            newuser.user.id = 0;
            newuser.user.IsSignedIn = false;
            newuser.user.IsSignedIn = false;
            userData = newuser;
        }

        public void flushUserData()
        {
            makeUser();

        }

        public bool isUserAnon()
        {
            return userData.user.user_type != "";
        }

        public void setUserAsSignedIn( string token)
        {
            userData.token = token;
            userData.user.IsSignedIn = true;
           // userData.user.IsSubscribed = true;
        }


        public void setUserAsSubscribed()
        {
            userData.user.IsSubscribed = true;
        }

        public void userIsNotSubscribed() // hamid 2.0.2 26th Nov 2019
        {
            userData.user.IsSubscribed = false;
        }


        public int getUserType()
        {
            return userData.user.user_type_id;
        }

        // send back token
        public string requestMyToken()
        {
            return userData.token;
        }

        public void setMyToken(string token)
        {
            userData.token = token;
        }
        // set profile
        public void setUserProfile(UserDetail user)
        {
            userData.user.name = user.name;
            userData.user.contact_number = user.contact_number;
        }

        // send back profile
        public UserProfile requestProfile()
        {
            return userData.profile;
        }
        // send back user
        public UserDetail requestUserDetail()
        {
            return userData.user;
        }

        // checking to see if user is filled
        public bool isUserDataFilled()
        {
            return userData.user.id > 0;
        }

        // user signed in already check
        public bool isUserSignedIn()
        {
            return userData.user.IsSignedIn;
        }

        // do user has a subscription
        public bool isUserSubscribed()
        {
            Debug.Log("User subscribed alread!");
            return userData.user.IsSubscribed;
        }

      

    }
}