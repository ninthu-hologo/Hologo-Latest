using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// settings ui element 
    /// </summary>
    public class settingsUI : iUILayoutBase
    {
        [Header("SETTINGS")]
        //[SerializeField]
        //float topbarNormalHeight = 109f;
        //[SerializeField]
        //float topbarCollapsedHeight = 50f;


        [Header("UI MAIN GROUPS")]
        // [SerializeField]
        //private GameObject mainSignInSignUpGroup;
        [SerializeField]
        private GameObject mainSettingsGroup;
        [Header("UI ELEMENTS SETTINGS MAIN GROUP")]
        [SerializeField]
        private TextMeshProUGUI heading;
        [SerializeField]
        private RectTransform topBar;
        [SerializeField]
        private RectTransform body;
        [SerializeField]
        private GameObject settingsButtonGroup;
        [SerializeField]
        private GameObject teachersGroup;
        [SerializeField]
        private GameObject studentsGroup;
        [SerializeField]
        private GameObject lessonsGroup;
        [SerializeField]
        private GameObject userGroup;
        [SerializeField]
        private GameObject profileEditGroup;
        [SerializeField]
        private GameObject changePasswordGroup;
        // buttons
        [SerializeField]
        private GameObject teachersButton;
        [SerializeField]
        private GameObject studentsButton;
        [SerializeField]
        private GameObject lessonsButton;
        [SerializeField]
        private GameObject backButton;
        [SerializeField]
        private switchButtonUIElement arModeButton;
        [SerializeField]
        private switchButtonUIElement soundOnOffButton;
        [SerializeField]
        private switchButtonUIElement lablesOnOffButton;
        
        [SerializeField]
        private switchButtonUIElement onboardingOnOffButton;// Shariz 25th Oct 2019 2.00

        [Header("UI ELEMENTS SUBSCRIPTIONS GROUP")]
        [SerializeField]
        private Transform subscriptionLayoutParent;
        [Header("UI ELEMENTS TEACHERS GROUP")]
        [SerializeField]
        private Transform teacherLayoutParent;
        [SerializeField]
        private GameObject noTeachersExistIndicatorGroup;
        [Header("UI ELEMENTS STUDENTS GROUP")]
        [SerializeField]
        private Transform studentLayoutParent;
        [SerializeField]
        private GameObject noStudentsExistIndicatorGroup;
        [Header("UI ELEMENTS LESSONS GROUP")]
        [SerializeField]
        private Transform lessonsLayoutParent;
        [SerializeField]
        private GameObject nolessonsExistIndicatorGroup;
        [SerializeField]
        private GameObject player;
        [Header("UI ELEMENTS EDIT PROFILE GROUP")]
        [SerializeField]
        private TMP_InputField editProfileName;
        //[SerializeField]
        //private TMP_InputField editProfileAddress;
        [SerializeField]
        private TMP_InputField editProfileEmail;
        [SerializeField]
        private TMP_InputField editProfilePhone;
        [Header("UI ELEMENTS CHANGE PASSWORD GROUP")]
        [SerializeField]
        public TMP_InputField oldPassword; //Shariz 29th March 2020 2.0.4 adding clear fields after password update
        [SerializeField]
        public TMP_InputField newPassword; //Shariz 29th March 2020 2.0.4 adding clear fields after password update
        [SerializeField]
        public TMP_InputField retypePassword; //Shariz 29th March 2020 2.0.4 adding clear fields after password update
        [SerializeField]
        private Button changePasswordButton;
        [Header("UI User GROUP")]
        [SerializeField]
        private TextMeshProUGUI userName;
        [SerializeField]
        private TextMeshProUGUI userType;

        [SerializeField]
        private Image userImage;
        [SerializeField]
        private Sprite studentSprite;
        [SerializeField]
        private Sprite teacherSprite;



        public void enableMainSettingsGroup()
        {
            mainSettingsGroup.SetActive(true);
            // mainSignInSignUpGroup.SetActive(false);

        }

        public switchButtonUIElement getArModeButton()
        {
            return arModeButton;
        }

        public switchButtonUIElement getSoundOnOffButton()
        {
            return soundOnOffButton;
        }

        public switchButtonUIElement getLabelsOnOffButton()
        {
            return lablesOnOffButton;
        }

        // Shariz 25th Oct 2019 2.00
        public switchButtonUIElement getOnboardingOnOffButton()
        {
            return onboardingOnOffButton;
        }

        public void enableSettingsGroup()
        {
            changeSettingsView(settingsUIEnum.SETTINGSMAIN);
            userImage.sprite = studentSprite;
        }

        #region CHANGE PASSWORD
        public string readOldPassword()
        {
            return oldPassword.text;
        }

        public string readNewPassword()
        {
            return newPassword.text;
        }

        public string readRetypePassword()
        {
            return retypePassword.text;
        }

        public void activeStateChangePasswordButton(bool state)
        {
            changePasswordButton.interactable = state;
        }

        #endregion

        #region SETTINGS

        public void setStudentSpecificSettings()
        {
            // here we disable teacher specific buttons in settings
            teachersButton.SetActive(true);
            studentsButton.SetActive(false);
            lessonsButton.SetActive(false);
            userImage.sprite = studentSprite;
        }
        public void setTeacherSpecificSettings()
        {
            // here we disable student specific buttons in settings
            teachersButton.SetActive(false);
            studentsButton.SetActive(true);
            lessonsButton.SetActive(true);
            userImage.sprite = teacherSprite;
        }

        public void changeSettingsView(settingsUIEnum settings)
        {
            switch (settings)
            {
                case settingsUIEnum.SETTINGSMAIN:
                    settingsButtonGroup.SetActive(true);
                    //                    subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(false);
                    break;
                case settingsUIEnum.SUBSCRIPTIONS:
                    settingsButtonGroup.SetActive(false);
                    //                    subscriptionsGroup.SetActive(true);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    player.SetActive(false);
                    break;
                case settingsUIEnum.TEACHERS:
                    settingsButtonGroup.SetActive(false);
                    //  subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(true);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    break;
                case settingsUIEnum.STUDENTS:
                    settingsButtonGroup.SetActive(false);
                    //  subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(true);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    break;
                case settingsUIEnum.LESSONS:
                    settingsButtonGroup.SetActive(false);
                    //  subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(true);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    break;
                case settingsUIEnum.USER:
                    settingsButtonGroup.SetActive(false);
                    // subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(true);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    player.SetActive(false);
                    break;
                case settingsUIEnum.PROFILE:
                    settingsButtonGroup.SetActive(false);
                    // subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(true);
                    changePasswordGroup.SetActive(false);
                    backButton.SetActive(true);
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
                    SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                    break;
                case settingsUIEnum.CHANGEPASSWORD:
                    settingsButtonGroup.SetActive(false);
                    // subscriptionsGroup.SetActive(false);
                    teachersGroup.SetActive(false);
                    studentsGroup.SetActive(false);
                    lessonsGroup.SetActive(false);
                    userGroup.SetActive(false);
                    profileEditGroup.SetActive(false);
                    changePasswordGroup.SetActive(true);
                    backButton.SetActive(true);
                    break;
                default:
                    break;
            }
            // settings player bar to false here. lessons view controller will enable it if there are lessons
            if (settings != settingsUIEnum.LESSONS)
                player.SetActive(false);
        }

        #endregion

        public Transform subscriptionsParent()
        {
            return subscriptionLayoutParent;
        }

        public Transform lessonsParent()
        {
            return lessonsLayoutParent;
        }

        public Transform getStudentsParent()
        {
            return studentLayoutParent;
        }

        public Transform getTeachersParent()
        {
            return teacherLayoutParent;
        }

        public void setStateNoLessonsIndicator(bool state)
        {
            nolessonsExistIndicatorGroup.SetActive(state);
        }
        public void setStateNoStudentsIndicator(bool state)
        {
            noStudentsExistIndicatorGroup.SetActive(state);
        }
        public void setStateNoTeachersIndicator(bool state)
        {
            noTeachersExistIndicatorGroup.SetActive(state);
        }

        #region USER PROFILE
        public void showUserNameAndType(string user, string type)
        {
            //user/parent/student_details/namex > type
            userName.text = user;
            userType.text = type;

        }

        public void populateProfile(string name, string contact_number, string email)
        {
            editProfileName.text = name;
            editProfileEmail.text = email;
            editProfileEmail.DeactivateInputField();
            editProfilePhone.text = contact_number;
        }

        public string readName()
        {
            return editProfileName.text;
        }

        public string readContactNumber()
        {
            return editProfilePhone.text;
        }

        #endregion


    }


}
