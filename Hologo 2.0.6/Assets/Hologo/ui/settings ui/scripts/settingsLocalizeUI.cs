using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 01 September 2019
    /// Modified by: 
    /// settings ui prefab localize 
    /// </summary>
    public class settingsLocalizeUI : MonoBehaviour, IPrefabLocalize
    {
        [Header("STATIC TEXTS")]
        [SerializeField]
        private TextMeshProUGUI SettingsTitle;
        [Header("INPUT FEILDS")]
        
        [Header("SETTINGS PAGE")]
        [SerializeField]
        private TextMeshProUGUI settingSettingsText;
        [SerializeField]
        private TextMeshProUGUI settingSubscriptionsText;
        [SerializeField]
        private TextMeshProUGUI settingTeachersText;
        [SerializeField]
        private TextMeshProUGUI settingStudentsText;
        // shariz v2 1/9/2019
        [SerializeField]
        private TextMeshProUGUI settingsLessonsText;
        [SerializeField]
        private TextMeshProUGUI settingClearCacheText;        
        // shariz v2 1/9/2019
        [SerializeField]
        private TextMeshProUGUI settingsCountryText;
        [SerializeField]
        private TextMeshProUGUI settingStartArModeText;
        [SerializeField]
        private TextMeshProUGUI settingLabelsOnText;

        [SerializeField]
        private TextMeshProUGUI settingsOnboardingText;// Shariz 25th Oct 2019 2.00
        
        [SerializeField]
        private TextMeshProUGUI settingLabelsSizeText;
        [SerializeField]
        private TextMeshProUGUI settingAppSoundText;
        [SerializeField]
        private TextMeshProUGUI settingAboutUsText;
        [SerializeField]
        private TextMeshProUGUI settingPrivacyPolicyText;
        [SerializeField]
        private TextMeshProUGUI settingContactUsText;
        [SerializeField]
        private TextMeshProUGUI settingFAQText;
        [SerializeField]
        private TextMeshProUGUI settingAppVersionText;
        [Header("PROFILE PAGE")]
        [SerializeField]
        private TextMeshProUGUI profileEditProfileText;
        [SerializeField]
        private TextMeshProUGUI profileNameLabel;
        [SerializeField]
        private TextMeshProUGUI profileNameInputText;
        [SerializeField]
        private TextMeshProUGUI profileAddressLabel;
        [SerializeField]
        private TextMeshProUGUI profileAddressInputText;
        [SerializeField]
        private TextMeshProUGUI profileEmailLabel;
        [SerializeField]
        private TextMeshProUGUI profileEmailDisplayText;        
        // shariz v2 1/9/2019
        [SerializeField]
        private TextMeshProUGUI profilePhoneLabel;
        [SerializeField]
        private TextMeshProUGUI profilePhoneDisplayText;
        [SerializeField]
        private TextMeshProUGUI profileSaveButtonText;
        [Header("TEACHERS PAGE")]
        [SerializeField]
        private TextMeshProUGUI teachersMyTeachersText;
        // shariz v2 1/9/2019        
        [SerializeField]
        private TextMeshProUGUI teachersNoTeachersText;
        [Header("STUDENTS PAGE")]
        [SerializeField]
        private TextMeshProUGUI studentsMyStudentsText;
        // shariz v2 1/9/2019        
        [SerializeField]
        private TextMeshProUGUI studentsNoStudentsText;
        [Header("LESSONS PAGE")]
        [SerializeField]
        private TextMeshProUGUI lessonsMyLessonsText;
        // shariz v2 1/9/2019        
        [SerializeField]
        private TextMeshProUGUI lessonsNoLessonsText;
        [Header("USER PAGE")]
        [SerializeField]
        private TextMeshProUGUI userNameText;
        [SerializeField]
        private TextMeshProUGUI userTypeText;
        [SerializeField]
        private TextMeshProUGUI userEditProfileButtonText;
        [SerializeField]
        private TextMeshProUGUI userChangePasswordButtonText;
        [SerializeField]
        private TextMeshProUGUI userSignOutButtonText;
        [Header("CHANGE PASSWORD PAGE")]
        [SerializeField]
        private TextMeshProUGUI cpChangePasswordText;
        [SerializeField]
        private TextMeshProUGUI cpOldPasswordLabel;
        [SerializeField]
        private TextMeshProUGUI cpOldPasswordInput;
        [SerializeField]
        private TextMeshProUGUI cpNewPasswordLabel;
        [SerializeField]
        private TextMeshProUGUI cpNewPasswordInput;
        [SerializeField]
        private TextMeshProUGUI cpRetypeNewPasswordLabel;
        [SerializeField]
        private TextMeshProUGUI cpRetypeNewPasswordInput;
        [SerializeField]
        private TextMeshProUGUI cpUpdateButtonText;

        [SerializeField]
        private TextMeshProUGUI cacheDescriptionText;

        [SerializeField]
        private TextMeshProUGUI changeDescriptionText;

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(SettingsTitle, language.getAFont(0), language.getATitleText(1));
            

            localizeSetting.setTextConfig(settingSettingsText, language.getAFont(14), language.getATitleText(1));
            localizeSetting.setTextConfig(settingSubscriptionsText, language.getAFont(14), language.getAButtonText(11));
            localizeSetting.setTextConfig(settingTeachersText, language.getAFont(14), language.getALabelText(10));
            localizeSetting.setTextConfig(settingStudentsText, language.getAFont(14), language.getALabelText(9));
            localizeSetting.setTextConfig(settingsLessonsText, language.getAFont(14), language.getAButtonText(12));
            localizeSetting.setTextConfig(settingClearCacheText, language.getAFont(14), language.getAButtonText(13));
            localizeSetting.setTextConfig(settingsCountryText, language.getAFont(14), language.getAMessageText(61));
            localizeSetting.setTextConfig(settingStartArModeText, language.getAFont(14), language.getAButtonText(16));
            localizeSetting.setTextConfig(settingLabelsOnText, language.getAFont(14), language.getAButtonText(14));
            localizeSetting.setTextConfig(settingsOnboardingText, language.getAFont(14), language.getALabelText(66));// Shariz 25th Oct 2019 2.00
            localizeSetting.setTextConfig(settingLabelsSizeText, language.getAFont(14), language.getAButtonText(17));
            localizeSetting.setTextConfig(settingAppSoundText, language.getAFont(14), language.getAButtonText(15));
            localizeSetting.setTextConfig(settingAboutUsText, language.getAFont(14), language.getAButtonText(18));
            localizeSetting.setTextConfig(settingPrivacyPolicyText, language.getAFont(14), language.getAButtonText(19));
            localizeSetting.setTextConfig(settingContactUsText, language.getAFont(14), language.getAButtonText(26));
            localizeSetting.setTextConfig(settingFAQText, language.getAFont(14), language.getAButtonText(31));
            localizeSetting.setTextConfig(settingAppVersionText, language.getAFont(14), language.getAButtonText(25));

            localizeSetting.setTextConfig(profileEditProfileText, language.getAFont(0), language.getAButtonText(10));// Shariz 26th Oct 2019 2.00
            localizeSetting.setTextConfig(profileNameLabel, language.getAFont(22), language.getALabelText(11));
            localizeSetting.setTextConfig(profileNameInputText, language.getAFont(34));
            localizeSetting.setTextConfig(profileAddressLabel, language.getAFont(22), language.getALabelText(12));
            localizeSetting.setTextConfig(profileAddressInputText, language.getAFont(34));
            localizeSetting.setTextConfig(profileEmailLabel, language.getAFont(22), language.getALabelText(2));
            localizeSetting.setTextConfig(profileEmailDisplayText, language.getAFont(34));
            localizeSetting.setTextConfig(profilePhoneLabel, language.getAFont(22),language.getALabelText(13));
            localizeSetting.setTextConfig(profilePhoneDisplayText, language.getAFont(34));
            localizeSetting.setTextConfig(profileSaveButtonText, language.getAFont(8), language.getAButtonText(32));

            localizeSetting.setTextConfig(teachersMyTeachersText, language.getAFont(0), language.getALabelText(10));// Shariz 26th Oct 2019 2.00
            localizeSetting.setTextConfig(teachersNoTeachersText, language.getAFont(32), language.getAbodyText(8));
            localizeSetting.setTextConfig(studentsMyStudentsText, language.getAFont(0), language.getALabelText(9));// Shariz 26th Oct 2019 2.00
            localizeSetting.setTextConfig(studentsNoStudentsText, language.getAFont(32), language.getAbodyText(13));
            localizeSetting.setTextConfig(lessonsMyLessonsText, language.getAFont(0), language.getAButtonText(12));// Shariz 26th Oct 2019 2.00
            localizeSetting.setTextConfig(lessonsNoLessonsText, language.getAFont(32), language.getAbodyText(14));

            // localizeSetting.setTextConfig(userNameText, language.getAFont(34));// Shariz 22nd Feb 2020 2.0.4 comment out for correct font
            // localizeSetting.setTextConfig(userTypeText, language.getAFont(6));
            // Debug.Log("usertype text is "+userTypeText.text);
            // if(userTypeText.text=="Student"){
            //     string usertype = localizationProvider.provide.getLanguage().getAButtonText(7);
            //     userTypeText.text = usertype;
            //     Debug.Log("usertype text should be "+usertype);
            // }
            // if(userTypeText.text=="Teacher"){
            //     string usertype = localizationProvider.provide.getLanguage().getAButtonText(8);
            //     userTypeText.text = usertype;
            //     Debug.Log("usertype text should be "+usertype);
            // }
            
            
            localizeSetting.setTextConfig(userEditProfileButtonText, language.getAFont(14), language.getAButtonText(24));
            localizeSetting.setTextConfig(userChangePasswordButtonText, language.getAFont(14), language.getAButtonText(21));
            localizeSetting.setTextConfig(userSignOutButtonText, language.getAFont(14), language.getAButtonText(27));


            localizeSetting.setTextConfig(cpChangePasswordText, language.getAFont(0), language.getALabelText(3));// Shariz 26th Oct 2019 2.00
            localizeSetting.setTextConfig(cpOldPasswordLabel, language.getAFont(22), language.getALabelText(14));
            localizeSetting.setTextConfig(cpOldPasswordInput, language.getAFont(34));
            localizeSetting.setTextConfig(cpNewPasswordLabel, language.getAFont(22), language.getALabelText(15));
            localizeSetting.setTextConfig(cpNewPasswordInput, language.getAFont(34));
            localizeSetting.setTextConfig(cpRetypeNewPasswordLabel, language.getAFont(22), language.getALabelText(16));
            localizeSetting.setTextConfig(cpRetypeNewPasswordInput, language.getAFont(34));
            localizeSetting.setTextConfig(cpUpdateButtonText, language.getAFont(8), language.getAButtonText(28));

            localizeSetting.setTextConfig(cacheDescriptionText, language.getAFont(40), language.getAMessageText(18));
            localizeSetting.setTextConfig(changeDescriptionText, language.getAFont(40), language.getAMessageText(19));

        }
    }

}