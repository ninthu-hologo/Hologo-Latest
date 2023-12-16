using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Hologo2
{

    [Serializable]
    public class HologoWebAPIGeneric<T>
    {
        public bool success;
        public T data;
        public string message;
    }


    // class container for deserialized json object and success status
    /// <summary>
    /// Generic object and status. a class that holds server messages
    /// \n bool Success : success status
    /// \n GenericObject : hold the actual json string. where you can pass in another class that same as the json.
    /// </summary>
    public class GenericObjectAndStatus<T>
    {
        public bool Success;
        public T GenericObject;
    }


    [Serializable]
    public class FromDiskOrInternet
    {
        public bool IsInternet;
        public string data;
    }

    [Serializable]
    public class HoloApiSuccessOnly
    {
        public bool success;
    }


    [Serializable]
    public class HologoAPIProductBuy
    {
        public bool success;
        public HologoOrder data;
        // public HologoItem items;
        public string message;
    }
    [Serializable]
    public class HologoOrder
    {
        public int id;
        public int profileable_id;
        public string status;

    }
    [Serializable]
    public class HologoItem
    {
        public int id;
    }


    [Serializable]
    public class ServerStatus
    {
        public bool status;
    }


    [Serializable]
    public class HologoAPI
    {
        public bool success;
        //public string data;
        public string message;
    }


    [Serializable]
    public class HologoUserAuth
    {
        public string message;
        public bool error;
    }

    // Shariz 26th May 2020 v2.0.6 Genie Payment Method
    [Serializable]
    public class HologoPaymentRecord
    {
        public bool success;
        public HologoPaymentRecordData data;
        public string message;
    }

    [Serializable]
    public class HologoPaymentRecordData
    {
        public int id;
    }


}