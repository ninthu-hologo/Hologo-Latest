using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2
{

    [Serializable]
    public class StudentInventory
    {
        public List<StudentCouponClass> StudentList;
    }

    [Serializable]
    public class StudentCouponClass : IdataObject
    {
        public int id;
        public string code;
        public int subject_id;
        public StudentDetails student;

    }


    [Serializable]
    public class StudentDetails
    {
        public int id;
        public string identifier;
        public string name;
    }



    [Serializable]
    public class followerDataClass
    {
        public int id;
        public int requester_id;
        public int acceptor_id;
        public bool status;
        public UserDetail requester;
    }

}