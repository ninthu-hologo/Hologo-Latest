using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Hologo2
{
    // for storing teacher details
    [Serializable]
    public class teacherDetail : IdataObject
    {
        public int id;
        public string identifier;
        public string subject_name;
    }

}