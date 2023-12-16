using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Hologo2.library;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 12 july 2019
    /// Modified by: 
    /// this script controlls the library view and its functions
    /// </summary>
    public class libraryUIConnect : MonoBehaviour
    {
        [Header("UI PREFABS")]
        [SerializeField]

        private settings_SO settings; //shariz added settings to get current

        [SerializeField]
        private localizePrefab_SO expElementHome;
        [SerializeField]
        private localizePrefab_SO expElementFeatured;
        [SerializeField]
        private localizePrefab_SO expElementThumb;

        [SerializeField]
        private localizePrefab_SO subcategoryPrefab;

        [SerializeField]
        private localizePrefab_SO categoryButton;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private libraryUI libUI;

        private List<GameObject> categoryButtons = new List<GameObject>();
        [SerializeField]
        private List<GameObject> activeSubcategories = new List<GameObject>();
        [SerializeField]
        private List<GameObject> activeExperiences = new List<GameObject>();

        public void stopMyCoroutines()
        {
            StopAllCoroutines();
        }

        public void clearLibrary()
        {
            DeleteActiveList();
        }

        public void setLibraryUIState(bool state)
        {
            libUI.gameObject.SetActive(state);
        }



        public async Task makeCategoryButtons(List<categoryClass> categories, params CallbackGameObject[] actions)
        {
            deleteUIElements(categoryButtons);
            GameObject homeButton = null;
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].status)
                {
                    GameObject go = Instantiate(categoryButton.givePrefab());
                    IUIElement iui = go.GetComponent<IUIElement>();
                    iui.fillWithDataSync(categories[i], actions);
                    go.transform.SetParent(libUI.giveCategoryParent());
                    go.transform.localScale = new Vector3(1, 1, 1);
                    if (categories[i].id == 18)
                    {
                        homeButton = go;
                    }
                    categoryButtons.Add(go);
                }
                await Task.Yield();
            }

            homeButton.transform.SetAsFirstSibling();
        }


        public IEnumerator makeCategoryButtonsCoroutine(List<categoryClass> categories, params CallbackGameObject[] actions)
        {
            deleteUIElements(categoryButtons);
            GameObject homeButton = null;
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].status)
                {
                    GameObject go = Instantiate(categoryButton.givePrefab());
                    IUIElement iui = go.GetComponent<IUIElement>();
                    iui.fillWithDataSync(categories[i], actions);
                    go.transform.SetParent(libUI.giveCategoryParent());
                    go.transform.localScale = new Vector3(1, 1, 1);
                    if (categories[i].id == 18)
                    {
                        homeButton = go;
                    }
                    categoryButtons.Add(go);
                }
                yield return null;
            }

            homeButton.transform.SetAsFirstSibling();
        }

       

        public async Task makeCachedCategoryExps(bool home,LibraryCategory libraryCategory, downloadImageQueue dq, CancellationToken cancelToken = new CancellationToken(), params CallbackGameObject[] actions)
        {
            GameObject go;
            GameObject gamePrefab;
            try
            {
                Transform featuredP = null;
                if(home)
                {
                    libUI.setFeaturedGroupState(false);
                    libUI.giveExperiencesParent().gameObject.SetActive(false);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                    libUI.giveExperiencesHomeParent().gameObject.SetActive(true);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                    featuredP = libUI.giveExperiencesHomeParent();// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                    gamePrefab = expElementHome.givePrefab();
                }
                else
                {
                    libUI.setFeaturedGroupState(true);
                    libUI.giveExperiencesParent().gameObject.SetActive(true);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                    libUI.giveExperiencesHomeParent().gameObject.SetActive(false);// shariz v2 12/9/2019           //Shariz 15th September 2019 v2          
                    featuredP = libUI.getFeaturedContentParent();
                    gamePrefab = expElementFeatured.givePrefab();
                }

                // making featured section
                if (libraryCategory.featuredList.Count <= 0)
                {
                    Debug.Log("no featured items");
                    libUI.setFeaturedGroupState(false);
                }
                else
                {
                    libUI.setFeaturedGroupState(true);
                    for (int i = 0; i < libraryCategory.featuredList.Count; i++)
                    {
                        if (libraryCategory.featuredList[i].experience.status)
                        {
                            // making home or featured list of experienc covers

                            go = Instantiate(gamePrefab);
                            experienceUIElement iui = go.GetComponent<experienceUIElement>();
                            iui.fillWithDataSync(libraryCategory.featuredList[i], dq, actions);
                            go.transform.SetParent(featuredP);
                            go.transform.localScale = new Vector3(1, 1, 1);
                            activeExperiences.Add(go);
                        }

                        cancelToken.ThrowIfCancellationRequested();
                        await Task.Yield();
                    }
                }

                if (libraryCategory.subCategoryAndExps.Count <= 0)
                    return;

                // making sub category section
                for (int i = 0; i < libraryCategory.subCategoryAndExps.Count; i++)
                {
                    if (libraryCategory.subCategoryAndExps[i].subcategory.status)
                    {
                        go = Instantiate(subcategoryPrefab.givePrefab());
                        subcategoryUIElement suie = go.GetComponent<subcategoryUIElement>();
                        suie.fillData(libraryCategory.subCategoryAndExps[i].subcategory.id, libraryCategory.subCategoryAndExps[i].subcategory.title, libUI.getParentScrollRect());
                        go.transform.SetParent(libUI.giveExperiencesParent());
                        go.transform.localScale = new Vector3(1, 1, 1);
                        Transform expParent = suie.getContentParent();
                        activeSubcategories.Add(go);

                        for (int a = 0; a < libraryCategory.subCategoryAndExps[i].subExperiences.Count; a++)
                        {
                            if (libraryCategory.subCategoryAndExps[i].subExperiences[a].experience.status)
                            {
                                go = Instantiate(expElementThumb.givePrefab());
                                experienceUIElement iui = go.GetComponent<experienceUIElement>();
                                // making experience covers inside sub category
                                
                                iui.fillWithDataSync(libraryCategory.subCategoryAndExps[i].subExperiences[a], dq, actions);
                                go.transform.SetParent(expParent);
                                go.transform.localScale = new Vector3(1, 1, 1);
                                activeExperiences.Add(go);
                            }

                            cancelToken.ThrowIfCancellationRequested();
                            await Task.Yield();
                        }
                    }
                    cancelToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log("cancellation catched in ui connect");
            }
        }


        public IEnumerator makeCachedCategoryExpsCoroutine(bool home, LibraryCategory libraryCategory, downloadImageQueue dq, params CallbackGameObject[] actions)
        {
            GameObject go;
            GameObject gamePrefab;

            Transform featuredP = null;
            if (home)
            {
                libUI.setFeaturedGroupState(false);
                libUI.giveExperiencesParent().gameObject.SetActive(false);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                libUI.giveExperiencesHomeParent().gameObject.SetActive(true);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                featuredP = libUI.giveExperiencesHomeParent();// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                gamePrefab = expElementHome.givePrefab();
            }
            else
            {
                libUI.setFeaturedGroupState(true);
                libUI.giveExperiencesParent().gameObject.SetActive(true);// shariz v2 12/9/2019 //Shariz 15th September 2019 v2
                libUI.giveExperiencesHomeParent().gameObject.SetActive(false);// shariz v2 12/9/2019           //Shariz 15th September 2019 v2          
                featuredP = libUI.getFeaturedContentParent();
                gamePrefab = expElementFeatured.givePrefab();
            }

            // making featured section
            if (libraryCategory.featuredList.Count <= 0)
            {
                Debug.Log("no featured items");
                libUI.setFeaturedGroupState(false);
            }
            else
            {
                libUI.setFeaturedGroupState(true);
                for (int i = 0; i < libraryCategory.featuredList.Count; i++)
                {
                    if (libraryCategory.featuredList[i].experience.status)
                    {
                        // making home or featured list of experienc covers

                        go = Instantiate(gamePrefab);
                        experienceUIElement iui = go.GetComponent<experienceUIElement>();
                        iui.fillWithDataSync(libraryCategory.featuredList[i], dq, actions);
                        go.transform.SetParent(featuredP);
                        go.transform.localScale = new Vector3(1, 1, 1);
                        activeExperiences.Add(go);
                    }

                    yield return null;
                }
            }

            if (libraryCategory.subCategoryAndExps.Count > 0)
            {
                // making sub category section
                for (int i = 0; i < libraryCategory.subCategoryAndExps.Count; i++)
                {
                    if (libraryCategory.subCategoryAndExps[i].subcategory.status)
                    {
                        go = Instantiate(subcategoryPrefab.givePrefab());
                        subcategoryUIElement suie = go.GetComponent<subcategoryUIElement>();
                        suie.fillData(libraryCategory.subCategoryAndExps[i].subcategory.id, libraryCategory.subCategoryAndExps[i].subcategory.title, libUI.getParentScrollRect());
                        go.transform.SetParent(libUI.giveExperiencesParent());
                        go.transform.localScale = new Vector3(1, 1, 1);
                        Transform expParent = suie.getContentParent();
                        activeSubcategories.Add(go);

                        for (int a = 0; a < libraryCategory.subCategoryAndExps[i].subExperiences.Count; a++)
                        {
                            if (libraryCategory.subCategoryAndExps[i].subExperiences[a].experience.status)
                            {
                                go = Instantiate(expElementThumb.givePrefab());
                                experienceUIElement iui = go.GetComponent<experienceUIElement>();
                                // making experience covers inside sub category

                                iui.fillWithDataSync(libraryCategory.subCategoryAndExps[i].subExperiences[a], dq, actions);
                                go.transform.SetParent(expParent);
                                go.transform.localScale = new Vector3(1, 1, 1);
                                activeExperiences.Add(go);
                            }

                            yield return null;
                        }
                    }
                  
                }
            }
               
            yield return null;

        }



        public void DeleteActiveList()
        {
            deleteUIElements(activeExperiences);
            deleteUIElements(activeSubcategories);
        }

        void deleteUIElements(List<GameObject> gameObjects)
        {
            if (gameObjects == null)
                gameObjects = new List<GameObject>();

            if (gameObjects.Count <= 0)
                return;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                Destroy(gameObjects[i]);
            }

            gameObjects.Clear();
            gameObjects = new List<GameObject>();
        }

        public bool isLibraryPopulated()
        {
            if (activeExperiences == null)
                return false;

            return activeExperiences.Count > 0;
        }


        //Shariz setting active Category
        public void setActiveCategory(int ID)
        {
            for (int i = 0; i < categoryButtons.Count; i++)
            {

                categoryUIButton iui = categoryButtons[i].GetComponent<categoryUIButton>();
                iui.deActiveCategories(false);

                if (iui.catID == ID)
                {
                    iui.deActiveCategories(true);
                }

            }


        }



    }
}
