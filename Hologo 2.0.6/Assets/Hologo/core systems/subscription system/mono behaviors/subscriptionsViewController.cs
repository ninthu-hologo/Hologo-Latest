using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;
using Hologo.iOSUI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 12 july 2019
    /// Modified by: 
    /// this script controlls the library view and its functions
    /// </summary>
    public class subscriptionsViewController : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private userData_SO userData;
        [SerializeField]
        private settings_SO mainSettings;
        [Header("SCRIPTS")]
        [SerializeField]
        private subscriptionManager subManager;
        [SerializeField]
        private userViewController uVController;
        [SerializeField]
        private settingsUIConnect sConnect;
        //[SerializeField]
        //private paymentManager pManager;
        [Header("Scene Objects")]
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;
        [SerializeField]
        private userUI userUI;

        [Header("EVENTS")]
        [SerializeField]
        private event_SO goToLibrary;// Shariz 27th Oct 2019 2.00

        

        [SerializeField]
        private bool freeTrialOffer; //Shariz 25th March 2020 2.0.4

        [SerializeField]
        private int paymentMethod; //Shariz 26th May 2020 2.0.6

        [SerializeField]
        private int paymentMethodTransID; //Shariz 26th May 2020 2.0.6 


        public void initiateSubscriptonsViewController()
        {
            syncGetSubscriptionsData();
        }



        async void syncGetSubscriptionsData()
        {
            await getSubscriptionsData();
        }

        public async Task asyncGetSubscriptionsState()
        {
            await getSubscriptionsData();
            await checkForValidSubscription(this.gameObject);
        }


        async Task getSubscriptionsData()
        {
            // get subscriptions now
            bool success = await subManager.loadSubscriptions();
            if (success)
            {
                // payment initialization
                //pManager.startPaymentInitialization(subManager.getSubcriptionPIs());
                //// also we can show another store depending on country

                ////Shariz 26th May 202 toggling free trial off 2.0.6 and updating for multiple payment methods
                //buyingFreeTrial(false, 0);
            }
            else
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(51);
                windowMain.InfomationToast.ShowToast(message, 3f);
            }
        }
         

        public void checkSubscriptionStatusEvent(GameObject go)
        {
            checkForSubs(go);
        }

        async void checkForSubs(GameObject go)
        {
            await checkForValidSubscription(go);
        }

        async Task checkForValidSubscription(GameObject go)
        {
            // userViewController uVController = go.GetComponent<userViewController>();
            bool success = await subManager.checkSubscriptionIsValid();
            Debug.Log("enabling subs");
            sConnect.EnableSubscriptions();
            if (success)
            {
                Debug.Log("has a valid subscription");
                // ONLY PLACE WHERE USER GETS SUBSCRIBED TO TRUE
                SubscriptionDetails sd = subManager.getSubscriptionDetails();
                // userData.setUserAsSubscribed();

                // here we can set subscription details;
                string description = sd.subscription_package.description;
                string title = sd.subscription_package.name;
                string perYear = localizationProvider.provide.getLanguage().getALabelText(7);// Shariz 22nd Feb 2020 2.0.4
                string price = "<font=English>USD</font>" + string.Format("{0:#.00}", sd.subscription_package.price) + perYear;//shariz 31st oct 2019 2.00 // Shariz 22nd Feb 2020 2.0.4

                
                //shariz 27th oct 2019 2.00
                string lessonsCount = sd.lesson_count.ToString() + "<font=English>/</font>" + sd.subscription_package.max_lessons; // Shariz 27 April 2020 2.0.5
                string expiryDate = sd.expired_at;

                userUI.setSubscriptionUI(true, userData.getUserType(), description, title, price, lessonsCount, expiryDate, sd.subscription_package.id);//shariz 25th March 2020 2.0.4
                // all is well. can show cancel showing sub intro ui
                // and go to library or settings
            }
            else
            {
                Debug.Log("has no valid subscription");
                if (userData.getUserType() == 1)
                {
                    selectTeacher();
                }
                else
                {
                    selectStudent();
                }



            }
        }



        // Shariz oct 27th hologo 2.00 new methods for checking subs when signing in
        /* 
         *  Checking subscription for sign in
         * 
         * */
        public void checkSubscriptionStatusEventSignIn(GameObject go)
        {
            checkForSubsSignIn(go);
        }

        async void checkForSubsSignIn(GameObject go)
        {
            await checkForValidSubscriptionSignIn(go);
        }

        async Task checkForValidSubscriptionSignIn(GameObject go)
        {
            // userViewController uVController = go.GetComponent<userViewController>();
            bool success = await subManager.checkSubscriptionIsValid();
            Debug.Log("enabling subs");
            sConnect.EnableSubscriptions();
            if (success)
            {
                Debug.Log("has a valid subscription and just signed in");
                // ONLY PLACE WHERE USER GETS SUBSCRIBED TO TRUE
                SubscriptionDetails sd = subManager.getSubscriptionDetails();
                // userData.setUserAsSubscribed();

                // here we can set subscription details;
                string description = sd.subscription_package.description;
                string title = sd.subscription_package.name;
                string perYear = localizationProvider.provide.getLanguage().getALabelText(7);// Shariz 22nd Feb 2020 2.0.4
                string price = "<font=English>USD</font>" + string.Format("{0:#.00}", sd.subscription_package.price) + perYear;//shariz 31st oct 2019 2.00 // Shariz 22nd Feb 2020 2.0.4

                // userUI.setSubscriptionUI(true, userData.getUserType(), description, title, price); // Shariz 27th Oct 2019 2.00
                goToLibrary.raise(this.gameObject); // Shariz 27th Oct 2019 2.00

                // all is well. can show cancel showing sub intro ui
                // and go to library or settings
            }
            else
            {
                Debug.Log("has no valid subscription");
                if (userData.getUserType() == 1)
                {
                    selectTeacher();
                }
                else
                {
                    selectStudent();
                }



            }
        }

        /* 
         *  End Checking subscription for sign in
         * 
         * */



        public void selectStudent()
        {
            if (userData.isUserSignedIn() && userData.getUserType() == 1)
            {
                selectTeacher();
                return;
            }

            SubscriptionDataClass sd = subManager.getSubscriptionByType("student");
            string description = sd.description;
            string title = sd.name;
            string perYear = localizationProvider.provide.getLanguage().getALabelText(7);// Shariz 22nd Feb 2020 2.0.4
            string price = "<font=English>USD</font>" + string.Format("{0:#.00}", sd.price) + perYear;//shariz 31st oct 2019 2.00 // Shariz 22nd Feb 2020 2.0.4
            // show sub intro

            //shariz 27th oct 2019 2.00
            string lessonsCount = "";
            string expiryDate = "";
            userUI.setSubscriptionUI(false, 2, description, title, price, lessonsCount, expiryDate, sd.id);//shariz 25th March 2020 2.0.4
        }

        public void selectTeacher()
        {
            if (userData.isUserSignedIn() && userData.getUserType() == 2)
            {
                selectStudent();
                return;
            }


            SubscriptionDataClass sd = subManager.getSubscriptionByType("teacher");
            string description = sd.description;
            string title = sd.name;
            string perYear = localizationProvider.provide.getLanguage().getALabelText(7);// Shariz 22nd Feb 2020 2.0.4
            string price = "<font=English>USD</font>" + string.Format("{0:#.00}", sd.price) + perYear;//shariz 31st oct 2019 2.00 // Shariz 22nd Feb 2020 2.0.4
            // show sub intro

            //shariz 27th oct 2019 2.00
            string lessonsCount = "";
            string expiryDate = "";
            userUI.setSubscriptionUI(false, 1, description, title, price, lessonsCount, expiryDate, sd.id);//shariz 25th March 2020 2.0.4
        }

        //Shariz 26th May 2020 2.0.6 Adding multiple payment methods
        public void buyingFreeTrial(bool isTrue, int paymentInt){
            Debug.Log("is buyinf Free Trial? "+isTrue);
            freeTrialOffer = isTrue;
            paymentMethod = paymentInt;
            uVController.goBuyOrSignUpForSubscription(); // This should be removed when free trial is done
        }

        // Shariz 26th May 2020 v2.0.6 adding multiple payment methods
        public void buySubscriptionEvent(GameObject go)
        {
            Debug.Log("item bought");
            // buy subscription start here.
            // this will be called from user after sign up or sign in.

            SubscriptionDataClass sd;
            sd = subManager.getSubscriptionByType(userUI.getUserTypeSelection());
            // Shariz 19th March 2020 v2.0.4
            //WARNING ITS IMPORTANT THAT THIS ID EQUALS TO SRI LANKA COUNTRY ID
            // if (mainSettings.country.id == 4)
            // {
            //     // open web view for dialogue payment.
            //     string title = localizationProvider.provide.getLanguage().getALabelText(95);// Shariz 22nd Feb 2020 2.0.4
            //     string message = localizationProvider.provide.getLanguage().getALabelText(96);// Shariz 22nd Feb 2020 2.0.4
            //     string ok = localizationProvider.provide.getLanguage().getAButtonText(0);// Shariz 22nd Feb 2020 2.0.4
            //     windowMain.ModalWindowD.ShowInfo(true, true, title, message, ok, StartDialoguePaymentProcess);// Shariz 22nd Feb 2020 2.0.4
            // }
            // else
            // {
                sd = subManager.getSubscriptionByType(userUI.getUserTypeSelection());

                if (mainSettings.country.id == 4 && freeTrialOffer)
                {
                //    int freeSubID = subManager.getFreeTrialSubscriptionByType(userUI.getUserTypeSelection());
                //    Debug.Log("got free trial by type and id is "+freeSubID);
                //    StartCoroutine(pManager.FreeTrialSubscriptionTransacrionValidation(freeSubID));
                //} else if (mainSettings.country.id == 4 && paymentMethod == 1) {
                //    StartCoroutine(pManager.PayByGenieMethod());
                //} else
                //{
                //    pManager.BuySubscription(sd.PI, sd);
                }

            // pManager.BuySubscription(sd.PI, sd);

            // }

        }

        

        // Shariz adding multiple payment methods 2.0.6 26th May 2020
        public void openPaymentMethods()
        {
            if (mainSettings.country.id == 4)
            {
                string title = "Payment Method";
                string description = "Choose your preferred payment method for your annual subscription.";

                string titleOne = "Buy with Apple Pay";

                if (mainSettings.osplatform == OSPlatform.ANDROID)
                {
                    titleOne = "Buy with Google Pay";
                }
                string titleTwo = "Pay by Genie";
                string bodyOne = "$9.99 / year";  // Shariz changing subscription price 2.0.6 26th June 2020
                string bodyTwo = "LKR 1999 / year";  // Shariz changing subscription price 2.0.6 26th June 2020

                Sprite imageTwo = userUI.payByGenieSprite;

                windowMain.ModalWindowC.ChoiceMaker(title, description, titleOne, titleTwo, "", false, true, false, bodyOne,
                    bodyTwo, null, null, imageTwo, null, payBuyDevice, payByGenie, null, null);
            } else
            {
                buyingFreeTrial(false, 0);
            }
        }



        // Shariz adding multiple payment methods 2.0.6 26th May 2020
        void payBuyDevice()
        {
            buyingFreeTrial(false, 0);
            Debug.Log("Pay by Device");
        }
        void payByGenie()
        {
            buyingFreeTrial(false, 1);
            
            Debug.Log("Pay by Genie");
        }



        //TODO implement once we get the dialogue payments implemented
        public void StartDialoguePaymentProcess()
        {
            Debug.Log("dialogue payment is being made now");
        }

    }
}
