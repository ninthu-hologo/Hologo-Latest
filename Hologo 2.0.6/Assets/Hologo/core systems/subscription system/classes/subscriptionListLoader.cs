using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class gets subscriptions from server only
    /// </summary>
    public class subscriptionsDataConnect
    {
        public async Task<string> loadSubscriptionsFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }


        public async Task<string> getUserSubscriptionStatus(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> getUserSubscriptionDetails(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        public async Task<string> validateSubscrition(string url,int id, string rp)
        {
            WWWForm signUpForm = new WWWForm();
            signUpForm.AddField("subscription_package_id", id);
            signUpForm.AddField("payload", rp);
            url = System.Uri.EscapeUriString(url);
            return await InternetfetchData.submitFormToServer(url, signUpForm);
        }

    }
}