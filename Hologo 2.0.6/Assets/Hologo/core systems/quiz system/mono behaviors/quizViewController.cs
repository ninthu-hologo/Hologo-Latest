using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 August 2019
    /// Modified by: 
    /// controlls quiz views and progression
    /// </summary>
    public class quizViewController : MonoBehaviour
    {

        // [Header("Assetbundle Data")]
        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        private quiz_SO currentQuiz;
        private experienceLocalizationData_SO experienceLocalizedData;
        [Header("UI Elements")]
        [SerializeField]
        private arSwitcher experienceMainUIElement;
        [SerializeField]
        private arSwitcher quizUI;
        [Header("MONO BEHAVIORS")]
        [SerializeField]
        private quizInstantiator qInstantiator;
        [SerializeField]
        private quizManager qManager;
        [SerializeField]
        private setColliderSize colliderSize;
        [Header("EVENTS")]
        [SerializeField]
        private event_SO quizEvent;

        // number of valid matches made
        int matchCount;
        quizLabelId label;
        quizLabelId origin;

        float timeTakenToFinishQuiz = 0f;
        int numberOfAttempts = 1;

        bool isQuizStarted = false;
        [SerializeField]
        private quizScoreData previousScore;

        private int quizType = 0;



        // public methods
        public void initiateQuizViewController(quiz_SO quiz, experienceLocalizationData_SO localizeData)
        {
            Debug.Log("initiate quiz view controller");

            if (quiz == null)
            {
                // no quiz for this experience
                // disable quiz button and return
                experienceMainUIElement.GetExperienceDisplayUIElementDeviceWithoutHiding().quizButtonSetState(false);
                Debug.Log("quiz is null so quiz state button set");
                return;
            }

            experienceMainUIElement.GetExperienceDisplayUIElementDeviceWithoutHiding().quizButtonSetState(true);
            currentQuiz = quiz;
            quizType = currentQuiz.quizType;
            experienceLocalizedData = localizeData;

            //getting scores from storage
            qManager.loadQuizScores();

            // getting the previous score for the current experience
            previousScore = qManager.getScore(mainSettings.currentExperience.id);

            qInstantiator.MakeQuiz(currentQuiz, experienceLocalizedData.quizSlides, mainSettings.getLabelSize());
            colliderSize.setCollider(qInstantiator.giveQuizModel());
        }

        // start quiz button event
        public void startQuiz()
        {
            // enable quiz slide
            // start timer
            // reset quiz attempt counter
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            // quiz event raised 
            quizEvent.raiseBoolEvent(true);
            //set quiz ui active
            quizUI.GetquizUI().gameObject.SetActive(true);
            quizUI.GetquizUI().showStartWindow("", experienceLocalizedData.quizSlides[0].LocaliziedTitle);
            // check if there is a previous score. display if 
            if (previousScore != null)
            {
                quizUI.GetquizUI().showPreviousScore(previousScore.starCount, previousScore.finishedTime, previousScore.score);
            }
            // set quiz model to active state
            qInstantiator.setQuizModelActiveState(true);
        }


        public void QuizGo()
        {
            numberOfAttempts = 0;
            timeTakenToFinishQuiz = 0f;
            matchCount = 0;
            ///
            resetQuiz();
            isQuizStarted = true;
            quizUI.GetquizUI().setStateStartGroup(false);
        }

        // replay quiz button event
        public void replayQuiz()
        {
            Debug.Log("trying to replay quiz");
            quizUI.GetquizUI().showRetryWindow();
            resetQuiz();
            // start timer
            // reset quiz attempt counter
            numberOfAttempts = 0;
            timeTakenToFinishQuiz = 0f;
            isQuizStarted = true;
        }

        // quit quiz button event
        public void quitQuiz()
        {
            // disable quiz slide and back to explore mode
            resetQuiz();
            isQuizStarted = false;
            // quiz event raised 
            quizEvent.raiseBoolEvent(false);
            quizUI.GetquizUI().gameObject.SetActive(false);
            // setting the parents back to model 

            // hide quiz model
            qInstantiator.setQuizModelActiveState(false);
        }


        void resetQuiz()
        {
            numberOfAttempts = 0;
            timeTakenToFinishQuiz = 0f;
            matchCount = 0;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            switch (quizType)
            {
                case 1:
                    // reset the labels to spawn points
                    qInstantiator.wrongMatchMadeResult();
                    // resuffle the labels
                    qInstantiator.reSuffleQuiz();
                    break;
                case 2:
                    // setting the pick me models to random spawn points
                    qInstantiator.SetupPickMeQuiz();
                    break;
                default:
                    break;
            }
        }

        #region QUIZ TYPE LABEL MATCH PLAY METHODS




        // update methof for keeping track of time\
        private void Update()
        {
            if (isQuizStarted)
            {
                // start timer
                timeTakenToFinishQuiz += Time.deltaTime;
            }
        }



        // here we get the touched object and see if its a label or a point
        // and assign it to the respective var.
        // this method is applied to the player interaction click label listener
        public void GetTouchedLabelOrOrigin(GameObject go)
        {
            if (!isQuizStarted)
                return;

            quizLabelId qId = go.GetComponent<quizLabelId>();

            if (qId == null)
            {
                qId = go.GetComponentInParent<quizLabelId>();

                // return if label is already matched
                if (qId.done == true)
                    return;


                if (label != null)
                    return;

                label = qId;
                qId.selected();
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            }
            else
            {
                // return if label is already matched
                if (qId.done == true)
                    return;

                if (origin != null)
                    return;

                origin = qId;
                qId.selected();
            }

            checkMatch();
        }

        // checking for a valid match
        void checkMatch()
        {
            if (origin != null && label != null)
            {
                if (origin.id == label.id)
                {
                    // match success
                    matchCount++;
                    Debug.Log("matched");
                    origin.done = true;
                    label.done = true;
                    qInstantiator.matchMadeResult(origin.id);
                    checkIfAllMatchesHaveBeenMade();
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                }
                else
                {
                    //match unsuccessfull
                    // Quiz Over
                    matchCount = 0;
                    Debug.Log("failed");
                    numberOfAttempts++;
                    qInstantiator.wrongMatchMadeResult();
                    quizUI.GetquizUI().showWrongAnswerWindow();
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                }

                origin = null;
                label = null;
            }
        }




        // check for all the matches have been done. by comparing the match count and label count
        void checkIfAllMatchesHaveBeenMade()
        {
            if (qInstantiator.doesCountMatch(matchCount))
            {
                // quiz is done. and now give stars
                Debug.Log("all matches are made! welldone");
                // adding score to quiz score list
                isQuizStarted = false;
                // make score and save
                // when adding score check if this time score is better than last score.
                // if so alert new high score else dont overwrite the score and alert
                // the user that to do better
                quizScoreData score = newQuizScore();
                bool highscore = qManager.addScore(score);
                quizUI.GetquizUI().showFinishedWindow(highscore, score.starCount, score.finishedTime, score.score, replayQuiz, quitQuiz);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00 
                SoundManager.instance.PlaySoundFromList(1);// Shariz 2nd Nov 2019 2.00 Game win sound
            }
        }

        #endregion


        #region QUIZ TYPE PICK ME PLAY METHODS
        // here we get the touched object and see if its a the right pick me
        public void GetTouchedPick(GameObject go)
        {
            if (!isQuizStarted)
                return;

            quizLabelId qId = go.GetComponent<quizLabelId>();

            if (qId.id == 1)
            {
                // picked the right one
                // quiz is done. and now give stars
                Debug.Log("all matches are made! welldone");
                // adding score to quiz score list
                isQuizStarted = false;
                // make score and save
                // when adding score check if this time score is better than last score.
                // if so alert new high score else dont overwrite the score and alert
                // the user that to do better
                quizScoreData score = newQuizScore();
                bool highscore = qManager.addScore(score);
                quizUI.GetquizUI().showFinishedWindow(highscore, score.starCount, score.finishedTime, score.score, replayQuiz, quitQuiz);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(1);// Shariz 2nd Nov 2019 2.00 Game Win Sound
            }
            else
            {
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                quizUI.GetquizUI().showWrongAnswerWindow();
                numberOfAttempts++;
                qInstantiator.SetupPickMeQuiz();
            }


        }
        #endregion
        // making a new score
        quizScoreData newQuizScore()
        {
            quizScoreData qsd = new quizScoreData();
            qsd.quizName = mainSettings.currentExperience.title;
            qsd.expereinceId = mainSettings.currentExperience.id;
            qsd.finishedTime = timeTakenToFinishQuiz;
            qsd.totalAttempsPerSesson = numberOfAttempts;
            // calculating number of stars achieved.
            qsd.starCount = mainSettings.currentExperience.calculateStarsAchieved(timeTakenToFinishQuiz, numberOfAttempts);
            qsd.calculateScore();
            return qsd;
        }
    }
}