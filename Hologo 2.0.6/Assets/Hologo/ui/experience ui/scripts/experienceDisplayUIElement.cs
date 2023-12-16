using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// class that localizes the ui of experience slides and handles the functions
    /// </summary>
    public class experienceDisplayUIElement : iUILayoutBase, IPrefabLocalize
    {

        [Header("UI ELEMENTS")]
        [SerializeField]
        private GameObject centerPlayPauseGroup;
        [SerializeField]
        private GameObject centerPlayGraphic;
        [SerializeField]
        private GameObject centerPauseGrapic;
        [SerializeField]
        private GameObject exploreButton;
        [SerializeField]
        private GameObject learnButton;
        [SerializeField]
        private Slider audioSlider;
        [SerializeField]
        private GameObject studentsteacherButtonsGroup;
        [SerializeField]
        private GameObject recordingGroup;
        [SerializeField]
        public GameObject sidePlayPauseGroup;
        [SerializeField]
        private GameObject LessonsGetGroup;
        [SerializeField]
        private GameObject recordGraphic;
        [SerializeField]
        private GameObject recordingInSessonGraphic;
        [SerializeField]
        private GameObject sidePlayGraphic;
        [SerializeField]
        private GameObject sidePauseGraphic;
        [SerializeField]
        private TextMeshProUGUI timer;
        [SerializeField]
        private GameObject lessonsGetGraphic;
        [SerializeField]
        private GameObject lessonDownLoadIndicatorGroup;
        [SerializeField]
        private Image downloadFillCircle;
        [SerializeField]
        private GameObject quizButton;
        [SerializeField]
        private GameObject arModeButton;
        [SerializeField]
        private GameObject arModeGraphic;
        [SerializeField]
        private GameObject objectModeGraphic;
        [SerializeField]
        private RectTransform arButtonSize;
        // new additon to ui
        [SerializeField]
        private GameObject learnExploreButtons;
        [SerializeField]
        private GameObject learnStepButtonsGroup;
        [SerializeField]
        private GameObject exploreStepButtonsGroup;
        [SerializeField]
        private GameObject labelButtonsGroup;
        [SerializeField]
        private GameObject blur;

        [SerializeField]
        private GameObject ARMenu;//Shariz 27th Oct 2019 v2
        



        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            // localizeSetting.setTextConfig(exploreText, language.getAFont(5), language.getAButtonText(3));
            //localizeSetting.setTextConfig(learnText, language.getAFont(5), language.getAButtonText(4));
        }


        public void disableArButton()
        {
            arButtonSize.sizeDelta = new Vector2(85f, arButtonSize.sizeDelta.y);
            arModeButton.SetActive(false);

        }

        public void enableArButton()
        {
            arButtonSize.sizeDelta = new Vector2(170f, arButtonSize.sizeDelta.y);
            arModeButton.SetActive(true);
        }
        /// <summary>
        /// true will set explore button true
        /// </summary>
        /// <param name="state"></param>
        public void ToggleLearnExploreMode(bool state)
        {
            exploreButton.SetActive(state);

            learnButton.SetActive(!state);
        }

        /// <summary>
        /// if true then ar is enabled and object is disabled
        /// </summary>
        /// <param name="arMode"></param>
        public void changeArMode(bool arMode)
        {
            arModeGraphic.SetActive(arMode);
            objectModeGraphic.SetActive(!arMode);
        }


        public void hideArModeChangeButton()
        {
            arModeButton.SetActive(false);
        }

        // auto toggle of play and pause
        // Shariz 7th March 2020 2.0.4
        public void playPauseToggle(bool play)
        {
            Debug.Log("Is in play: "+play);
            if (play)
            {
                centerPlayGraphic.SetActive(false);
                centerPauseGrapic.SetActive(true);
            }
            else
            {
                centerPlayGraphic.SetActive(true);
                centerPauseGrapic.SetActive(false);
            }
        }

        // manual setting of play and pause
        public void isPlaying(bool state)
        {
            if (state)
            {
                centerPlayGraphic.SetActive(false);
                centerPauseGrapic.SetActive(true);
            }
            else
            {
                centerPlayGraphic.SetActive(true);
                centerPauseGrapic.SetActive(false);
            }
        }


        public void audioSliderStart(float length)
        {

            audioSlider.gameObject.SetActive(true);
            audioSlider.direction = Slider.Direction.LeftToRight;
            audioSlider.minValue = 0;
            audioSlider.maxValue = length;
        }

        public void audioSliderStop()
        {

            audioSlider.gameObject.SetActive(false);
            audioSlider.value = 0;

        }

        public void audioSliderUpdate(float time)
        {
            audioSlider.value = time;

        }

        public void makeUIForPortal()
        {
            LessonsGetGroup.SetActive(false);
            recordingGroup.SetActive(false);
            setStateSidePlayPauseGroup(false);
            learnExploreButtons.SetActive(false);
            learnStepButtonsGroup.SetActive(false);
            exploreStepButtonsGroup.SetActive(false);
            labelButtonsGroup.SetActive(false);
            blur.SetActive(false);
            ARMenu.SetActive(false);//Shariz 27th Oct 2019 v2
        }

        public void makeUIForStudents()
        {
            recordingGroup.SetActive(false);
            sidePlayPauseGroup.SetActive(true);
      //      setStateSidePlayPauseGroup(false);
        }

        public void makeUIForTeachers()
        {
            LessonsGetGroup.SetActive(true);
            recordingGroup.SetActive(true);
//            setStateSidePlayPauseGroup(false);
        }

        public void makeUIForAnonymous()
        {
            LessonsGetGroup.SetActive(false);
            recordingGroup.SetActive(false);
            setStateSidePlayPauseGroup(false);
        }

        public void setStateSidePlayPauseGroup(bool state)
        {
            Debug.Log("called this");
            sidePlayPauseGroup.SetActive(state);
        }

        public void recordingGraphicsUpdate(bool state)
        {
            recordGraphic.SetActive(!state);
            recordingInSessonGraphic.SetActive(state);
        }

        public void downloadGroupActiveStatus(bool state)
        {
            lessonsGetGraphic.SetActive(!state);
            lessonDownLoadIndicatorGroup.SetActive(state);
        }

        public void playPauseGraphicsUpdate(bool state)
        {
            sidePlayGraphic.SetActive(!state);
            sidePauseGraphic.SetActive(state);
        }

        public void timerDisplaySetState(bool state)
        {
            timer.gameObject.SetActive(state);
            // setting timer to zero
            if (state == false)
            {
                timer.text = "0.00";
            }
        }

        public void timerDisplayUpdate(float time)
        {
            timer.text = time.ToString("0.00");
        }

        public void downloadSliderUpdate(float value)
        {
            downloadFillCircle.fillAmount = value;
        }

        public void setActiveStateStudentsteacherButtonsGroup(bool state, bool user)
        {
            studentsteacherButtonsGroup.SetActive(state);
            centerPlayPauseGroup.SetActive(!state);
        }


        public void quizButtonSetState(bool state)
        {
            quizButton.SetActive(state);
        }
    }
}
