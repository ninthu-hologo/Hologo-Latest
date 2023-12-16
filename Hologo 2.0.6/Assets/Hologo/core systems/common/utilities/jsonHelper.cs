using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2
{
    public static class jsonHelper
    {
        // Generic Json Deserializer. returns deserialized object and success status
        public static GenericObjectAndStatus<T> DeserializeFromJson<T>(string data)
        {
            T myobject;
            GenericObjectAndStatus<T> gos = new GenericObjectAndStatus<T>();

            try
            {
                Debug.Log("success from jsonhelper");
                myobject = JsonUtility.FromJson<T>(data);
                // Debug.Log(myobject);
                gos.Success = true;
                gos.GenericObject = myobject;

            }
            catch (Exception)
            {
                gos.Success = false;
            }

            return gos;
        }

        // deserializing a json into a scriptable object
        public static bool DeserializeJsonToScriptableObject(string data, ScriptableObject so)
        {
            bool success = false;

            try
            {
                JsonUtility.FromJsonOverwrite(data, so);
                success = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                success = false;
            }
            return success;
        }



        /// <summary>
        /// shuffle list extension method located in jsonHelper script
        /// // need to move to someplace else
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}