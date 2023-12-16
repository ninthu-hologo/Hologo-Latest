using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2
{

    [Serializable]
    public class UserData
    {
        public string token;
        public UserDetail user;
        public UserProfile profile;
    }

    [Serializable]
    public class UserDetail
    {
        public string name;
        public string email;
        public string language_id;
        public string country_id;
        public string user_type;
        public string current_login_device;
        public string contact_number;
        public int verified;
        public int id;

        public string password;

        //app only fields
        public bool IsSignedIn;
        public bool IsSubscribed;
        public int user_type_id;
    }

    [Serializable]
    public class UserProfile
    {
        public int id;
        public int user_id;
        public string full_name;
        public string address;
        public string mobile_number;
        public string company;
    }

    // class to hold the user authentication from json
    [Serializable]
    public class UserDataAuth
    {
        public string token;
        public UserDetail user;
    }

    [Serializable]
    public class HologoUserAuthAndData
    {
        public string message;
        public UserDataAuth data;
        public bool success;
    }

    // class to hold the user authentication from json
    [Serializable]
    public class UserAuthenticate
    {
        public UserDataAuth data;
    }
}
