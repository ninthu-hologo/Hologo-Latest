using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2.library;
using System.Threading;
using System;
using Hologo.iOSUI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 12 july 2019
    /// Modified by:  //Shariz 15th September 2019 v2
    /// this script controlls the library view and its functions
    /// </summary>
    public class libraryViewController : MonoBehaviour
    {
        [Header("DATA")]
        [SerializeField]
        private settings_SO settings;
        [SerializeField]
        private userData_SO userData;
        [SerializeField]
        private categoryData_SO categoryData;
        [SerializeField]
        private libraryData_SO libraryData;
        [Header("UI CONNECTOR")]
        [SerializeField]
        private librarySwitcher lConnect;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;
        [SerializeField]
        private loadScene sceneLoader;
        [Header("EVENTS")]
        [SerializeField]
        private event_SO loginOrSignUpEvent;

        // shariz oct 26 making user go to subscription if already signed in but no subscription 2.00
        [SerializeField]
        private event_SO gotoSubscriptionsEvent;

        [Header("SCRIPTS")]
        [SerializeField]
        private libraryManager lManager;
        [SerializeField]
        private downloadImageQueue downloadQueuer;

        [SerializeField]
        private categoryManager cManager;

        // callback for category buttons
        private CallbackGameObject[] categoryActions = new CallbackGameObject[1];
        // callbacks for experience items
        private CallbackGameObject[] libraryActions = new CallbackGameObject[2];
        // this is a cancellation source.which is use to cancel async task methods.
        // mainly used in cancelling making items for the previous category when user clicks on another category
        CancellationTokenSource tokenSource;

        //shariz adding DL Exp object
        public GameObject dlExpObject;

        //shariz adding bool for library load check
        public eventBool libraryLoad;



        public void setLibraryViewActive()
        {
            lConnect.getUIConnect().setLibraryUIState(true); //Shariz 15th September 2019 v2
            //generate ui if its not 
            initiateLibraryViewController();
        }


        public void setLibraryViewFalse()
        {
            lConnect.getUIConnect().setLibraryUIState(false); //Shariz 15th September 2019 v2
        }


        private void Start()
        {
           // initiateLibraryViewController();
            StartCoroutine(initiateLibraryViewControllerCoroutine());

            //shariz making viewDescription for Deeplinks
            dlExpObject.SetActive(false);
        }


        #region COROUTINE LIBRARY GENERATE
        // generating ui here
        // get data from the settings . category . library
        // get current category from settings
        // then generate the category ui
        // then generate the library ui
        public IEnumerator initiateLibraryViewControllerCoroutine()
        {
            // check to see if library UI is already generated
            if (lConnect.getUIConnect().isLibraryPopulated()) //Shariz 15th September 2019 v2
                yield break;

            // settings up the actions for callbacks
            categoryActions[0] = changeCategoryNew; // coroutine test by hamid 01-11-2019
            libraryActions[0] = viewDescription;
            libraryActions[1] = viewExperience;
            // make categories
            yield return StartCoroutine(generateCategoriesCoroutine());
            // make active library. if settings current category is null then we assign a default one.
            // make active library. if settings current category is null then we assign a default one.
            if (settings.currentCategory == null)
            {
                settings.makeHomeDefaultCategory();
            }

            if (settings.currentCategory.id == 0)
            {
                settings.makeHomeDefaultCategory();
            }


            lConnect.getUIConnect().setActiveCategory(settings.currentCategory.id); //shariz setting home as active category //Shariz 15th September 2019 v2
            // make library

            //shariz attempting to add deeplink
            libraryLoad.Invoke(true);

            yield return StartCoroutine(libraryRefreshCoroutine(settings.currentCategory));

            

        }

        IEnumerator generateCategoriesCoroutine()
        {
            yield return StartCoroutine( lConnect.getUIConnect().makeCategoryButtonsCoroutine(categoryData.getCategoryData(), categoryActions)); //Shariz 15th September 2019 v2
        }

        // update library experiences according to category
        IEnumerator libraryRefreshCoroutine(categoryClass category)
        {
            if (category == null)
            {
                category = categoryData.getCategoryData()[0];
            }
            bool home = false;
            if (category.id == 18)
            {
                home = true;
            }

                tokenSource = new CancellationTokenSource();
                List<experienceDataClass> data = libraryData.filterLibraryToCategory(category);
                yield return StartCoroutine( lConnect.getUIConnect().makeCachedCategoryExpsCoroutine(home, libraryData.getCachedCategory(category.orderInList), downloadQueuer, libraryActions)); //Shariz 15th September 2019 v2
        }


        // change category view user clicks a category button
        void changeCategoryNew(GameObject go)
        {
            // check if tokenSource is not null and set the token to cancel. if a previous library items is beinf generated
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
            StopAllCoroutines();
            lConnect.getUIConnect().stopMyCoroutines();
            // delete the active exprience gameobject itms
            lConnect.getUIConnect().DeleteActiveList(); //Shariz 15th September 2019 v2
            // async change category 
            StartCoroutine(changeCategoryCoroutine(go));
            lConnect.getUIConnect().setActiveCategory(go.GetComponent<categoryUIButton>().catID); //shariz setting active category //Shariz 15th September 2019 v2
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        // here when user clicks category. that category is set in mainsettings and library is generated for that category
        IEnumerator changeCategoryCoroutine(GameObject go)
        {
            IUIElement iui = go.GetComponent<IUIElement>();
            settings.setCurrentCategory(iui.getData() as categoryClass);
            yield return StartCoroutine(libraryRefreshCoroutine(settings.currentCategory));
        }

        #endregion



        // generating ui here
        // get data from the settings . category . library
        // get current category from settings
        // then generate the category ui
        // then generate the library ui
        public async void initiateLibraryViewController()
        {
            // check to see if library UI is already generated
            if (lConnect.getUIConnect().isLibraryPopulated()) //Shariz 15th September 2019 v2
                return;
           // settings up the actions for callbacks
            categoryActions[0] = changeCategory;
            libraryActions[0] = viewDescription;
            libraryActions[1] = viewExperience;
            // make categories
            await generateCategories();
            // make active library. if settings current category is null then we assign a default one.
            // make active library. if settings current category is null then we assign a default one.
            if (settings.currentCategory == null)
            {
                settings.makeHomeDefaultCategory();
            }

            if (settings.currentCategory.id == 0)
            {
                settings.makeHomeDefaultCategory();
            }
                

            lConnect.getUIConnect().setActiveCategory(settings.currentCategory.id); //shariz setting home as active category //Shariz 15th September 2019 v2
            // make library
            await libraryRefresh(settings.currentCategory);
        }

        // async method to make categories. the main practical part is carried out in store ui connect
        async Task generateCategories()
        {
            await lConnect.getUIConnect().makeCategoryButtons(categoryData.getCategoryData(), categoryActions); //Shariz 15th September 2019 v2
        }

        // change category view user clicks a category button
        void changeCategory(GameObject go)
        {
            // check if tokenSource is not null and set the token to cancel. if a previous library items is beinf generated
            if(tokenSource !=null)
            {
                tokenSource.Cancel();
            }
            // delete the active exprience gameobject itms
            lConnect.getUIConnect().DeleteActiveList(); //Shariz 15th September 2019 v2
            // async change category 
            asyncChangeCategory(go);
            lConnect.getUIConnect().setActiveCategory(go.GetComponent<categoryUIButton>().catID); //shariz setting active category //Shariz 15th September 2019 v2
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }

        // here when user clicks category. that category is set in mainsettings and library is generated for that category
        async void asyncChangeCategory(GameObject go)
        {
            IUIElement iui = go.GetComponent<IUIElement>();
            settings.setCurrentCategory(iui.getData() as categoryClass);
            await libraryRefresh(settings.currentCategory,tokenSource.Token);
        }

        // update library experiences according to category
        async Task libraryRefresh(categoryClass category, CancellationToken cancelToken = new CancellationToken())
        {
            if (category == null)
            {
                category = categoryData.getCategoryData()[0];
            }
            bool home = false;
            if(category.id == 18)
            {
                home = true;
            }

            try
            {
                tokenSource = new CancellationTokenSource();
            List<experienceDataClass> data = libraryData.filterLibraryToCategory(category);
            
               // await lConnect.makeSubcategoriesAndExperiences(category, libraryData.getLibraryData(), tokenSource.Token, libraryActions);
                await lConnect.getUIConnect().makeCachedCategoryExps(home, libraryData.getCachedCategory(category.orderInList), downloadQueuer, tokenSource.Token, libraryActions); //Shariz 15th September 2019 v2
                cancelToken.ThrowIfCancellationRequested();
           }
            catch (OperationCanceledException ex)
            {
                Debug.Log("cancellation catched in ui controller");
            }
        }


        //async Task libraryRefreshWithSubCategories(categoryClass category, CancellationToken cancelToken = new CancellationToken())
        //{

        //    List<categoryClass> subcategories = category.children;
        //    try
        //    {

        //    }
        //    catch (OperationCanceledException ex)
        //    {
        //        Debug.Log("cancellation catched in ui controller");
        //    }
        //}

        // excutuing server library update process
        public void updateLibraryFromDragDown()
        {
            updatelibraryFromServer();
        }

        async void updatelibraryFromServer()
        {
            bool success = await cManager.updateCategories();
             success = await lManager.updateLibrary(categoryData.getCategoryData());
            if(success)
            {
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                Debug.Log("new updates");
               string title = localizationProvider.provide.getLanguage().getAMessageText(62);
                windowMain.InfomationToast.ShowToast(title, 3f);
               // lConnect.clearLibrary();
               // await libraryRefresh(settings.currentCategory);
            }
        }

        // sends to ar view and experience is downloaded/loaded and shown
        void viewExperience(GameObject go)
        {
            Debug.Log("viewing ar");
            experienceUIElement euie = go.GetComponent<experienceUIElement>();
            experienceDataClass edc = euie.getData() as experienceDataClass;
            
            if(edc.experience.asset_bundles.Count<=0)
            {
                string toastMessage = localizationProvider.provide.getLanguage().getALabelText(91);// Shariz 22nd Feb 2020 2.0.4
                windowMain.InfomationToast.ShowToast(toastMessage, 3f);// Shariz 22nd Feb 2020 2.0.4
                return;
            }


            if(edc.experience.free)
            {
                settings.currentExperience = euie.getData() as experienceDataClass;
                gotoArScene();
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {
                if (userData.isUserSubscribed() && userData.isUserSignedIn())
                {
                    settings.currentExperience = euie.getData() as experienceDataClass;
                    gotoArScene();
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
                    SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                }
                // shariz oct 26 making user go to subscription if already signed in but no subscription 2.00
                else if (!userData.isUserSubscribed() && userData.isUserSignedIn()){
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
                    gotoSubscriptionsEvent.raise(this.gameObject);
                }
                else
                {
                    // show login page
                    iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
                    SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                    loginOrSignUpEvent.raise(this.gameObject);
                }
            }
        }
        
        // view experience description
        void viewDescription(GameObject go)
        {
            experienceUIElement euie = go.GetComponent<experienceUIElement>();
            experienceDataClass edc = euie.getData() as experienceDataClass;
            bool canview = edc.experience.asset_bundles.Count > 0;
           
                Debug.Log("viewing descrciption");
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            windowMain.experienceDetailWindow.getExpDetailWindow().FillInput(canview,euie.giveImageSprite(), settings.currentCategory.title, edc.title, edc.body, ()=> viewExperience(go));
        }

        ////shariz making viewDescription for Deeplinks
        public void setDLExperience(GameObject go)
        {
            ProcessDeepLinkMngr deepLinkManager = go.GetComponent<ProcessDeepLinkMngr>();

            experienceUIElement iui = dlExpObject.GetComponent<experienceUIElement>();
            iui.fillWithDataSync(deepLinkManager.expData, downloadQueuer, libraryActions);
            dlExpObject.SetActive(true);

            viewExperience(dlExpObject);
            viewDescription(dlExpObject);
        }



        void gotoArScene()
        {
            sceneLoader.goToScene();
        }
    }
}