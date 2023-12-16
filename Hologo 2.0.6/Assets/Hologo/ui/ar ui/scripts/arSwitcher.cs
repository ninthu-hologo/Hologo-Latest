using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
public class arSwitcher : MonoBehaviour
{
  //Shariz 15th September 2019 v2
[Header("DATA")]
[SerializeField]
private settings_SO mainSettings;


[Header("UI CONNECTOR")]
        [SerializeField]
        private experienceDisplayUIElement expUIElementMobile;        
        
        [SerializeField]
        private experienceDisplayUIElement expUIElementTablet;

        [SerializeField]
        private Transform lessonUIElementParentMobile;        
        [SerializeField]
        private Transform lessonUIElementParentTablet;
        [SerializeField]
        private GameObject lessonCanvasMobile;
        [SerializeField]
        private GameObject lessonCanvasTablet;

        [SerializeField]
        private quizUIElement quizUIMobile;

        [SerializeField]
        private quizUIElement quizUITablet;

        [SerializeField]
        private arLoadingSceneUICanvas arLoadingMobile;        
        
        [SerializeField]
        private arLoadingSceneUICanvas arLoadingTablet; 

        [SerializeField]
        private Transform exploreStepsParentMobile;

        [SerializeField]
        private Transform exploreStepsParentTablet;


        [SerializeField]
        private Transform learnStepsParentMobile;

        [SerializeField]
        private Transform learnStepsParentTablet;
        [SerializeField]
        private onboardingPages onboardingMobile;// Shariz 25th Oct 2019 2.00

        [SerializeField]
        private onboardingPages onboardingTablet;// Shariz 25th Oct 2019 2.00
                
        [SerializeField]
        private GameObject ObjectModelPortalExitButtonMobile;// Shariz 10th Dec 2019 2.0.2

        [SerializeField]
        private GameObject ObjectModelPortalExitButtonTablet;// Shariz 10th Dec 2019 2.0.2



    // Shariz 10th Dec 2019 2.0.2
    public GameObject GetObjectModelPortalExitButton()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return ObjectModelPortalExitButtonTablet;
        }
        return ObjectModelPortalExitButtonMobile;
    }


    public Transform GetexploreStepsParent()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return exploreStepsParentTablet;
        }
        return exploreStepsParentMobile;
    }

    public Transform GetlearnStepsParent()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return learnStepsParentTablet;
        }
        return learnStepsParentMobile;
    }

    
    
    
    public experienceDisplayUIElement GetExperienceDisplayUIElementDeviceWithoutHiding()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            expUIElementTablet.gameObject.SetActive(true);
            expUIElementMobile.gameObject.SetActive(false);
            return expUIElementTablet;
        }
        expUIElementTablet.gameObject.SetActive(false);
        expUIElementMobile.gameObject.SetActive(true);
        return expUIElementMobile;
    }
    public experienceDisplayUIElement GetExperienceDisplayUIElementDevice()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return expUIElementTablet;
        }
        return expUIElementMobile;
    }
    
   
    public Transform GetlessonUIElementParent()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            // lessonUIElementParentMobile.gameObject.SetActive(false);
            // lessonUIElementParentTablet.gameObject.SetActive(true);
            return lessonUIElementParentTablet;
        }
        // lessonUIElementParentTablet.gameObject.SetActive(false);
        // lessonUIElementParentMobile.gameObject.SetActive(true);
        return lessonUIElementParentMobile;
    }

    public GameObject GetlessonCanvas()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            // lessonCanvasMobile.gameObject.SetActive(false);
            // lessonCanvasTablet.gameObject.SetActive(true);
            return lessonCanvasTablet;
        }
        // lessonCanvasTablet.gameObject.SetActive(false);
        // lessonCanvasMobile.gameObject.SetActive(true);
        return lessonCanvasMobile;
    }

    public quizUIElement GetquizUI()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            // quizUIMobile.gameObject.SetActive(false);
            // quizUITablet.gameObject.SetActive(true);
            return quizUITablet;
        }
        // quizUITablet.gameObject.SetActive(false);
        // quizUIMobile.gameObject.SetActive(true);
        return quizUIMobile;
    }
    public arLoadingSceneUICanvas GetARLoadingUI()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            arLoadingMobile.gameObject.SetActive(false);
            arLoadingTablet.gameObject.SetActive(true);
            return arLoadingTablet;
        }
        arLoadingTablet.gameObject.SetActive(false);
        arLoadingMobile.gameObject.SetActive(true);
        return arLoadingMobile;
    }

    // Shariz 25th Oct 2019 2.00
    public onboardingPages GetOnboardingWindow()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            // onboardingMobile.gameObject.SetActive(false);
            // onboardingTablet.gameObject.SetActive(true);
            return onboardingTablet;
        }
        // onboardingTablet.gameObject.SetActive(false);
        // onboardingMobile.gameObject.SetActive(true);
        return onboardingMobile;
    }

    
   
}
