using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;
using System.Threading.Tasks;
using UnityEngine.UI;
using OneSignalPush.MiniJSON;

namespace Hologo2
{
    /// <summary>
    /// Created by: Shariz - 16 October 2019
    /// Modified by: 
    /// view controller for Rate Us 
    /// </summary>

    public class OneSignalController : MonoBehaviour
    {


        [Header("SCRIPTS")]

        [SerializeField]
        private settings_SO MainSettings;
        [Header("Scene Objects")]
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;

        [Header("Data")]
        
        public int countReached;

        public Sprite notificationImage;

        private static bool requiresUserPrivacyConsent = false;

        private static string extraMessage;

        public static OneSignalController instance = null; //SHARIZ 29th Oct 2019 2.00

        //SHARIZ 29th Oct 2019 2.00
        void Awake()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy(gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);
        }

        public void Start (){
            if(MainSettings.pushNotificationOn==false){
                if(MainSettings.AppCount % countReached == 0) {
                    Invoke("ShowNotificationPrompt", 7);
                }
            }
            initiateOneSignalController();
            Debug.Log("started onesignal controller");
            extraMessage = null;
            
        }

        public void initiateOneSignalController()
        {
            initialize();
        }


        public void initialize()
        {
            // Enable line below to debug issues with setuping OneSignal. (logLevel, visualLogLevel)    
            OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.VERBOSE, OneSignal.LOG_LEVEL.NONE);
            OneSignal.SetRequiresUserPrivacyConsent(requiresUserPrivacyConsent);

            OneSignal.StartInit("8c8ff134-eda6-4406-b9df-2cd6b4addc59")
            .HandleNotificationReceived(HandleNotificationReceived)
            .HandleNotificationOpened(HandleNotificationOpened)
                 .Settings(new Dictionary<string, bool>() {
                    { OneSignal.kOSSettingsAutoPrompt, false } })
            .EndInit();

            OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

            OneSignal.SendTag("Hologo2", "Yes");
            OneSignal.SendTag("Country", MainSettings.country.ToString());
            OneSignal.SendTag("Language", MainSettings.language.ToString());

            var status = OneSignal.GetPermissionSubscriptionState();
            var notifpermission = status.permissionStatus.hasPrompted;
            var notifsubscription = status.subscriptionStatus.subscribed;

            OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
            OneSignal.permissionObserver += OneSignal_permissionObserver;
            OneSignal.subscriptionObserver += OneSignal_subscriptionObserver;

            var pushState = OneSignal.GetPermissionSubscriptionState();

            

