using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 August 2019
    /// Modified by: 
    /// quiz ui 
    /// </summary>
    public class quizUIElement : iUILayoutBase, IPrefabLocalize
    {
        [Header("FINISHED GROUP")]
        [SerializeField]
        private GameObject finishedGroup;
        [SerializeField]
        private starsUIElement finishedStarsGroup;
        // [SerializeField]
        // private TextMeshProUGUI highScoreText;
        // [SerializeField]
        // private TextMeshProUGUI timeText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private Button replayButton;        
        [SerializeField]
        private Button doneButton;
        [Header("PREVIOUSE SCORE GROUP")]
        [SerializeField]
        private GameObject PreviousScoreGroup;
        [SerializeField]
        private starsUIElement previousStarsGroup;
        [SerializeField]
        private TextMeshProUGUI previousScoreText;
        [Header("START GROUP")]
        [SerializeField]
        private GameObject startGroup;
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI DescriptionText;
        

        //Adding previous score to Finished window
        // [SerializeField]
        // private TextMeshProUGUI previousScoreTextShow;        
        // [SerializeField]
        // private GameObject previousScoreTextShowGroup;        


        //Shariz adding object view ar view to quiz too
        [SerializeField]
        private GameObject arModeGraphic;
        [SerializeField]
        private GameObject objectModeGraphic;

        //shariz v2 1/9/2019
        [SerializeField]
        private string YourScore;
        // [SerializeField]
        // private string CompletedThisTaskWithHighScore;
        // [SerializeField]
        // private string CompletedThisTask;
        [SerializeField]
        private string ScorePrefix;
        [SerializeField]
        private string PreviousScorePrefix;
    
        //shariz v2 13/9/2019
        
        [SerializeField]
        private GameObject HighScoreObject;
  
        [SerializeField]
        private TextMeshProUGUI scoreNumber;

        [SerializeField]
        private GameObject darkBG;

        [SerializeField]
        private GameObject mainReplayButton;

        public GameObject WrongAnswerWindow;
        public GameObject RetryWindowIcon;



        // shariz changing displayed ui a little
        public void showFinishedWindow(bool isHighScore,int starCount, float time, int score, params UnityAction[] actions)
        {
            darkBG.SetActive(true);
            finishedGroup.SetActive(true);
            mainReplayButton.SetActive(false);
            finishedStarsGroup.setStarCount(starCount);
            // timeText.text = "time: " + time + " seconds";
            scoreText.text = YourScore;
            scoreNumber.text = score.ToString();
            if(isHighScore)
            {
                HighScoreObject.SetActive(true);
                // highScoreText.text = CompletedThisTaskWithHighScore;
                // previousScoreTextShowGroup.SetActive(true);
            }
            else
            {
                HighScoreObject.SetActive(false);
                // highScoreText.text = CompletedThisTask;
                // previousScoreTextShowGroup.SetActive(false);
            }

            replayButton.onClick.RemoveAllListeners();
            doneButton.onClick.RemoveAllListeners();
            replayButton.onClick.AddListener(actions[0]);
            replayButton.onClick.AddListener(closeFinishedWindow);
            doneButton.onClick.AddListener(actions[1]);
            doneButton.onClick.AddListener(closeFinishedWindow);
            

        }


        public void showStartWindow( string title, string description)
        {
            titleText.text = title;
            DescriptionText.text = description;
            setStateStartGroup(true);
            finishedGroup.SetActive(false);//shariz v2 18/9/2019
            darkBG.SetActive(false);//shariz v2 18/9/2019
            mainReplayButton.SetActive(true);//shariz v2 18/9/2019
        }

        public void setStateStartGroup(bool state)
        {
            startGroup.SetActive(state);
        }


        //Shariz removed time from previous score
        public void showPreviousScore(int starCount, float time, int score)
        {
            PreviousScoreGroup.SetActive(true);
            previousStarsGroup.setStarCount(starCount);
            previousScoreText.text = ScorePrefix + score;
            // previousScoreTextShow.text = PreviousScorePrefix + score;
        }

        void closeFinishedWindow()
        {
            finishedGroup.SetActive(false);
            darkBG.SetActive(false);
            mainReplayButton.SetActive(true);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
        }

        // shariz switching object / ar button
        public void changeArMode(bool arMode)
        {
            arModeGraphic.SetActive(arMode);
            objectModeGraphic.SetActive(!arMode);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        //shariz v2 1/9/2019
         public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            // localizeSetting.setTextConfig(YourScore, language.getAFont(20));
            YourScore = language.getAMessageText(6);
            // CompletedThisTaskWithHighScore = language.getAMessageText(2);
            // CompletedThisTask = language.getAMessageText(3);
            ScorePrefix = language.getAMessageText(4);
            PreviousScorePrefix = language.getAMessageText(5);

            // localizeSetting.setTextConfig(highScoreText, language.getAFont(25));
            localizeSetting.setTextConfig(scoreText, language.getAFont(25));
            localizeSetting.setTextConfig(previousScoreText, language.getAFont(30));
            localizeSetting.setTextConfig(titleText, language.getAFont(42));
            localizeSetting.setTextConfig(DescriptionText, language.getAFont(42));
            // localizeSetting.setTextConfig(previousScoreTextShow, language.getAFont(3));


        }

        public void showWrongAnswerWindow(){
            Animator wrongAnswer_Animator = WrongAnswerWindow.GetComponent<Animator>();
            wrongAnswer_Animator.SetTrigger("Wrong");
        }

        public void showRetryWindow(){
            Animator retry_Animator = RetryWindowIcon.GetComponent<Animator>();
            retry_Animator.SetTrigger("Retry");
        }

        

    }
}
