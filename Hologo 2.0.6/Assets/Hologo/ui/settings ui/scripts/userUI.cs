using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 August 2019
    /// Modified by: 
    /// settings ui element 
    /// </summary>
    public class userUI : iUILayoutBase, IPrefabLocalize
    {

        [SerializeField]
        private settings_SO mainSettings;

        [SerializeField]
        private GameObject userCanvas;
        [SerializeField]
        private GameObject mainSignInSignUpGroup;

        [SerializeField]
        private GameObject signInGroup;
        [SerializeField]
        private RectTransform signInFooter;//shariz 12/9/2019 v2
        [SerializeField]
        private GameObject subscriptionIntro;
        [SerializeField]
        private RectTransform subscriptionIntroFooter;//shariz 12/9/2019 v2

        [SerializeField]
        private GameObject signUpGroup;
        [SerializeField]
        private RectTransform signUpFooter; //shariz 12/9/2019 v2

        [Header("SIGN IN")]
        [SerializeField]
        private TMP_InputField signInEmailX;
        [SerializeField]
        private TMP_InputField signInPasswordX;

        [SerializeField]
        private TextMeshProUGUI welcomeBackText;
        [SerializeField]
        private TextMeshProUGUI welcomeBackBodyText;
        [SerializeField]
        private TextMeshProUGUI signInEmailLabel;
        [SerializeField]
        private TextMeshProUGUI signInEmail;
        [SerializeField]
        private TextMeshProUGUI signInPasswordLabel;
        [SerializeField]
        private TextMeshProUGUI signInPassword;
        [SerializeField]
        private TextMeshProUGUI forgotYourPasswordText;
        [SerializeField]
        private TextMeshProUGUI signInButtonText;
        [SerializeField]
        private TextMeshProUGUI signInsignUpButtonText;
        [SerializeField]
        private TextMeshProUGUI dontHaveAnAccountText;

        [SerializeField]
        private Button signInButton;



        //shariz adding images for tablet

        [SerializeField]
        private Image signInImage;
        [SerializeField]
        private Sprite signInGraphic;
        [SerializeField]
        private Sprite signInGraphicTablet;



        [Header("SIGN UP")]
        [SerializeField]
        private TMP_InputField userNameX;
        [SerializeField]
        private TMP_InputField signUpEmailX;
        [SerializeField]
        private TMP_InputField signUpPasswordX;

        [SerializeField]
        private TextMeshProUGUI letsgetStartedText;
        [SerializeField]
        private TextMeshProUGUI letsgetStartedBodyText;
        [SerializeField]
        private TextMeshProUGUI signUpUserNameLabel;
        [SerializeField]
        private TextMeshProUGUI signUpUserName;
        [SerializeField]
        private TextMeshProUGUI signUpEmailLabel;
        [SerializeField]
        private TextMeshProUGUI signUpEmail;
        [SerializeField]
        private TextMeshProUGUI signUpPasswordLabel;
        [SerializeField]
        private TextMeshProUGUI signUpPassword;
        [SerializeField]
        private TextMeshProUGUI signUnButtonText;
        [SerializeField]
        private TextMeshProUGUI signUpsignInButtonText;
        [SerializeField]
        private TextMeshProUGUI alreadyHaveAnAccountText;

        [SerializeField]
        private Button signUpButton;

        //shariz adding user type selection
        [SerializeField]
        private TMP_Dropdown userTypeSelect; 

        //shariz adding images for tablet

        [SerializeField]
        private Image signUpImage;
        [SerializeField]
        private Sprite signUpGraphic;
        [SerializeField]
        private Sprite signUpGraphicTablet;

        [Header("SUBSCRIPTION INTRO")]
        [SerializeField]
        private GameObject subscriptionsGroup;

        [SerializeField]
        private TextMeshProUGUI subsStudentButtonText;
        [SerializeField]
        private TextMeshProUGUI subsTeacherButtonText;
        [SerializeField]
        private TextMeshProUGUI subsDescriptionText;
        [SerializeField]
        private TextMeshProUGUI subsNameText;
        [SerializeField]
        private TextMeshProUGUI subsPriceText;

        [SerializeField]
        private GameObject userTypeButtonsGroup;
        [SerializeField]
        private Image subcriptionTypeImage;
        [SerializeField]
        private Sprite studentGraphic;
        [SerializeField]
        private Sprite teacherGraphic;


        




        //shariz adding images for tablet

        [SerializeField]
        private Sprite studentGraphicTablet;
        [SerializeField]
        private Sprite teacherGraphicTablet;
        
        [SerializeField]
        private GameObject alreadyBoughtArrow;
        [SerializeField]
        private GameObject nonBoughtArrow;
        [SerializeField]
        private Image studentButtonBg;
        [SerializeField]
        private Image teacherButtonBg;
        [SerializeField]
        private Color selected;
        [SerializeField]
        private Color deselected;
        

        // Shariz - adding free trial offer button for Sri Lanka 25th March 2020 2.0.4
        [SerializeField]
        private GameObject freeTrialButton;
        
        // Shariz - adding free trial offer button for Sri Lanka 25th March 2020 2.0.4
        [SerializeField]
        private TextMeshProUGUI freeTrialButtonText;


        //Shariz adding Pay by Genie icon 2.0.6 26th May 2020
        [SerializeField]
        public Sprite payByGenieSprite;


        // Shariz 27th Oct 2019 2.00 adding my subscriptions page
        [Header("MY SUBSCRIPTIONS")]
        [SerializeField]
        private TextMeshProUGUI mySubscriptionsTitle;
        [SerializeField]
        private TextMeshProUGUI yourSubscriptionsText;
        [SerializeField]
        private TextMeshProUGUI subscriptionName;
        [SerializeField]
        private TextMeshProUGUI subscriptionSubtitle;
        
        [SerializeField]
        private TextMeshProUGUI lessonCountText;
        
        [SerializeField]
        private TextMeshProUGUI expiresOnDateText;

        [SerializeField]
        private Image subscriptionIcon;

        [SerializeField]
        private ContentSizeFitter mySubscriptionParent;        
        
        [SerializeField]
        private TextMeshProUGUI subscriptionMessage;

        [SerializeField]
        private RectTransform mySubsBackButton; // Shariz 1st Nov 2019








        /// <summary>
        /// true enables sign in
        /// false enables sign up
        /// </summary>
        /// <param name="state"></param>
        public void enableSignInOrSignUpGroup(bool state)
        {
            signInGroup.SetActive(state);
            signUpGroup.SetActive(!state);


        }


        public void enableMainSignInGroup()
        {

        //shariz adding images for tablet

                if(mainSettings.device==deviceType.TABLET){
                signInImage.sprite = signInGraphicTablet;
                signUpImage.sprite = signUpGraphicTablet;
                }
                else {
                signInImage.sprite = signInGraphic;
                signUpImage.sprite = signUpGraphic;
                }

            

            Debug.Log("enable user");
            userCanvas.SetActive(true);
            mainSignInSignUpGroup.SetActive(true);
            enableSignInOrSignUpGroup(true);
            subscriptionsGroup.SetActive(false);
            resetAllText();
        }
        public void enableMySubscriptionsGroup() // Shariz 12th Nov 2019 2.01        
        {


            Debug.Log("enable subscriptions");
            userCanvas.SetActive(true);
            mainSignInSignUpGroup.SetActive(false);
            enableSignInOrSignUpGroup(true);
            subscriptionsGroup.SetActive(false);
            resetAllText();
        }


        public void resetAllText()
        {
            signInEmailX.text = null;
            signUpEmailX.text = null;
            userNameX.text = null;
            signUpEmailX.text = null;
            signUpPasswordX.text = null;
        }

        public void disableMainSignInGroup()
        {
           // Debug.Log("enable user");
            userCanvas.SetActive(false);
            mainSignInSignUpGroup.SetActive(false);
           
        }

        //shariz removing subscriptions
        public void setSignUpOrSignIn(userUIEnum user)
        {
            switch (user)
            {
                case userUIEnum.SIGNIN:
                    enableSignInOrSignUpGroup(true);
                    break;
                case userUIEnum.SIGNUP:
                    enableSignInOrSignUpGroup(false);
                    //disableSubscription(false);
                    break;
                default:
                    break;
            }

        }

        //shariz setting user type for removing subscriptions

        public void setUserType(int type)
        {
            switch (type)
            {
                case 0:
                    this.userType = "student";
                    break;
                case 1:
                    this.userType = "teacher";
                    break;
                default:
                    break;

            }
        }

        



        #region SUBSCRIPTION


        private string userType = "";

        // Shariz 26th March 2020 2.0.4
        public void setSubscriptionUI(bool isbought, int userType, string description, string subsName, string price, string lessonsCount, string expiryDate, int subID)//shariz 25th March 2020 2.0.4
        {

            alreadyBoughtArrow.SetActive(isbought);
            nonBoughtArrow.SetActive(!isbought);
            if (userType == 1)
            {
                if(mainSettings.device==deviceType.TABLET){
                subcriptionTypeImage.sprite = teacherGraphicTablet;
                }
                else {
                subcriptionTypeImage.sprite = teacherGraphic;
                }
                // Shariz 26th March 2020 2.0.4
                var seq = LeanTween.sequence();
                seq.append(LeanTween.alpha(studentButtonBg.gameObject.GetComponent<RectTransform>(),0f,0.07f).setEase(LeanTweenType.easeOutQuad));
                seq.append(LeanTween.alpha(teacherButtonBg.gameObject.GetComponent<RectTransform>(),0.52f,0.1f).setEase(LeanTweenType.easeInQuad));
                // studentButtonBg.color = deselected;
                // teacherButtonBg.color = selected;
                this.userType = "teacher";
                // Shariz 25th March 2020 2.0.4
                description = localizationProvider.provide.getLanguage().getAbodyText(1);
                description = description.Replace("\\n", "\n");
                subsDescriptionText.text = description;
                Debug.Log("sub id is "+subID);
            }
            else
            {
                if(mainSettings.device==deviceType.TABLET){
                subcriptionTypeImage.sprite = studentGraphicTablet;
                }
                else {
                subcriptionTypeImage.sprite = studentGraphic;
                }
                // Shariz 26th March 2020 2.0.4
                var seq = LeanTween.sequence();
                seq.append(LeanTween.alpha(teacherButtonBg.gameObject.GetComponent<RectTransform>(),0f,0.07f).setEase(LeanTweenType.easeOutQuad));
                seq.append(LeanTween.alpha(studentButtonBg.gameObject.GetComponent<RectTransform>(),0.52f,0.1f).setEase(LeanTweenType.easeInQuad));
                // studentButtonBg.color = selected;
                // teacherButtonBg.color = deselected;
                this.userType = "student";
                // Shariz 25th March 2020 2.0.4
                description = localizationProvider.provide.getLanguage().getAbodyText(2);
                description = description.Replace("\\n", "\n");
                subsDescriptionText.text = description;
                Debug.Log("sub id is "+subID);
            }

            
            subsNameText.text = subsName;
            subsPriceText.text = price;
            if (isbought)
            {
                // Shariz - adding free trial offer button for Sri Lanka 25th March 2020 2.0.4
                freeTrialButton.SetActive(false);

                userTypeButtonsGroup.SetActive(false);

                //Shariz 27th Oct 2019 2.00
                if (userType == 1)
                {
                subscriptionIcon.sprite = teacherGraphic;
                lessonCountText.text = localizationProvider.provide.getLanguage().getALabelText(70) + " " + lessonsCount; // Shariz 30th Oct 2019 2.00
                }
                else {
                subscriptionIcon.sprite = studentGraphic;
                }
                subscriptionName.text = subsName;

                // Shariz 30th Oct 2019 2.00
                DateTime dateofExpiry = DateTime.Parse(expiryDate);
                Debug.Log(dateofExpiry);
                //string dateExpired = dateofExpiry.ToString("dd MMM yyyy").ToUpper();
                string dateExpired = dateofExpiry.ToString("dd-MM-yyyy").ToUpper();  // Shariz 27 April 2020 2.0.5
                expiresOnDateText.text = localizationProvider.provide.getLanguage().getALabelText(71) + " " + dateExpired;
                if(mainSettings.device==deviceType.TABLET){
                mySubscriptionParent.enabled = true;
                } else {
                mySubscriptionParent.enabled = false;   
                }

                // Shariz 25th March 2020 2.0.4 
                if(subID==5 || subID==6){
                    subscriptionSubtitle.text = localizationProvider.provide.getLanguage().getAbodyText(0);
                } else {
                    subscriptionSubtitle.text = localizationProvider.provide.getLanguage().getALabelText(69);
                }
            }
            else
            {
                // Shariz - adding free trial offer button for Sri Lanka 25th March 2020 2.0.4
                if(mainSettings.country.id == 4){
                    freeTrialButton.SetActive(false); // Shariz - hiding free trial offer button for Sri Lanka 10th Ma7 2020 2.0.6
                } else {
                    freeTrialButton.SetActive(false);
                }
                userTypeButtonsGroup.SetActive(true);
            }
            subscriptionsGroup.SetActive(true);

            
        }


        public void disableSubscription(bool state)
        {
            subscriptionsGroup.SetActive(state);

        }


        #endregion

        #region SIGN IN

        public string readSignInEmail()
        {
            return signInEmailX.text;
        }

        public string readSignInPassword()
        {
            return signInPasswordX.text;
        }

        public void activeStateSignInButton(bool state)
        {
            signInButton.interactable = state;
        }

        #endregion

        #region SIGN UP
        public string readSignUpUserName()
        {
            return userNameX.text;
        }
        public string readSignUpEmail()
        {
            return signUpEmailX.text;
        }
        public string readSignUpPassword()
        {
            return signUpPasswordX.text;
        }

        public string getUserTypeSelection()
        {
            return userType;
        }

        public void activeStateSignUpButton(bool state)
        {
            signUpButton.interactable = state;
        }

        #endregion


        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            localizeSetting.setTextConfig(welcomeBackText, language.getAFont(7), language.getAButtonText(5));// Shariz 27th Oct 2019 2.00
            localizeSetting.setTextConfig(welcomeBackBodyText, language.getAFont(10), language.getAMessageText(0));
            localizeSetting.setTextConfig(signInEmailLabel, language.getAFont(9), language.getALabelText(2));
            localizeSetting.setTextConfig(signInEmail, language.getAFont(33));
            localizeSetting.setTextConfig(signInPasswordLabel, language.getAFont(9), language.getALabelText(3));
            localizeSetting.setTextConfig(signInPassword, language.getAFont(33));
            localizeSetting.setTextConfig(forgotYourPasswordText, language.getAFont(31), language.getAButtonText(22));
            localizeSetting.setTextConfig(signInButtonText, language.getAFont(37), language.getAButtonText(5));
            localizeSetting.setTextConfig(signInsignUpButtonText, language.getAFont(39), language.getAButtonText(6));
            localizeSetting.setTextConfig(dontHaveAnAccountText, language.getAFont(31), language.getALabelText(4));

            localizeSetting.setTextConfig(letsgetStartedText, language.getAFont(7), language.getAButtonText(6));// Shariz 27th Oct 2019 2.00
            // letsgetStartedText.text = "Sign up";// Shariz 28th Oct 2019 2.00 this is temporary  
            localizeSetting.setTextConfig(letsgetStartedBodyText, language.getAFont(10), language.getAMessageText(1));
            localizeSetting.setTextConfig(signUpUserNameLabel, language.getAFont(9), language.getALabelText(6));
            localizeSetting.setTextConfig(signUpUserName, language.getAFont(33));
            localizeSetting.setTextConfig(signUpEmailLabel, language.getAFont(9), language.getALabelText(2));
            localizeSetting.setTextConfig(signUpEmail, language.getAFont(33));
            localizeSetting.setTextConfig(signUpPasswordLabel, language.getAFont(9), language.getALabelText(100)); //Shariz - 29th March 2020 v2.0.4
            localizeSetting.setTextConfig(signUpPassword, language.getAFont(33));
            localizeSetting.setTextConfig(signUnButtonText, language.getAFont(36), language.getAButtonText(6));
            // signUnButtonText.text = "Sign up";// Shariz 28th Oct 2019 2.00 this is temporary
            localizeSetting.setTextConfig(signUpsignInButtonText, language.getAFont(38), language.getAButtonText(5));
            localizeSetting.setTextConfig(alreadyHaveAnAccountText, language.getAFont(31), language.getALabelText(5));

            
            // signUpPasswordLabel.text = localizationProvider.provide.getLanguage().getALabelText(3) + " (min 6 characters)";// Shariz 27th Oct 2019 2.00

            localizeSetting.setTextConfig(subsStudentButtonText, language.getAFont(11), language.getAButtonText(7));
            localizeSetting.setTextConfig(subsTeacherButtonText, language.getAFont(11), language.getAButtonText(8));
            localizeSetting.setTextConfig(subsDescriptionText, language.getAFont(12));
            
            // localizeSetting.setTextConfig(subsNameText, language.getAFont(13)); // Shariz 12th Nov 2019 2.01
            // localizeSetting.setTextConfig(subsPriceText, language.getAFont(12)); // Shariz 12th Nov 2019 2.01

            // Shariz 27th Oct 2019 2.00 adding my subscriptions page
            localizeSetting.setTextConfig(mySubscriptionsTitle, language.getAFont(0), language.getALabelText(17));
            localizeSetting.setTextConfig(yourSubscriptionsText, language.getAFont(22), language.getALabelText(68));
            localizeSetting.setTextConfig(subscriptionName, language.getAFont(21));
            localizeSetting.setTextConfig(subscriptionSubtitle, language.getAFont(17));// Shariz 25th March 2020 2.0.4
            localizeSetting.setTextConfig(lessonCountText, language.getAFont(17), language.getALabelText(70));
            localizeSetting.setTextConfig(expiresOnDateText, language.getAFont(22), language.getALabelText(71));
            localizeSetting.setTextConfig(subscriptionMessage, language.getAFont(6), language.getALabelText(72));

            // Shariz 25th March 2020 2.0.4
            localizeSetting.setTextConfig(freeTrialButtonText, language.getAFont(50), language.getAbodyText(0));
            freeTrialButtonText.text = freeTrialButtonText.text+" <sprite=\"FontAwesome\" name=\"chevronright\">";

            //shariz 16th Jan 2023 2.0.10 setting text for dropdown
            userTypeSelect.options.Clear();
            userTypeSelect.options.Add(new TMP_Dropdown.OptionData(language.getAButtonText(7)));
            userTypeSelect.options.Add(new TMP_Dropdown.OptionData(language.getAButtonText(8)));
            localizeSetting.setDropdownTextConfig(userTypeSelect, language.getAFont(10));
        }


        //shariz 12/9/2019 v2
        public override void setMargins(float titleMargin, float bodyHeight, float footerMargin, float rightMargin, float leftMargin){
            if(footerMargin >0 && signUpFooter != null)
            {
                signUpFooter.sizeDelta = new Vector2(signUpFooter.sizeDelta.x, signUpFooter.sizeDelta.y + footerMargin);
            }
            if(footerMargin >0 && signInFooter != null)
            {
                signInFooter.sizeDelta = new Vector2(signInFooter.sizeDelta.x, signInFooter.sizeDelta.y + footerMargin);
            }
            if(footerMargin >0 && subscriptionIntroFooter != null)
            {
                subscriptionIntroFooter.sizeDelta = new Vector2(subscriptionIntroFooter.sizeDelta.x, subscriptionIntroFooter.sizeDelta.y + footerMargin);
            }

            // shariz 1st nov make back button responsive
            if(mainSettings.device == deviceType.IPHONEX)
            {
                mySubsBackButton.sizeDelta = new Vector2(mySubsBackButton.sizeDelta.x,mySubsBackButton.sizeDelta.y+30);
                mySubsBackButton.offsetMax = new Vector2(mySubsBackButton.offsetMax.x,mySubsBackButton.offsetMax.y-30);
            }
        }
    }
}