            // if(MainSettings.appRated == true){
            // }
            // else {
            //     if(MainSettings.AppCount % countReached == 0) {
            //         openNotificationRequestWindow();
            //     }
            // }
        }

        private void OneSignal_subscriptionObserver(OSSubscriptionStateChanges stateChanges) {
            Debug.Log("SUBSCRIPTION stateChanges: " + stateChanges);
            Debug.Log("SUBSCRIPTION stateChanges.to.userId: " + stateChanges.to.userId);
            Debug.Log("SUBSCRIPTION stateChanges.to.subscribed: " + stateChanges.to.subscribed);
        }

        private void OneSignal_permissionObserver(OSPermissionStateChanges stateChanges) {
            Debug.Log("PERMISSION stateChanges.from.status: " + stateChanges.from.status);
            Debug.Log("PERMISSION stateChanges.to.status: " + stateChanges.to.status);
        }

        public void updateNotificationTags(){
            OneSignal.SendTag("Country", MainSettings.country.ToString());
            OneSignal.SendTag("Language", MainSettings.language.ToString());
        }

        // Called when your app is in focus and a notificaiton is recieved.
        // The name of the method can be anything as long as the signature matches.
        // Method must be static or this object should be marked as DontDestroyOnLoad
        private static void HandleNotificationReceived(OSNotification notification) {
            OSNotificationPayload payload = notification.payload;
            string message = payload.body;

            print("GameControllerExample:HandleNotificationReceived: " + message);
            print("displayType: " + notification.displayType);
            extraMessage = "Notification received with text: " + message;

        Dictionary<string, object> additionalData = payload.additionalData;
        if (additionalData == null) 
            Debug.Log ("[HandleNotificationReceived] Additional Data == null");
        else
            Debug.Log("[HandleNotificationReceived] message "+ message +", additionalData: "+ Json.Serialize(additionalData) as string);
        }

        

        // Gets called when the player opens the notification.
        private static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            iOS_UIModalWindowMain windowMain = iOS_UIModalWindowMain.Instance();

            Debug.Log("HandleNotificationOpened");
            OSNotificationPayload payload = result.notification.payload;
            string message = payload.body;
            Debug.Log(message);
            windowMain.InfomationToast.ShowToast(message, 0);
        }


    public void DelayedNotify()
    {
        Debug.Log("Delayed notify");
        //Shariz show notification prompt on 2nd visit
        // if (!HologoMAIN.Singleton.AppFirstTime)
        // {
            var status = OneSignal.GetPermissionSubscriptionState();
            var notifpermission = status.permissionStatus.hasPrompted;
            var notifsubscription = status.subscriptionStatus.subscribed;
           Debug.Log("permission status is" + notifpermission);
           Debug.Log("subscription status is" + notifsubscription);
            OneSignal.GetPermissionSubscriptionState();
            if (notifpermission == false)
            {
                Invoke("ShowNotificationPrompt", 7);
                //yield return StartCoroutine(ShowNotificationPrompt());
            }
            else
            {
                if (notifsubscription == false)
                {
                    string toastMessage = localizationProvider.provide.getLanguage().getALabelText(92);// Shariz 22nd Feb 2020 2.0.4
                    windowMain.InfomationToast.ShowToast(toastMessage, 3f);
                }
                else
                {
                    string toastMessage = localizationProvider.provide.getLanguage().getALabelText(93);// Shariz 22nd Feb 2020 2.0.4
                    windowMain.InfomationToast.ShowToast(toastMessage, 3f);
                }


            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
            }
        // }
        //end shariz

        //trying to make notification prompt Shariz
    }


    void ShowNotificationPrompt() // Shariz 18th March 2020 v2.0.4
    {
        string title = localizationProvider.provide.getLanguage().getALabelText(97);
        // string title = "Enable push notifications";
        string description = localizationProvider.provide.getLanguage().getALabelText(98);
        // string description = "Would you like to get notified when we \n add new experiences, features, and updates?";
        Debug.Log("show notification prompt");
        iOS_UIModalWindowMain windowMain = iOS_UIModalWindowMain.Instance();
        windowMain.ModelWindowB.ChoiceMaker(true,iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", notificationImage, EnableAndSubscribeforPush);// Shariz 29th Oct 2019 2.00
        // windowMain.ModelWindowG.ChoiceMaker(iOS_ModalWindowG.ButtonOptions.CancelOk, false, "Would you like to get notified \n when we add new experiences, features, and updates ?",
        // "click Yes to get notifications", "", () => EnableAndSubscribeforPush());

    }

    void EnableAndSubscribeforPush()
    {
        MainSettings.pushNotificationOn = true;
        Debug.Log("enable and subscribe for push");
        OneSignal.RegisterForPushNotifications();
        OneSignal.SetSubscription(true);
    }

    string showNotificationStatus()
    {

        var status = OneSignal.GetPermissionSubscriptionState();
        var notifpermission = status.permissionStatus.hasPrompted;
        var notifsubscription = status.subscriptionStatus.subscribed;

        string t = " permission accepted : " + notifpermission;
        string k = " subscribed : " + notifsubscription;

        string o = "" + t + "\n" + k ;
        return o;
    }

    public void TestNotification(){
        ShowNotificationPrompt();
    }

   
    }

}