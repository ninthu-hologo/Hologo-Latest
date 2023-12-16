using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System;


namespace Hologo2
{
    
    /// <summary>
    /// Created by: Shariz - 22 Oct 2019
    /// Modified by: 
    /// quiz ui 
    /// </summary>

    public class onboardingPages : MonoBehaviour
    {

        // [SerializeField]
        // private localizePrefab_SO onboardingSlide;

        [SerializeField]
        private List<localizePrefab_SO> onboardingSlides = new List<localizePrefab_SO>();

        [SerializeField]
        private GameObject onboardingPanel;        
        
        [SerializeField]
        private int numberOfSlides;

        [SerializeField]
        private int currentSlide;
                
        [SerializeField]
        private List<GameObject> currentOnboardingSlides = new List<GameObject>();

        public RectTransform window;

        public CanvasGroup windowCanvas;

        public settings_SO main_settings;


        [SerializeField]
        private experienceAssetManager eAManager;


        // Start whole process here
        private void Start()
        {
            // initiateOnboarding();
            
        }

        // Initiate onboarding. set current slide as first slide and open onboarding window
        public void initiateOnboarding(){
            if((main_settings.onboardingOn==true) && (eAManager.getCurrentExperience().modelType == 1)){
                currentSlide = 0;
                openOnboarding();
                StartCoroutine(makeOnboardingSlide(onboardingSlides));
            }
        }


        
        // Creating of slides for onboarding with localization
        IEnumerator makeOnboardingSlide(List<localizePrefab_SO> onboardingSlides)
        {
            currentSlide = 0;
            for (int i = 0; i < onboardingSlides.Count; i++)
            {
                    GameObject go = Instantiate(onboardingSlides[i].givePrefab());
                    go.transform.SetParent(onboardingPanel.transform);
                    go.transform.localScale = new Vector3(1, 1, 1);
                    RectTransform rt = go.GetComponent<RectTransform>();
                    rt.offsetMin = rt.offsetMax = Vector2.zero ;

                    // Deactivate all slides other than first slide
                    if(i != 0){
                        go.SetActive(false);
                    }

                    // hide previous button in first slide
                    if(i == 0){
                        go.GetComponent<onboardingSlides>().getPrevButton().gameObject.SetActive(false);
                    }
                    // in last slide, hide next button and skip button, and show close button.
                    if(i == numberOfSlides-1){
                        go.GetComponent<onboardingSlides>().getNextButton().gameObject.SetActive(false);
                        go.GetComponent<onboardingSlides>().getCloseButton().gameObject.SetActive(true);
                        go.GetComponent<onboardingSlides>().getCloseButton().onClick.AddListener(closeOnboarding);
                        go.GetComponent<onboardingSlides>().getSkipButton().gameObject.SetActive(false);
                    }
                    // show next button and skip button and hide close button for all slides other than last slide
                    if(i != numberOfSlides-1){
                    go.GetComponent<onboardingSlides>().getNextButton().onClick.AddListener(goNextSlide);
                    go.GetComponent<onboardingSlides>().getCloseButton().gameObject.SetActive(false);
                    go.GetComponent<onboardingSlides>().getSkipButton().gameObject.SetActive(true);
                    go.GetComponent<onboardingSlides>().getSkipButton().onClick.AddListener(skipToLastSlide);
                    }
                    // add previous slide method to all except first slide
                    if(i != 0){
                        go.GetComponent<onboardingSlides>().getPrevButton().onClick.AddListener(goPreviousSlide);
                    }

                    // add all created slides to active list
                   currentOnboardingSlides.Add(go);

                yield return null;
            }
            
        }

        // skip to last slide 
        public void skipToLastSlide(){
            currentSlide = numberOfSlides-1;
            changeOnboardingSlide(currentOnboardingSlides);
        }

        // go to next slide by incrementing current slide plus calling change slide method
        public void goNextSlide(){
            currentSlide += 1;
            changeOnboardingSlide(currentOnboardingSlides);
        }

        // go to previous slide by decrementing current slide plus calling change slide method
        public void goPreviousSlide(){
            currentSlide -= 1;
            changeOnboardingSlide(currentOnboardingSlides);
        }

        // open the window for onboarding
        public void openOnboarding(){
            window.gameObject.SetActive(true);
    
            LeanTween.scale(window, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 1f, 0.3f).setEase(LeanTweenType.easeInBack);
        }

        // close the window for onboarding
        public void closeOnboarding(){
            currentSlide = 0;
            destroyInstantiatedSlides(currentOnboardingSlides);
            currentOnboardingSlides.Clear();

            LeanTween.scale(window, new Vector3(0.6f, 0.6f, 1), 0.3f).setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(windowCanvas, 0f, 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(CleanWindow);
            main_settings.onboardingOn = false;
        }

        // destroy created onboarding slides
        void destroyInstantiatedSlides(List<GameObject> currentOnboardingSlides){
            for(int i = 0; i < currentOnboardingSlides.Count; i++)
                {
                    Destroy(currentOnboardingSlides[i]);
                }
        }

        // exit onboarding
        void CleanWindow(){
            window.gameObject.SetActive(false);
        }

        // change onboarding slide
        public void changeOnboardingSlide(List<GameObject> currentOnboardingSlides){
            for (int i = 0; i < currentOnboardingSlides.Count; i++)
            {
                if(i != currentSlide){
                LeanTween.alphaCanvas(currentOnboardingSlides[i].GetComponent<CanvasGroup>(), 0f, 0.15f).setEase(LeanTweenType.easeInBack);
                // LeanTween.delayedCall(currentOnboardingSlides[i], 0.15f,()=>currentOnboardingSlides[i].SetActive(false));
                currentOnboardingSlides[i].SetActive(false);
                }
                if(i == currentSlide){
                    currentOnboardingSlides[i].SetActive(true);
                    LeanTween.alphaCanvas(currentOnboardingSlides[i].GetComponent<CanvasGroup>(), 1f, 0.25f).setEase(LeanTweenType.easeInBack);
                }
            } 
        }

        
    }
}