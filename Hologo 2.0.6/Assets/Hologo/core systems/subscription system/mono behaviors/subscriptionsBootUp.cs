using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 july 2019
    /// Modified by: 
    /// boots up subscription
    /// </summary>
    public class subscriptionsBootUp : messageLogging, IBootUp
    {

        [SerializeField]
        userData_SO userData;

        [SerializeField]
        subscriptionManager sManager;

        // hamid 2.0.2 26th Nov 2019
        [SerializeField]
        userManager uManager;

        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;
        }

        public void runBootSequence()
        {
          // InitSub();
        }

        public async Task runBootUp()
        {
            bootUpSuccess = await initSubscriptions();
        }


        async Task<bool> initSubscriptions()
        {
            bool success = false;
            success = await sManager.loadSubscriptions();
            
            // check to see if user is signed in.
            // if not return false because user cannot 
            // check subscriptions
            if(userData.isUserSignedIn())
            {
                success = await sManager.checkSubscriptionIsValid();
                uManager.saveUserToStorage(); // hamid 2.0.2 26th Nov 2019
            }
                
            // if user doesnt have a subscription no. 
            // point in checking
            if (!userData.isUserSubscribed())
                return false;
            // checking if user has a subscription
            
            

            return success;

        }
    }
}
