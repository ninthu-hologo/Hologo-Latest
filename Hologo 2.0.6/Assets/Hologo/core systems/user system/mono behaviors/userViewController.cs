using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hologo.iOSUI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// user view controller
    /// </summary>
    public class userViewController : MonoBehaviour
    {

        [Header("UI ELEMENTS")]
        [SerializeField]
        private settingsUIConnect settingsUIConnect;
        [SerializeField]
        private settingsSwitcher settingsUI;
        [SerializeField]
        private userUI userUI;
        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private userData_SO userData;
        [Header("THIS SHOULD NOT BE DONE")]
        [SerializeField]
        private lessonsList_SO lesson;
        [SerializeField]
        private studentLessons_SO studentLessons;
        [Header("Scripts")]
        [SerializeField]
        private userManager uManager;
        [SerializeField]
        private profileManager pManager;
        [SerializeField]
        private quizManager qManager;
        [SerializeField]
        private iOS_UIModalWindowMain modalWindow;
        [SerializeField]
        private directoryDeleterCreator direct;
        [Header("Events")]
        [SerializeField]
        private event_SO checkSubscriptionEvent;
        [SerializeField]
        private event_SO checkSubscriptionEventSignIn; // Shariz 27th Oct 2019 2.00
        [SerializeField]
        private event_SO buySubscriptionEvent;
        [SerializeField]
        private event_SO goToLibrary;
        //shariz removing subscriptions
        [SerializeField]
        private bottomBarNavigation bbNavigation;



        public void getProfile()
        {
            Debug.Log("get profile");
            getAsyncProfile();
            settingsUIConnect.populateProfile(userData.requestUserDetail().name, userData.requestUserDetail().contact_number, userData.requestUserDetail().email);
        }


        public void updateProfile()
        {
            if(string.IsNullOrEmpty(settingsUI.getSettingsUI().readName()))
            {
                    string message = localizationProvider.provide.getLanguage().getAMessageText(20);
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, message, ok);
                return;
            }
            else
            {
                asyncUpdateProfile();
            }
        }


        public void showUserNameAndType()
        {
            settingsUIConnect.showUserNameAndType(userData.requestUserDetail().name, userData.requestUserDetail().user_type);
        }


        public void SignInUser()
        {
            bool doesAnotherUserExist = uManager.doesAUserAlreadyExist(userUI.readSignInEmail());
            if (doesAnotherUserExist)
            {
                string title = localizationProvider.provide.getLanguage().getAMessageText(58);
                string description = localizationProvider.provide.getLanguage().getAMessageText(60);
                modalWindow.ModelWindowB.ChoiceMaker(false,iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", null,signInFromWindow);// Shariz 29th Oct 2019 2.00
                return;
            }

            asyncSignInUser();
        }

        void signInFromWindow()
        {
            direct.deleteFilesInDirectories();
            uManager.deleteUserDataFile();
            qManager.deleteScoresData();
            asyncSignInUser();
        }

        public void SignUpUser()
        {
            bool doesAnotherUserExist = uManager.doesAUserAlreadyExist(userUI.readSignUpEmail());
            if (doesAnotherUserExist)
            {
                string title = localizationProvider.provide.getLanguage().getAMessageText(58);
                string description = localizationProvider.provide.getLanguage().getAMessageText(60);
                modalWindow.ModelWindowB.ChoiceMaker(false,iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", null, signUpFromWindow);// Shariz 29th Oct 2019 2.00
                return;
            }

            asyncSignUpUser();
        }

        // user overide delete things
        void signUpFromWindow()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            lesson.flushData();
            studentLessons.flushData();
            direct.deleteFilesInDirectories();
            uManager.deleteUserDataFile();
            qManager.deleteScoresData();
            asyncSignUpUser();
        }


        public void SignOut() // Shariz 27th Oct 2019 2.00
        {
            // after signing out/
            // clear token/password
            // send to libray
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
            string title = localizationProvider.provide.getLanguage().getAButtonText(27);
            string description = localizationProvider.provide.getLanguage().getALabelText(75);
            modalWindow.ModelWindowB.ChoiceMaker(false,iOS_ModalWindowB.ButtonOptions.CancelOk, title, description, "", "", null, ConfirmSignOut);// Shariz 29th Oct 2019 2.00
            
        }

        // Shariz 27th Oct 2019 2.00
        public void ConfirmSignOut(){
            uManager.SignOutUser();
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
            goToLibrary.raise(this.gameObject);
        }

        // shariz 29th Jan 2020 Hologo 2.0.35
        public void ForgotPasswordButton()
        {
            // string title = localizationProvider.provide.getLanguage().getAMessageText(52);
            // string description = localizationProvider.provide.getLanguage().getAMessageText(53);
            string email = localizationProvider.provide.getLanguage().getALabelText(2);
            string title = localizationProvider.provide.getLanguage().getAButtonText(22); // Shariz adding localization for forget password 19th August 2020 2.0.9
            string description = localizationProvider.provide.getLanguage().getALabelText(103); // Shariz adding localization for forget password 19th August 2020 2.0.9
            //string description = "Please enter your email address and you will receive an email with instructions to reset your password";
            modalWindow.ModalWindowE.getiOS_ModalWindowE().FillInput(true, title, description, email, forgotPasswordModal);
        }

        // shariz 29th Jan 2020 Hologo 2.0.35
        void forgotPasswordModal(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                Debug.Log("please enter an email address");
            }
            else
            {
                Debug.Log("send new password");
                forgotPassword(emailAddress);
            }
        }


        public void forgotPassword(string email)
        {
            asyncForgotPassword(email);
        }

        public void updatePassword()
        {
            asyncChangePassword();
        }

        async void asyncSignInUser()
        {
            // check for existing user.
           
           bool success = await uManager.logInUser(userUI.readSignInEmail(), userUI.readSignInPassword());
            if(!success)
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18);// Shariz 1st Nov 2019 2.00
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                //show modal window alert
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+uManager.readMessage()+"</font>", ok );// Shariz 22nd Feb 2020 2.0.4
                return;
            }

            checkForSubscription();
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            Debug.Log("sign in user > success");
            // can fire event to other scripts
        }

       

        async void asyncSignUpUser()
        {
            bool success = await uManager.createUser(userUI.readSignUpUserName(), userUI.readSignUpEmail(),
                userUI.readSignUpPassword(), userUI.getUserTypeSelection(), mainSettings.language.id, mainSettings.country.id, mainSettings.getPlatform(), false);

            Debug.Log("sign up error message:" +uManager.readMessage());
            if (!success)
            {
                //show modal window alert
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18);// Shariz 28th Oct 2019 2.00
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+uManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
                return;
            }
            Debug.Log("sign up user > success");
            // can fire event to other scripts

            success = await uManager.autoLogIn();

            if (success)
            {
                Debug.Log("user loging from signup succes and raise buy sub event");
                //shariz removing subscriptions
                //buySubscriptionEvent.raise(this.gameObject);
                settingsUIConnect.closeUserUI();
                bbNavigation.gotoSettings();
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }

            Debug.Log("autologin > success");
        }


        //Shariz 26th May 2020 2.0.6 Adding multiple payment methods
        //button event from subscription select;
        public void goBuyOrSignUpForSubscription()
        {
            if (userData.isUserSignedIn() && userData.isUserSubscribed())
                return;

            if (userData.isUserSignedIn())
            {
                // initiate buy
                //shariz removing subscriptions
                //buySubscriptionEvent.raise(this.gameObject);
                settingsUIConnect.closeUserUI();
                bbNavigation.gotoSettings();
            }
            else
            {
                Debug.Log("Sign up view");
                settingsUIConnect.SignUpView();
            }

        }

        private void checkForSubscription()
        {
            // check to see if subscribed
            checkSubscriptionEventSignIn.raise(this.gameObject); // Shariz 27th Oct 2019 2.00
        }

        async void asyncForgotPassword(string email)
        {
            string ok = localizationProvider.provide.getLanguage().getAButtonText(0);

            bool success = await uManager.forgotPassword(email);
            if (!success)
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18);// shariz 29th Jan 2020 Hologo 2.0.35
                //show modal window alert
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, uManager.readMessage(), ok);
                return;
            }
                    string message = localizationProvider.provide.getLanguage().getAbodyText(12);
                    string successTitle = localizationProvider.provide.getLanguage().getATitleText(19);// shariz 29th Jan 2020 Hologo 2.0.35

            modalWindow.ModalWindowD.ShowInfo(true, true, successTitle, message, ok);
        }

        async void asyncChangePassword()
        {
            string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18); // Shariz hologo 2.0.2 10th dec 2019
            string successTitle = localizationProvider.provide.getLanguage().getATitleText(19); // Shariz hologo 2.0.2 10th dec 2019
            string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
            string message = localizationProvider.provide.getLanguage().getATitleText(20);
            bool success = await uManager.updatePassword(settingsUI.getSettingsUI().readNewPassword(),settingsUI.getSettingsUI().readOldPassword());
            if (!success)
            {
                //show modal window alert
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+uManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                return;
            }


            Debug.Log("change password > success");
            // autologin with new password
            success = await uManager.autoLogIn();
            Debug.Log("autologin > success");
            modalWindow.ModalWindowD.ShowInfo(true, true, successTitle, message, ok);
            settingsUIConnect.clearPasswordFields(); //Shariz 29th March 2020 2.0.4 adding clear fields after password update
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }


        async void getAsyncProfile()
        {
            bool success = await pManager.getProfile();
        }

        async void asyncUpdateProfile()
        {
            bool success = await pManager.updateProfileToServer(settingsUI.getSettingsUI().readName(),
                settingsUI.getSettingsUI().readContactNumber(), mainSettings.language.id,
                mainSettings.country.id, mainSettings.getPlatform());

            Debug.Log(pManager.readMessage());
            if (!success)
            {
            string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
            string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                //show modal window alert
                modalWindow.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+uManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                return;
            }
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            string toastMessage = localizationProvider.provide.getLanguage().getALabelText(94);// Shariz 22nd Feb 2020 2.0.4
            modalWindow.InfomationToast.ShowToast(toastMessage, 2f);//shariz 31st oct 2019 2.00// Shariz 22nd Feb 2020 2.0.4
            Debug.Log("update user > success");
            // can fire event to other scripts
        }

    }
}
