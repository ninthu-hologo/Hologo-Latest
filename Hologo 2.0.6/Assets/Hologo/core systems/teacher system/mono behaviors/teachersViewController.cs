using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 01 August 2019
    /// Modified by: 
    /// view controller for teachers 
    /// </summary>
    public class teachersViewController : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private userData_SO userData;
        [Header("SCRIPTS")]
        [SerializeField]
        private teachersManager tManager;
        [SerializeField]
        private settingsUIConnect sConnect;
        [Header("Scene Objects")]
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;

        private CallbackGameObject[] teacherActions = new CallbackGameObject[1];

        [SerializeField]
        private settingsSwitcher hasNoTeachers;// Shariz 28th Oct 2019 2.00


        public void initiateacherViewController()
        {
            // get my teachers
            // get my teachers pending list
            // generate ui for them
            initialize();
        }


        async void initialize()
        {
            await getTeachers();
            await getPendingTeachers();

            updateNoTeachers();// Shariz 28th Oct 2019 2.00

        }

        public void AddTeacher()
        {
            string title = localizationProvider.provide.getLanguage().getAMessageText(52);
            string description = localizationProvider.provide.getLanguage().getAMessageText(53);
            string email = localizationProvider.provide.getLanguage().getALabelText(2);
            windowMain.ModalWindowE.getiOS_ModalWindowE().FillInput(true, title, description, email, addTeacherFromModal);
        }



        void addTeacherFromModal(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                Debug.Log("please enter an email address");
            }
            else
            {
                Debug.Log("add teacher");
                addTeacher(emailAddress);
            }
        }

        async void addTeacher(string emailAddress)
        {
                    string successTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18); // Shariz 21st March 2020 2.0.4
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
            bool success = await tManager.addTeacher(emailAddress, userData.requestMyToken());
            if (success)
            {
                string successMessage = "Your request has been sent to the respective teacher whose classroom you would like to join.";// Shariz 29th March 2020 2.0.4
                windowMain.ModalWindowD.ShowInfo(true, true, successTitle, "<font=English>"+successMessage+"</font>", ok);// Shariz 29th March 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+tManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
        }


        async Task getTeachers()
        {
            bool success = await tManager.getTeachersFromServer(userData.requestMyToken(), userData.requestUserDetail().id);

        }

        async Task getPendingTeachers()
        {
            bool success = await tManager.getTeachersPending(userData.requestMyToken());
            sConnect.makeStudentUIButtons(true,tManager.getPendingTeachers(), pendingAction, normalAction);
        }

        void cancel()
        {

        }

        void acceptRequest(int id, GameObject go)
        {
            Debug.Log("accepted me " + id);
            acceptRequestAsync(id, go);
        }

        async void acceptRequestAsync(int id, GameObject go)
        {
            bool success = await tManager.AcceptTeacher(id, userData.requestMyToken());
            if (success)
            {
                followersUIElement fuiE = go.GetComponent<followersUIElement>();
                fuiE.acceptedStateOn();
            }
            else
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+tManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
            }
        }


        void rejectRequest(int id, GameObject go)
        {
            Debug.Log("rejected me " + id);
            rejectRequestAsync(id, go);
        }

        async void rejectRequestAsync(int id, GameObject go)
        {
            bool success = await tManager.removeRejectTeacher(id, userData.requestMyToken());
            if (success)
            {
                sConnect.removeUIElement(go);

            }
            else
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+tManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
            }
        }


        void pendingAction(GameObject go)
        {
            followersUIElement fuiE = go.GetComponent<followersUIElement>();
            followerDataClass fdc = fuiE.getFollower();
            // Debug.Log("clicked me " + fdc.requester_id);
            string reject_invitation = localizationProvider.provide.getLanguage().getAMessageText(48);
            string accept_invitation = localizationProvider.provide.getLanguage().getAMessageText(49);
            windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, reject_invitation, accept_invitation, "",
                cancel, null, () => rejectRequest(fdc.requester_id, go), () => acceptRequest(fdc.id, go), null);

        }

        void normalAction(GameObject go)
        {
            followersUIElement fuiE = go.GetComponent<followersUIElement>();
            followerDataClass fdc = fuiE.getFollower();
            // Debug.Log("clicked me " + fdc.requester_id);
            string remove_teacher = localizationProvider.provide.getLanguage().getAMessageText(54);
            windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, remove_teacher, "", "",
                cancel, null, () => rejectRequest(fdc.requester_id, go), null, null);
        }

        // Shariz 28th Oct 2019 2.00 updating no teachers text
        void updateNoTeachers(){
            if(!tManager.hasTeachers){ // Shariz 28th Oct 2019 2.00
                Debug.Log("Doesn't have any teachers");
                hasNoTeachers.getSettingsUI().setStateNoTeachersIndicator(true);
            } else {
                hasNoTeachers.getSettingsUI().setStateNoTeachersIndicator(false);
            }
        }


    }
}
