using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: 
    /// experience ui element 
    /// </summary>
    public class libraryUI : iUILayoutBase, IPrefabLocalize
    {
        /* things needed to chnage
         * library heading . font . alignment
         * category content rect . anchors (for rtl)
         * */

        [Header("UI ELEMENTS")]
        [SerializeField]
        private TextMeshProUGUI LibraryHeading;
        [SerializeField]
        private RectTransform categoryContent;
        [SerializeField]
        private Transform categoryContentParent;
        [SerializeField]
        private Transform experiencesContentParent;
        [SerializeField]
        private Transform featuredContentParent;
        [SerializeField]
        private GameObject featuredGroup;
        [SerializeField]
        private MyScrollRect scrollRect;

        // shariz v2 12/9/2019
        [SerializeField]
        private Transform experiencesHomeParent; //Shariz 15th September 2019 v2
        
        [SerializeField]
        private RectTransform smallTitleBar; //Shariz 1st nov 2019 v2

        


        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            //if (language.rightToLeft)
            //{
            //    uiHelper.SetAnchor(categoryContent, AnchorPresets.MiddleRight);
            //    uiHelper.SetPivot(categoryContent, PivotPresets.MiddleRight);
            //}
            //else
            //{
            //    uiHelper.SetAnchor(categoryContent, AnchorPresets.MiddleLeft);
            //    uiHelper.SetPivot(categoryContent, PivotPresets.MiddleLeft);
            //}

            // setting the library font and text
            localizeSetting.setTextConfig(LibraryHeading, localizeSetting.getLanguage().getAFont(0), localizeSetting.getLanguage().getATitleText(0));


        // shariz v2 5/9/2019 attempting scrolling small header
            localizeSetting.setTextConfig(LibraryHeadingSmall, localizeSetting.getLanguage().getAFont(35), localizeSetting.getLanguage().getATitleText(0)); //Shariz 15th September 2019 v2
        }


        public Transform giveCategoryParent()
        {
            return categoryContentParent;
        }

        public Transform giveExperiencesParent()
        {
            return experiencesContentParent;
        }

        // shariz v2 12/9/2019
        public Transform giveExperiencesHomeParent() //Shariz 15th September 2019 v2
        {
            return experiencesHomeParent;
        }

        public MyScrollRect getParentScrollRect()
        {
            return scrollRect;
        }

        public Transform getFeaturedContentParent()
        {
            setFeaturedGroupState(true);
            return featuredContentParent;
        }

        public void setFeaturedGroupState(bool state)
        {
           featuredGroup.SetActive(state);
        }




        public void Start() {
            //Subscribe to the Scrollbar event
            // scrollRect.onValueChanged.AddListener(scrollRectCallBack);
            startPosition = scrollRect.transform.InverseTransformPoint(trackTrans.position).y;
            initPos = true;
            scrollRect.onValueChanged.AddListener(newScrollRectCallBack);
            LibraryHeadingSmall.gameObject.SetActive(false);
            LibraryHeading.gameObject.SetActive(true);

           //    Debug.Log("Libraru UI started");

            //Shariz 1st nov 2019 v2
            if(mainSettings.device==deviceType.IPHONEX){
            smallTitleBar.offsetMax = new Vector2(smallTitleBar.offsetMax.x, 0);
            smallTitleBar.offsetMin = new Vector2(smallTitleBar.offsetMin.x, -70);
            }
        }



        // shariz v2 5/9/2019 attempting scrolling small header

        [SerializeField]
        private TextMeshProUGUI LibraryHeadingSmall;
      //  float lastValue = 0;
        		
       [SerializeField]
       private settings_SO mainSettings;

        void OnEnable()
        {
            //Subscribe to the Scrollbar event
          //  scrollRect.onValueChanged.AddListener(scrollRectCallBack);
            scrollRect.onValueChanged.AddListener(newScrollRectCallBack);
            LibraryHeadingSmall.gameObject.SetActive(false);
            LibraryHeading.gameObject.SetActive(true);
             //  Debug.Log("Libraru UI enabled");
        }

    

        //Will be called when ScrollRect changes
        void scrollRectCallBack(Vector2 value)
        {
          

            if (value.y < 0.9){
                LibraryHeadingSmall.gameObject.SetActive(true);
                LibraryHeading.gameObject.SetActive(false);
                getTitleTransform().sizeDelta = new Vector2(getTitleTransform().sizeDelta.x,100+mainSettings.titleMargin);
                getBodyTransform().offsetMax = new Vector2(getBodyTransform().offsetMax.x,-120-mainSettings.titleMargin);
            }
            else {
                LibraryHeadingSmall.gameObject.SetActive(false);
                LibraryHeading.gameObject.SetActive(true);
                getTitleTransform().sizeDelta = new Vector2(getTitleTransform().sizeDelta.x,163+mainSettings.titleMargin);
                getBodyTransform().offsetMax = new Vector2(getBodyTransform().offsetMax.x,-184-mainSettings.titleMargin);
                
            }
//            Debug.Log("ScrollRect Changed: " + value);
        }

        public Transform trackTrans;
        private bool initPos = false;
        private float startPosition;
        [SerializeField]
        private float posBuffer = 200f;

        public bool headerBigChanged;
        public bool headerSmallChanged;
        

        void newScrollRectCallBack(Vector2 value) //shariz 1st Nov hologo 2 animating header
        {
            if(initPos)
            {
                startPosition = scrollRect.transform.InverseTransformPoint(trackTrans.position).y;
                initPos = false;
            }


            float pos = scrollRect.transform.InverseTransformPoint(trackTrans.position).y;

            if (pos > startPosition+posBuffer)
            {

                if(headerBigChanged){
                    headerBigChanged = false;
                }
                if(!headerSmallChanged){
                LeanTween.value( LibraryHeading.gameObject, changeHeadingColor, new Color(0.1019608f,0.1019608f,0.1019608f, 1), new Color(0.1019608f,0.1019608f,0.1019608f, 0), 0.2f).setEase(LeanTweenType.easeOutQuart);
                LeanTween.value( LibraryHeadingSmall.gameObject, changeHeadingSmallColor, new Color(0.1019608f,0.1019608f,0.1019608f, 0), new Color(0.1019608f,0.1019608f,0.1019608f, 1), 0.3f).setEase(LeanTweenType.easeOutQuart);
                headerSmallChanged = true;
                LibraryHeadingSmall.gameObject.SetActive(true);
                LeanTween.delayedCall(LibraryHeading.gameObject, 0.22f, ()=>{
                LibraryHeading.gameObject.SetActive(false);
                });
                }

                LeanTween.size(getTitleTransform(), new Vector2(getTitleTransform().sizeDelta.x, 100+mainSettings.titleMargin), 0.3f).setEase(LeanTweenType.easeOutQuart);
                LeanTween.value(getBodyTransform().gameObject, changeBodyOffset, getBodyTransform().offsetMax.y, -120-mainSettings.titleMargin,0.3f).setEase(LeanTweenType.easeOutQuart);;
                

                


                 

                
                
            }
            if(pos < startPosition + posBuffer)
            {
                if(headerSmallChanged){
                    headerSmallChanged = false;
                }
                if(!headerBigChanged){
                    LeanTween.value( LibraryHeadingSmall.gameObject, changeHeadingSmallColor, new Color(0.1019608f,0.1019608f,0.1019608f, 1), new Color(0.1019608f,0.1019608f,0.1019608f, 0), 0.07f).setEase(LeanTweenType.easeOutQuart);
                    LeanTween.value( LibraryHeading.gameObject, changeHeadingColor, new Color(0.1019608f,0.1019608f,0.1019608f, 0), new Color(0.1019608f,0.1019608f,0.1019608f, 1), 0.25f).setEase(LeanTweenType.easeOutQuart);
                    headerBigChanged = true;
                    LeanTween.delayedCall(LibraryHeadingSmall.gameObject, 0.1f, ()=>{
                    LibraryHeadingSmall.gameObject.SetActive(false);
                    });
                    LibraryHeading.gameObject.SetActive(true);
                }


                LeanTween.size(getTitleTransform(), new Vector2(getTitleTransform().sizeDelta.x, 163+mainSettings.titleMargin), 0.25f).setEase(LeanTweenType.easeOutQuad);;
                LeanTween.value(getBodyTransform().gameObject, changeBodyOffset, getBodyTransform().offsetMax.y, -184-mainSettings.titleMargin,0.25f).setEase(LeanTweenType.easeOutQuad);;



            }
        }

        //shariz oct 20 hologo 2 animating header
        void changeBodyOffset(float value){
            getBodyTransform().offsetMax = new Vector2(getBodyTransform().offsetMax.x, value);
        }

        //shariz oct 20 hologo 2 animating header
        void changeHeadingSmallColor( Color value ){
            LibraryHeadingSmall.color = value;
        } 
        
        //shariz oct 20 hologo 2 animating header
        void changeHeadingColor( Color value ){
            LibraryHeading.color = value;
        } 


        void OnDisable()
        {
            //Un-Subscribe To ScrollRect Event
           // scrollRect.onValueChanged.RemoveListener(scrollRectCallBack);
        }

      
    }
}
