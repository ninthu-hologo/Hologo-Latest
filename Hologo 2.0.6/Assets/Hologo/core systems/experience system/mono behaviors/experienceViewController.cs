using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 16 july 2019
    /// Modified by: 
    /// class that makes all the experience model stuff
    /// </summary>s
    public class experienceViewController : messageLogging
    {
        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private bool testMode;
        [SerializeField]
        private event_SO myEvent;
        [SerializeField]
        private languageData_SO currentLanguage;
        [SerializeField]
        [Header("Assetbundle Data")]
        private experienceData_SO currentExperience;
        [SerializeField]
        private experienceLocalizationData_SO experienceLocalizedData;
        [SerializeField]
        private quiz_SO currentQuiz;
        [Header("UI Elements")]
        [SerializeField]
        private arSwitcher experienceMainUIElement;
        [Header("Scene Objects")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private GameObject screenfader;
        


        [Header("MONO BEHAVIORS")]
        [SerializeField]
        private experienceInstantiator experienceInstantiate;
        [SerializeField]
        private portalInstantiator pInstantiator;
        [SerializeField]
        private experienceUIConnect expUIConnect;
        [SerializeField]
        private experienceAssetManager eAManager;
        [SerializeField]
        private setColliderSize setCollider;
        [SerializeField]
        private arViewController arVController;

        // keeps track of which mode we are in
        private ExperienceSlideMode mode;
        // current active slide gameobject
        private GameObject currentSlide;
        // index of current playing narrations slides index in list
        private int learningIndex = 0;
        // a check if the user has clicked play and its already in playing mode
        // if so we just puase and play instead of starting from new
        private bool isInPlay = false;
        //script of experienc ui keeping store here
        private float audioCutOff = 0.1f;
      //  private bool isAssetLoading = false;
        private bool isLabelsOn = false; //Shariz 26th Feb 2020 v2.0.4 


        public void initiateExperienceViewController()
        {
            if (!testMode)
            {
                currentExperience = eAManager.getCurrentExperience();
                experienceLocalizedData = eAManager.getCurrentExperienceLocalizedData();
                currentQuiz = eAManager.getCurrentQuiz();
            }

            // setting experience model specifics for ar display
            arVController.RecieveArFoundationSpecifisFromExperience(mainSettings.isArSupported, currentExperience.enableShadow, currentExperience.objectModeScale
                , currentExperience.arModeScale, currentExperience.backgroundColor, currentExperience.modelType);

            if(currentExperience.modelType == 1)
            {
                // instantiating slide models and localizing them
                experienceInstantiate.makeNormalExperience(currentExperience, currentExperience.Models, experienceLocalizedData.giveSlides(), mainSettings.labelsAlwaysOn, mainSettings.getLabelSize());
                // creating explore and learn buttons

                expUIConnect.makeLearnAndExploreStepButtons(currentExperience.Models, experienceLocalizedData.giveSlides(), clickStepSlideButton);

                changeToLearnMode();

                //Shariz 26th Feb 2020 v2.0.4
                if(mainSettings.isArSupported){
                    Debug.Log("AR is supported so initializing in AR Mode");
                    arVController.ToggleObjectArMode();
                } else
                {
                    Debug.Log("AR is not supported so initializing in Object Mode");

                }


                // experienceLabeler el = currentSlide.GetComponent<experienceLabeler>(); //Shariz 26th Feb 2020 v2.0.4 
                // el.MyLabelsEnable(false);//Shariz 26th Feb 2020 v2.0.4
            }
            else
            {
                pInstantiator.makePortalExpereince(currentExperience);
                expUIConnect.makeUIForPortal();
               // setCollider.setCollider(pInstantiator.getPortalCollider());

                // hide all recorde/play buttons and enable only enter button in object mode.
            }
            
        }

        // assigned to the step button to advance to next slide
        // also if in learn mode it will start the narration;
        void clickStepSlideButton(int id)
        {
           
            // when user clicks a step if its in learn mode
            // narration start to play for that slide
            switch (mode)
            {
                case ExperienceSlideMode.EXPLORE:
                    advanceStep(id,true);
                    break;
                case ExperienceSlideMode.LEARN:
                    advanceStep(id, false);
                    learningIndex = id;
                    StopAllCoroutines();
                    isInPlay = false;
                    startLearning();
                    break;
                default:
                    break;
            }
           
        }

        // toggle label visibility
        public void toggleLabels()
        {
            if (isLabelsOn)
            {
                isLabelsOn = false;

            }
            else
            {
                isLabelsOn = true;
            }
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
			SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
			setLabelVisibility();
        }

        private void setLabelVisibility()
        {
            experienceLabeler el = currentSlide.GetComponent<experienceLabeler>();
            if (el != null)
            {
                el.MyLabelsEnable(isLabelsOn);
            }

          
        }

        public void stopAudio()
        {
            togglePlayMode(false);// Shariz 7th March 2020 2.0.4
            audioSource.Stop();
        }

        public experienceData_SO getExperienceData()
        {
            return currentExperience;
        }

        public experienceLocalizationData_SO getExperienceLocalizationData()
        {
            return experienceLocalizedData;
        }

        // for advancing a step;
        void advanceStep(int id , bool isExp)
        {
            expUIConnect.activateCurrentSlideButton(id, isExp);
            Debug.Log("Current active slide ID is "+id);
            currentSlide = experienceInstantiate.changeActiveSlide(id);
            setCollider.setCollider(currentSlide);
            setLabelVisibility();
        }

 

        // assigned to the button
        public void changeToExploreMode()
        {
             changeExperienceSlideMode(ExperienceSlideMode.EXPLORE);
            
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderStop();
            StopAllCoroutines();
            audioSource.clip = null;
            isInPlay = false;
            //event
            raiseExploreLearnModeChangeEvent();
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().ToggleLearnExploreMode(true);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
			SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
		}

        // assigned to the button
        public void changeToLearnMode()
        {
            togglePlayMode(false);// Shariz 7th March 2020 2.0.4
            changeExperienceSlideMode(ExperienceSlideMode.LEARN);
            //event
            raiseExploreLearnModeChangeEvent();
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().ToggleLearnExploreMode(false);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
			SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
		}

        public ExperienceSlideMode getCurrentMode()
        {
            return mode;
        }

        #region EVENT METHODS
        void raiseExploreLearnModeChangeEvent()
        {
            // raise mode event
            myEvent.raise(this.gameObject);
        }

        public void quizStartOrDone(bool state)
        {
            // hide ar ui if true and show ar ui if false
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().gameObject.SetActive(!state);
            experienceInstantiate.setSlidesActiveState(!state);
        }
        #endregion

        // changing the mode between explore and learn
        public void changeExperienceSlideMode(ExperienceSlideMode mode)
        {
            this.mode = mode;
            currentSlide = experienceInstantiate.changeSlideMode(mode);
            setCollider.setCollider(currentSlide);
            expUIConnect.changeSlideMode(mode);
            setLabelVisibility();
        }

        // assigned to the play button for learning 
        // Shariz 7th March 2020 2.0.4
        public void startLearning()
        {
            if (isInPlay)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                    togglePlayMode(false);// Shariz 7th March 2020 2.0.4
                }
                else
                {
                    if (audioSource.time != 0)
                    {
                        audioSource.Play();
                        togglePlayMode(true);// Shariz 7th March 2020 2.0.4
                    }
                }
            }
            else
            {
                learnExecute();
                togglePlayMode(true);// Shariz 7th March 2020 2.0.4
            }
        }

        private void togglePlayMode(bool play) // Shariz 7th March 2020 2.0.4
        {
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseToggle(play); // Shariz 7th March 2020 2.0.4
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
			SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
		}

        // calculating the next step to play
        void learnExecute()
        {
            if (mode == ExperienceSlideMode.EXPLORE)
                return;

            if (learningIndex >= currentExperience.Models.Count)
            {
                learningIndex = 0;
                // check to see if experience is in learn loop mode.
                if(!currentExperience.learnLoopMode)
                {
                    return;
                }
            }

            slideDetails sd = currentExperience.Models[learningIndex];

            if (sd.isInLearnMode)
            {
                // advancing to next slide and play narration
                advanceStep(learningIndex,false);// Shariz 7th March 2020 2.0.4
                StartCoroutine(playStep());
            }
            else
            {
                // if this step is not in learn we increase the count and run this method again
                learningIndex++;
                learnExecute();
            }
        }


        // audio narration player will advance to next step after finishing current slides audio
        IEnumerator playStep()
        {
            isInPlay = true;
            slideDetails sd = currentExperience.Models[learningIndex];
            audioSource.clip = sd.narration;
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderStart(sd.narration.length);
            audioSource.Play();
            // awaiting while the audio clip is not done
            while (audioSource.time < audioSource.clip.length- audioCutOff)
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderUpdate(audioSource.time);
                yield return null;
            }
            //go to next step
            learningIndex++;
            learnExecute();
            yield return null;
        }


        public void quitArScene()
        {
            screenfader.SetActive(true);
            arVController.disableArSession();
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);// Shariz 17th Oct 2019 2.00
			SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
		}


        public void unloadAssetBundle()
        {
            // shariz oct 27 when this IF is not used, app freezes when trying to go back from an empty or broken exprience. 2.00
            if(currentExperience){
                if(currentExperience.modelType == 1)
                {
                    experienceInstantiate.cleanUpExperience();
                }
                else
                {
                    pInstantiator.cleanUpPortal();
                }
            }
            eAManager.unloadExperience();
        }



        public void goInOrOutOfPortal(GameObject go)
        {
            pInstantiator.GoInOrOutOfPortal(go);
        }

    }

    public enum ExperienceSlideMode
    {
        EXPLORE,
        LEARN,
    }
}
