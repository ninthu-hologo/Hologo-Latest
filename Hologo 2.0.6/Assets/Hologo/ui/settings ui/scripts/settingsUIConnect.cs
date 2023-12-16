using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// settings ui connector to ui
    /// </summary>
    public class settingsUIConnect : MonoBehaviour
    {

        [Header("UI ELEMENTS")]
        [SerializeField]
        private settingsSwitcher settingsUI;
        [SerializeField]
        private userUI userUI;
        [SerializeField]
        private settingsSwitcher settingsUIGameObject;
        [SerializeField]
        private localizePrefab_SO followerPrefab;
        [Header("SUBSCRIPTION UI ELEMENTS")]
        [SerializeField]
        private localizePrefab_SO subscriptionUIElement;
        [Header("TEACHER RECORDED LESSONS UI ELEMENTS")]
        [SerializeField]
        private localizePrefab_SO recordedLessonUIElement;


        // holder for all temp ui list buttons . eg> subscriptions ui elements, teachers,lessons,students
        private List<GameObject> listElements;

        // setting main settings ui state to enable or disable
        // then checking id user is signed in if so we enable settings panel
        // if not we enable sign in panel
        // also user type is checked so when we enable settings user specific buttons can be enabled
        public void setSettingsUIState(bool state , bool isSignedIn, int userType)
        {
            settingsUIGameObject.getSettingsObject().SetActive(state);
            if(state==true){ // shariz v2 13/9/2019
                if(!isSignedIn)
                {
                    EnableUserUI();
                }
                else
                {
                    EnableSettingsUI(userType);
                }
            }
        }

        public void closeUserUI()
        {
            userUI.disableMainSignInGroup();
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }
        // enabling user sign in ui
        public void EnableUserUI()
        {
            userUI.enableMainSignInGroup();
        }
        public void EnableSubscriptionsUI() // Shariz 12th Nov 2019 2.01      
        {
            userUI.enableMySubscriptionsGroup();
        }

        public void EnableSubscriptions()
        {
            userUI.disableSubscription(true);
        }

        // enabling settings ui and also set user specific buttons to active
        public void EnableSettingsUI(int userType)
        {
            //set teacher type
            settingsUI.getSettingsUI().enableMainSettingsGroup();
            settingsUI.getSettingsUI().changeSettingsView(settingsUIEnum.SETTINGSMAIN);
            if (userType == 1)
            {
                settingsUI.getSettingsUI().setTeacherSpecificSettings();
                return;
            }
            // set student type
            if(userType == 2)
            {
                settingsUI.getSettingsUI().setStudentSpecificSettings();
            }
           
        }




        #region USER FUNCTIONS
        // changing between sign in and sign up view
        public void SignUpView()
        {
            userUI.setSignUpOrSignIn(userUIEnum.SIGNUP);
        }

        // exp above
        public void SignInView()
        {
            userUI.setSignUpOrSignIn(userUIEnum.SIGNIN);
        }

        //assigned to onvalue changed of sign in email and password to check if values have been entered and enable sign in button
        public void CheckToEnableSignInButton()
        {
            if (!string.IsNullOrEmpty(userUI.readSignInEmail()) && !string.IsNullOrEmpty(userUI.readSignInPassword()))
            {
                userUI.activeStateSignInButton(true);
            }
            else
            {
                userUI.activeStateSignInButton(false);
            }
        }

        // assigned to onvalue changed of sign up username, email , password and usetype to check if values have been entered and enable sign up button
        public void CheckToEnableSignUpButton() // shariz 27 oct 2019
        {
            if (!string.IsNullOrEmpty(userUI.readSignUpUserName()) && !string.IsNullOrEmpty(userUI.readSignUpEmail())
                && !string.IsNullOrEmpty(userUI.readSignUpPassword()) &&  !string.IsNullOrEmpty(userUI.getUserTypeSelection()) && checkPasswordLenght(userUI.readSignUpPassword()))
            {
                userUI.activeStateSignUpButton(true);
            }
            else
            {
                userUI.activeStateSignUpButton(false);
            }
        }

        bool checkPasswordLenght(string password){ // shariz 27 oct 2019
            return password.Length >= 6;
        }

        // check onvalue change of change password . also compare new password and retype new password
        public void CheckToEnableChangePasswordButton()
        {
            Debug.Log("Checking before enabling change password button");
            if (!string.IsNullOrEmpty(settingsUI.getSettingsUI().readOldPassword()) && !string.IsNullOrEmpty(settingsUI.getSettingsUI().readNewPassword())
                && !string.IsNullOrEmpty(settingsUI.getSettingsUI().readRetypePassword()))
            {
                if (string.Equals(settingsUI.getSettingsUI().readNewPassword(), settingsUI.getSettingsUI().readRetypePassword(), System.StringComparison.Ordinal))
                {
                    settingsUI.getSettingsUI().activeStateChangePasswordButton(true);
            Debug.Log("Everything works so change password button enabled");
                    return;
                }
                            Debug.Log("New passwords don't match so disabling change password button");

                settingsUI.getSettingsUI().activeStateChangePasswordButton(false);
            }
                        Debug.Log("Fields empty so disabling change password button");

            settingsUI.getSettingsUI().activeStateChangePasswordButton(false);
        }


        //Shariz 29th March 2020 2.0.4 adding clear fields after password update
        public void clearPasswordFields(){
            settingsUI.getSettingsUI().oldPassword.text = "";
            settingsUI.getSettingsUI().newPassword.text = "";
            settingsUI.getSettingsUI().retypePassword.text = "";
        }

        #endregion

        #region USER PROFILE
        public void showUserNameAndType(string user, string type)
        {
            //user/parent/student_details/namex > type
            settingsUI.getSettingsUI().showUserNameAndType(user, type);
        }

        public void populateProfile(string name,string contact_number, string email)
        {
            settingsUI.getSettingsUI().populateProfile(name, contact_number, email);
        }

        #endregion

     


        #region SUBSCRIPTION FUNCTIONS

        public void makeSubscriptionsUI(List<SubscriptionDataClass> dataList, int userType,params CallbackGameObject[] actions)
        {
            deleteUIElements(listElements);
            listElements = new List<GameObject>();
            for (int i = 0; i < dataList.Count; i++)
            {
                GameObject go = Instantiate(subscriptionUIElement.givePrefab());
                subscriptionUIElement ele = go.GetComponent<subscriptionUIElement>();
                ele.fillWithData(dataList[i], userType,actions);
                go.transform.SetParent(settingsUI.getSettingsUI().subscriptionsParent());
                go.transform.localScale = new Vector3(1, 1, 1);
                listElements.Add(go);
            }
        }

        #endregion


        #region TEACHER RECORDED LESSONS MAKE
        // makes recorded lessons buttons ui for the loaded experience
        public void makeLessonSmartButtons(bool makeNew,List<audioClipDetail> clipDataList, params CallbackGameObject[] actions)
        {
            if (makeNew)
            {
                deleteUIElements(listElements);
                listElements = new List<GameObject>();
            }
            for (int i = 0; i < clipDataList.Count; i++)
            {
                GameObject go = Instantiate(recordedLessonUIElement.givePrefab());
                recordedLessonUIElement iui = go.GetComponent<recordedLessonUIElement>();
                iui.fillWithData(clipDataList[i],i, actions);
                go.transform.SetParent(settingsUI.getSettingsUI().lessonsParent());
                go.transform.localScale = new Vector3(1, 1, 1);
                listElements.Add(go);
            }
            settingsUI.getSettingsUI().setStateNoLessonsIndicator(false);
        }


        public GameObject getNextLesson(int id)
        {
            id++;

            if (id == listElements.Count)
                id = 0;

            return listElements[id];
        }

        public void ShowNoLessonsText()
        {
            settingsUI.getSettingsUI().setStateNoLessonsIndicator(true);
        }

        #endregion


        #region STUDENTS

        public void makeStudentUIButtons(bool isTeachers,List<followerDataClass> students, CallbackGameObject pendingAction, CallbackGameObject action)
        {
            
                deleteUIElements(listElements);
                listElements = new List<GameObject>();
            for (int i = 0; i < students.Count; i++)
            {
               
                GameObject go = Instantiate(followerPrefab.givePrefab());
                followersUIElement fuiE = go.GetComponent<followersUIElement>();
                if (students[i].status == true)
                {
                    fuiE.fillData(students[i].status, students[i], action);
                }
                else
                {
                    fuiE.fillData(students[i].status, students[i], pendingAction);
                }
                if(isTeachers)
                {
                    go.transform.SetParent(settingsUI.getSettingsUI().getTeachersParent());
                }
                else
                {
                    go.transform.SetParent(settingsUI.getSettingsUI().getStudentsParent());
                }
               
                go.transform.localScale = new Vector3(1, 1, 1);
                listElements.Add(go);
                }

        }
        #endregion


        #region TEACHERS
        public void makeTeacherUIButtons(List<teacherDetail> teachers , params CallbackGameObject[] actions)
        {
            deleteUIElements(listElements);
            listElements = new List<GameObject>();
            for (int i = 0; i < teachers.Count; i++)
            {

            }

        }

        public void setStateNoLessonsIndicator(bool state)
        {
            settingsUI.getSettingsUI().setStateNoLessonsIndicator(state);
        }

        #endregion

        public void removeUIElement(GameObject go)
        {
            listElements.Remove(go);
            Destroy(go);
        }


        void deleteUIElements(List<GameObject> gameObjects)
        {
            if (gameObjects == null)
                gameObjects = new List<GameObject>();

            if (gameObjects.Count <= 0)
                return;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                Destroy(gameObjects[i]);
            }

            gameObjects.Clear();
            gameObjects = new List<GameObject>();
        }
    }
}
