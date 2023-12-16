using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class gets manages subscriptions
    /// </summary>
    public class subscriptionManager : messageLogging
    {

        [SerializeField]
        private dataPaths_SO dataPath;
        [SerializeField]
        private userData_SO userData;
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private subscriptionData_SO subscriptionData;

        [SerializeField]
        private List<SubscriptionDataClass> subscriptionDataList;

        [SerializeField]
        private SubscriptionDetails userSubscriptionDetails;


        //com.prodesigners.hologo.teacher99
        //com.prodesigners.hologo.student99
        public async Task<bool> loadSubscriptions()
        {


            bool success = false;

            if (subscriptionDataList != null && subscriptionDataList.Count > 0)
                return true;

            subscriptionsDataConnect sdc = new subscriptionsDataConnect();

            var task = sdc.loadSubscriptionsFromServer(dataPath.getUrl(0)+ mainSettings.language.id);

            string result;

            try
            {
                result = await task;

                GenericObjectAndStatus<HologoWebAPIGeneric<List<SubscriptionDataClass>>> subscriptionAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<SubscriptionDataClass>>>(result);
                Debug.Log(result);
                if (!subscriptionAndStatus.GenericObject.success)
                {
                    createMessage("error");
                    return false;
                }

                success = true;
                subscriptionDataList = subscriptionAndStatus.GenericObject.data;
                subscriptionData.setSubscriptionsData(subscriptionDataList);

            }
            catch (HologoInternetException ex)
            {
                success = false;
                createMessage(ex.Message);
            }

            return success;
        }

        public List<string> getSubcriptionPIs()
        {
            List<string> pi = new List<string>();
            for (int i = 0; i < subscriptionData.getSubscriptionsData().Count; i++)
            {
                pi.Add(subscriptionData.getSubscriptionsData()[i].PI);
            }

            return pi;
        }

        public SubscriptionDataClass getSubscriptionByType(string type)
        {
            for (int i = 0; i < subscriptionData.getSubscriptionsData().Count; i++)
            {
                if (string.Equals(type, subscriptionData.getSubscriptionsData()[i].user_type, System.StringComparison.Ordinal))
                {
                    return subscriptionData.getSubscriptionsData()[i];
                }
            }
            return null;
        }

        // Shariz 25th March 2020 v2.0.4 adding free trial offer
        public int getFreeTrialSubscriptionByType(string type)
        {
            Debug.Log("Getting free trial by type for "+type);
            if(type=="teacher"){
                return 6;
            } else {
                return 5;
            }
            
        }

        

        //public async Task<bool> getSubscriptionDetails(int productable_id)
        //{
        //    bool success = false;

        //    if (userSubscriptionDetails != null)
        //        return true;

        //    subscriptionsDataConnect sdc = new subscriptionsDataConnect();

        //    // need to include the token in the url ..
        //    //TODO::
        //    var task = sdc.getUserSubscriptionStatus(dataPath.getUrl(0));

        //    string result;

        //    try
        //    {
        //        result = await task;

        //        GenericObjectAndStatus<HologoWebAPIGeneric<SubscriptionDetails>> subDetailsAndStatus =
        //        jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<SubscriptionDetails>>(result);

        //        if (!subDetailsAndStatus.GenericObject.success)
        //        {
        //            createMessage("error");
        //            return false;
        //        }

        //        success = true;
        //        userSubscriptionDetails = subDetailsAndStatus.GenericObject.data;
        //    }
        //    catch (HologoInternetException ex)
        //    {
        //        success = false;
        //        createMessage(ex.Message);
        //    }

        //    return success;
        //}

        // buy subscription
        //TODO : implement this
        public bool buySubscription(SubscriptionDataClass subscription)
        {
            return true;
        }

        
        public async Task<bool> checkSubscriptionIsValid()
        {
            string result;
            bool success = false;
            subscriptionsDataConnect sdc = new subscriptionsDataConnect();
            string url = dataPath.getUrl(1) + mainSettings.language.id + "&token=" + userData.requestMyToken(); // Shariz 1st April 2020 v2.0.5 trying to get localized my subscriptions
            Debug.Log(url);
            var task = sdc.getUserSubscriptionStatus(url);
            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoWebAPIGeneric<SubscriptionDetails>> subDetailsAndStatus =
               jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<SubscriptionDetails>>(result);

                Debug.Log(result);
                if(subDetailsAndStatus.GenericObject.success)
                {
                    userSubscriptionDetails = subDetailsAndStatus.GenericObject.data;
                    userSubscriptionDetails.subscription_package.isBought = true;
                    subscriptionData.setSubscriptionsDetails(userSubscriptionDetails);
                    // ONLY PLACE WHERE USER GETS SUBSCRIBED TO TRUE
                    userData.setUserAsSubscribed();
                    success = true;
                }
                // hamid 2.0.2 26th Nov 2019
                else { 
                    userData.userIsNotSubscribed();
                }

            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }

            return success;
        
        }

        

        //public List<SubscriptionDataClass> getListOfAvailableSubscriptions(int usertype = 0)
        //{
        //    if(usertype >0)
        //    {
        //        return filterListToUserType(usertype);
        //    }

        //    return subscriptionDataList;
        //}


        //List<SubscriptionDataClass> filterListToUserType(int type)
        //{
        //    List<SubscriptionDataClass> tempList = new List<SubscriptionDataClass>();

        //    for (int i = 0; i < subscriptionDataList.Count; i++)
        //    {
        //        if(subscriptionDataList[i].user_type_id == type)
        //        {
        //            tempList.Add(subscriptionDataList[i]);
        //        }
        //    }

        //    return tempList;
        //}

        public SubscriptionDetails getSubscriptionDetails()
        {
            return subscriptionData.getSubscriptonsDetails();
        }

        #region EDITOR FUNCS
        public async void loadSubscriptionsEditorOnly()
        {
            await loadSubscriptions();
        }
        #endregion

    }
}
