using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class manages everthing to do with the library
    /// </summary>
    public class updateLibrary
    {


        // comparing storage and server library and returning new and updated experiences
        public List<experienceDataClass> updatesLibrary(List<experienceDataClass> libraryDataStorage, List<experienceDataClass> libraryDataServer)
        {
            List<experienceDataClass> updatedlibraryData = new List<experienceDataClass>();

            // checking to see if there are new experiences and add them to the updated library data
            getNewExperiences(libraryDataStorage, libraryDataServer, updatedlibraryData);
            // checking for updated experiences and getting them
            getUpdatedExperiences(libraryDataStorage, libraryDataServer, updatedlibraryData);

            return updatedlibraryData;
        }
        

        // getting updated experiences
        private static void getUpdatedExperiences(List<experienceDataClass> libraryDataStorage, List<experienceDataClass> libraryDataServer, 
            List<experienceDataClass> updatedlibraryData)
        {
            for (int a = 0; a < libraryDataStorage.Count; a++)
            {
                for (int b = 0; b < libraryDataServer.Count; b++)
                {
                    if (libraryDataStorage[a].Equals(libraryDataServer[b]))
                    {
                        if (libraryDataStorage[a].dateCompare(libraryDataServer[b]))
                        {
                            Debug.Log(libraryDataServer[b].title + " is updated!");
                            updatedlibraryData.Add(libraryDataServer[b]);
                        }
                    }
                }
            }
        }

        // getting new experiences
        private void getNewExperiences(List<experienceDataClass> libraryDataStorage, List<experienceDataClass> libraryDataServer, 
            List<experienceDataClass> updatedlibraryData)
        {
            if (CheckCountBetweenTwoLists(libraryDataStorage.Count, libraryDataServer.Count))
            {

                for (int a = 0; a < libraryDataServer.Count; a++)
                {
                    bool isNew = true;
                    for (int b = 0; b < libraryDataStorage.Count; b++)
                    {
                        if (libraryDataStorage[b].Equals(libraryDataServer[a]))
                        {
                            isNew = false;
                        }
                    }

                    if (isNew)
                    {
                        Debug.Log(libraryDataServer[a].title + " is new!");
                        libraryDataServer[a].isNew = true;
                        updatedlibraryData.Add(libraryDataServer[a]);
                    }
                }
            }
        }

        // this where the updated experiences gets copied over to data storage
        public void copyOverUpdates(List<experienceDataClass> libraryDataStorage, List<experienceDataClass> updatedExperiences)
        {
            for (int a = 0; a < updatedExperiences.Count; a++)
            {
                for (int b = 0; b < libraryDataStorage.Count; b++)
                {
                    if (updatedExperiences[a].Equals(libraryDataStorage[b]))
                    {
                        //libraryDataStorage[b].setExperienceItemData(updatedExperiences[a]);
                        libraryDataStorage[b] = updatedExperiences[a];
                        libraryDataStorage[b].IsDataChanged = true;
                    }
                    if(updatedExperiences[a].isNew)
                    {
                        updatedExperiences[a].isNew = false;
                        libraryDataStorage.Add(updatedExperiences[a]);
                        //here we also add this to cached part.
                    }
                }
            }
        }


        // checking for duplicate entries and removing them
        public List<experienceDataClass> removeDuplicateEntries(List<experienceDataClass> libraryData)
        {
            List<experienceDataClass> updatedlibraryData = new List<experienceDataClass>();

            return updatedlibraryData;
        }


        // list counct comparison
        protected bool CheckCountBetweenTwoLists(int originalList, int newList)
        {
            return newList > originalList;
        }
    }
}