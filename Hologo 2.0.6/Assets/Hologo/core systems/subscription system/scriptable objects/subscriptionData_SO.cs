using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 september 2019
    /// Modified by: 
    /// this script controlls the library view and its functions
    /// </summary>
    [CreateAssetMenu(fileName = "subscriptionsData.asset", menuName = "Hologo V2/new subscriptions data")]
    public class subscriptionData_SO : ScriptableObject
    {
        [SerializeField]
        private List<SubscriptionDataClass> subscriptionDataList;

        [SerializeField]
        private SubscriptionDetails userSubscriptionDetails;


        public void setSubscriptionsData(List<SubscriptionDataClass> data)
        {
            subscriptionDataList = data;
        }

        public List<SubscriptionDataClass> getSubscriptionsData()
        {
            return subscriptionDataList;
        }


        public void setSubscriptionsDetails(SubscriptionDetails data)
        {
            userSubscriptionDetails = data;
        }

        public SubscriptionDetails getSubscriptonsDetails()
        {
            return userSubscriptionDetails;
        }

        public void flushData()
        {
            subscriptionDataList.Clear();
            userSubscriptionDetails = new SubscriptionDetails();
        }

    }
}
