using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this scriptable object is a data container for experiences
    /// </summary>
    [CreateAssetMenu(fileName ="library.asset",menuName ="Hologo V2/new library Data")]
    public class libraryData_SO : ScriptableObject
    {
        [SerializeField]
        private List<experienceDataClass> libraryData;

        [SerializeField]
        private List<experienceDataClass> ActiveLibrary;

        [SerializeField]
        private List<experienceDataClass> homeLibrary;

        [SerializeField]
        private List<LibraryCategory> cachedLibraryData;


        public void fillData(List<experienceDataClass> library)
        {
            libraryData = library;
        }

        public bool isLibaryfilled()
        {
            return libraryData.Count > 0;

        }

        public void clearAsset()
        {
            libraryData.Clear();
            ActiveLibrary.Clear();
        }

        public List<experienceDataClass> filterLibraryToCategory(categoryClass category)
        {
            ActiveLibrary.Clear();
            ActiveLibrary = new List<experienceDataClass>();
            // Shariz checking if category id is zero, ie Home
            if (category.id == 0)
            {
                for (int i = 0; i < libraryData.Count; i++)
                {
                    if (libraryData[i].featured)
                    {
                        ActiveLibrary.Add(libraryData[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < libraryData.Count; i++)
                {
                    if (libraryData[i].subject_id == category.id)
                    {
                        ActiveLibrary.Add(libraryData[i]);
                    }
                }
            }
            return ActiveLibrary;
        }

        public List<experienceDataClass> getLibraryData()
        {
            return libraryData;
        }

        public LibraryCategory getCachedCategory(int id)
        {
            return cachedLibraryData[id];
        }

       


        public void cacheFilteredLibrary(List<categoryClass> categoryData)
        {
            cachedLibraryData = new List<LibraryCategory>();
            homeLibrary = new List<experienceDataClass>();
            for (int a = 0; a < categoryData.Count; a++)
            {
                LibraryCategory lc = new LibraryCategory();
                lc.name = categoryData[a].title;
                lc.featuredList = new List<experienceDataClass>();
                lc.subCategoryAndExps = new List<SubcategoryAndExperiences>();

                for (int b = 0; b < categoryData[a].children.Count; b++)
                {
                    SubcategoryAndExperiences se = new SubcategoryAndExperiences();
                    se.subExperiences = new List<experienceDataClass>();
                    se.name = categoryData[a].children[b].title;
                    se.subcategory = categoryData[a].children[b];
                    for (int i = 0; i < libraryData.Count; i++)
                    {
                        if(libraryData[i].subject_id == categoryData[a].children[b].id)
                        {
                            if(libraryData[i].experience.featured)
                            {
                                lc.featuredList.Add(libraryData[i]);
                            }

                            se.subExperiences.Add(libraryData[i]);

                            if (libraryData[i].international)
                            {
                                homeLibrary.Add(libraryData[i]);
                               
                            }

                        }
                    }

                    lc.subCategoryAndExps.Add(se);
                }


                cachedLibraryData.Add(lc);
            }

            cachedLibraryData[0].featuredList = homeLibrary;

        }

        public void flushData()
        {
            libraryData.Clear();
            ActiveLibrary.Clear();
            homeLibrary.Clear();
            cachedLibraryData.Clear();
        }
    }
}
