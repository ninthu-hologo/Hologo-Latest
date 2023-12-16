using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 july 2019
    /// Modified by: 
    /// boots up user 
    /// </summary>
    public class userBootUp : messageLogging, IBootUp
    {
        [SerializeField]
        private userManager uManager;


        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;
        }

        public void runBootSequence()
        {
           // loadUserAndLogInUser();
        }
        public async Task runBootUp()
        {
            // try loading user from storage or make new one
            Debug.Log("booting user");
            uManager.loadUserFromStorage();
            Debug.Log("loading done");
            if (uManager.isUserSignedIn())
            {
                bootUpSuccess = await uManager.autoLogIn();
            }
            else
            {
                // means this is a new account or user is not signed in
                bootUpSuccess = false;
            }
        }
    }
}
