using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class handles updates to categories
    /// </summary>
    public class updateCategories
    {
        // comparing storage and server library and returning new and updated experiences
        public List<categoryClass> updatesLibrary(List<categoryClass> categoryDataStorage, List<categoryClass> categoryDataServer)
        {
            List<categoryClass> updatedlibraryData = new List<categoryClass>();

            // checking to see if there are new experiences and add them to the updated library data
            getNewCategories(categoryDataStorage, categoryDataServer, updatedlibraryData);
            // checking for updated experiences and getting them
            getUpdatedCategories(categoryDataStorage, categoryDataServer, updatedlibraryData);

            return updatedlibraryData;
        }


        // getting updated experiences
        private static void getUpdatedCategories(List<categoryClass> libraryDataStorage, List<categoryClass> libraryDataServer,
            List<categoryClass> updatedlibraryData)
        {
            for (int a = 0; a < libraryDataStorage.Count; a++)
            {
                for (int b = 0; b < libraryDataServer.Count; b++)
                {
                    if (libraryDataStorage[a].Equals(libraryDataServer[b]))
                    {
                        if (libraryDataStorage[a].dateCompare(libraryDataServer[b]))
                        {
                            updatedlibraryData.Add(libraryDataServer[b]);
                        }
                    }
                }
            }
        }

        // getting new experiences
        private void getNewCategories(List<categoryClass> categoryDataStorage, List<categoryClass> categoryDataServer,
            List<categoryClass> updatedCategoryData)
        {
            if (CheckCountBetweenTwoLists(categoryDataStorage.Count, categoryDataServer.Count))
            {

                for (int a = 0; a < categoryDataServer.Count; a++)
                {
                    bool isNew = true;
                    for (int b = 0; b < categoryDataStorage.Count; b++)
                    {
                        if (categoryDataStorage[b].Equals(categoryDataServer[a]))
                        {
                            isNew = false;
                        }
                    }

                    if (isNew)
                    {
                        categoryDataServer[a].isNew = true;
                        updatedCategoryData.Add(categoryDataServer[a]);
                    }
                }
            }
        }

        // this where the updated experiences gets copied over to data storage
        public void copyOverUpdates(List<categoryClass> categoryDataStorage, List<categoryClass> updatedCategoryData)
        {
            for (int a = 0; a < updatedCategoryData.Count; a++)
            {
                for (int b = 0; b < categoryDataStorage.Count; b++)
                {
                    if (updatedCategoryData[a].Equals(categoryDataStorage[b]))
                    {
                        categoryDataStorage[b].setCategoryData(updatedCategoryData[a]);
                    }
                    if (updatedCategoryData[a].isNew)
                    {
                        updatedCategoryData[a].isNew = false;
                        categoryDataStorage.Add(updatedCategoryData[a]);
                    }
                }
            }
        }


       
        // list counct comparison
        protected bool CheckCountBetweenTwoLists(int originalList, int newList)
        {
            return newList > originalList;
        }


    }
    
}
