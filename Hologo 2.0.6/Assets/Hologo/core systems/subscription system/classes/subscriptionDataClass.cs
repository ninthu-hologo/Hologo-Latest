using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class manages everthing to do with the library
    /// </summary>
    /// 
    [Serializable]
    public class SubscriptionDataList
    {
        public List<SubscriptionDataClass> subscriptionList;
    }

    [Serializable]
    public class SubscriptionDataClass : IdataObject, IEquatable<SubscriptionDataClass>
    {
        public int id;
        public string name;
        public string description;
        public double price;
        public int duration;
        public string user_type;
        public string order_date;
        public string expire_date;
        public int subject_id;
        public int max_lessons; // hamid oct 26th 2019 2.00
        public string PI;
        public string status;
        public int order_id;
        public int premium;

        public bool isBought;

        public bool Equals(SubscriptionDataClass other)
        {
            if (other == null)
                return false;
            if (this.name.Equals(other.name, StringComparison.Ordinal))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

   

    [Serializable]
    public class SubscriptionDetails : IdataObject
    {
        public int id;
        public int user_id;
        public string name; 
        public string description;
        public int subscription_package_id;
        public double amount;
        public int duration;
        public int subject_id;
        public string start_at;
        public string expired_at;
        public string user_type;
        public int max_lessons;
        public int max_coupons;
        public int lesson_count;
        public SubscriptionDataClass subscription_package;
    }

    [Serializable]
    public class Subscription_Stats
    {
        public int total_lessons;
        public int used_lessons;
        public int total_coupons;
        public int used_coupons;

    }


}
