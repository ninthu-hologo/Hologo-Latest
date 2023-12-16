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
    /// view controller for students 
    /// </summary>
    public class studentsViewController : MonoBehaviour
    {

        [Header("DATA")]
        [SerializeField]
        private userData_SO userData;
        [Header("SCRIPTS")]
        [SerializeField]
        private studentsManager stManager;
        [SerializeField]
        private settingsUIConnect sConnect;
        [Header("Scene Objects")]
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;

        [SerializeField]
        private settingsSwitcher hasNoStudents;// Shariz 28th Oct 2019 2.00

        


        public void initiateStudentsViewController()
        {
            // get my students
            // get my students pending list
            // generate ui for them
            initialize();
        }

        async void initialize()
        {
            await geAcceptedStudents();
            await getPendingStudents();

            updateNoStudents(); // Shariz 28th Oct 2019 2.00
        }

        public void AddStudent()
        {
            string title = localizationProvider.provide.getLanguage().getAMessageText(46);
            string description = localizationProvider.provide.getLanguage().getAMessageText(47);
            string email = localizationProvider.provide.getLanguage().getALabelText(2);
            windowMain.ModalWindowE.getiOS_ModalWindowE().FillInput(true, title, description, email, addStudentFromModal);
        }



        void addStudentFromModal(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                Debug.Log("please enter an email address");
            }
            else
            {
                Debug.Log("add teacher");
                addStudent(emailAddress);
            }
        }

        async void addStudent(string emailAddress)
        {
            bool success = await stManager.addStudent(emailAddress, userData.requestMyToken());
                    string successTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(18); // Shariz 21st March 2020 2.0.4
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
            if (success)
            {
                string successMessage = "Your request has been sent to the respective student you would like to add to your classroom.";// Shariz 29th March 2020 2.0.4
                windowMain.ModalWindowD.ShowInfo(true, true, successTitle, "<font=English>"+successMessage+"</font>", ok);// Shariz 29th March 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+stManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
        }

        async Task geAcceptedStudents()
        {
            bool success = await stManager.getStudentsFromServer(userData.requestMyToken(), userData.requestUserDetail().id);
           
        }



        async Task getPendingStudents()
        {
            bool success = await stManager.getStudentsPending(userData.requestMyToken());
            sConnect.makeStudentUIButtons(false,stManager.getPendingStudents(), pendingAction,normalAction);
        }



        void cancel()
        {

        }

        void acceptRequest(int id,GameObject go)
        {
            Debug.Log("accepted me " + id);
            acceptRequestAsync(id,go);
            
        }

        async void acceptRequestAsync(int id, GameObject go)
        {
            bool success = await stManager.AcceptStudent(id, userData.requestMyToken());
            if(success)
            {
                followersUIElement fuiE = go.GetComponent<followersUIElement>();
                fuiE.acceptedStateOn();
            }
            else
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+stManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
            }
        }


        void rejectRequest(int id, GameObject go)
        {
            Debug.Log("rejected me " +id);
            rejectRequestAsync(id,go);
        }

        async void rejectRequestAsync(int id, GameObject go)
        {
            bool success = await stManager.removeRejectStudent(id, userData.requestMyToken());
            if (success)
            {
                sConnect.removeUIElement(go);
                
            }
            else
            {
                    string warningTitle = localizationProvider.provide.getLanguage().getATitleText(19);
                    string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                windowMain.ModalWindowD.ShowInfo(true, false, warningTitle, "<font=English>"+stManager.readMessage()+"</font>", ok);// Shariz 22nd Feb 2020 2.0.4
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
                cancel, null, () => rejectRequest(fdc.requester_id,go), () => acceptRequest(fdc.id,go), null);

        }

        void normalAction(GameObject go)
        {
            followersUIElement fuiE = go.GetComponent<followersUIElement>();
            followerDataClass fdc = fuiE.getFollower();
            // Debug.Log("clicked me " + fdc.requester_id);
            string remove_student = localizationProvider.provide.getLanguage().getAMessageText(50);
            windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, remove_student, "", "",
                cancel, null, () => rejectRequest(fdc.requester_id,go), null, null);
        }

         // Shariz 28th Oct 2019 2.00
        void updateNoStudents(){
            if(!stManager.hasStudents){
                Debug.Log("Doesn't have any students");
                hasNoStudents.getSettingsUI().setStateNoStudentsIndicator(true);
            } else {
                hasNoStudents.getSettingsUI().setStateNoStudentsIndicator(false);
            }
        }

    }
}