using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class is a date object for servers incoming attachables
    /// </summary>
    [System.Serializable]
    public class attachablesClass:  IEquatable<attachablesClass>
    {
		public string title;
		public int id;
        public string file_name;
        public uint crc;
        public int attachable_id;
        public float size;
        public uint version;
        public string updated_at;
        public string platform;

        public bool Equals(attachablesClass other)
        {

            if (this.id == other.id)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// will return true if other is newer
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool versionCheck(attachablesClass other)
        {
            if(other.version > version)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
