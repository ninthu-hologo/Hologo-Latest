using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class manages everthing to do with the library
    /// </summary>
    public class categoryManager : messageLogging
    {
        [SerializeField]
        private settings_SO settings_SO;
        [SerializeField]
        private categoryData_SO categories;
        [SerializeField]
        private dataPaths_SO dataPath;


        public async Task<bool> loadCategories()
        {
            /* 1. first check to see if category exits in the storage.
               2. if so load from storage - return true
               3. if not then try to connect to the server
               4. if cannot app cannot run and inform user to connect to the internet and launch app again - return false
               5. if can connect to server download category and save - return true */
            bool success = false;
            bool readFromStorage = false;

            // return true if the library is already filled
            if (categories.isCategoryfilled())
                return true;

            if (checkIfCategoriesExists())
            {
                string libDataString = readCategoriesFromStorage();
                if (string.IsNullOrEmpty(libDataString))
                {
                    // failed loadind data;
                    // try to get library from internet
                    success = await loadCategoriesFromServer();
                }
                else
                {
                    //success
                    //try convert data read from file to object data
                    success = readFromStorage = convertLibraryData(libDataString);
                    //failed try to get library from internet
                    if (!success)
                    {
                        success = await loadCategoriesFromServer();
                    }

                }
            }
            else
            {
                //failed try to get library from internet
                success = await loadCategoriesFromServer();
            }

            if (success && !readFromStorage)
            {
                saveCategoriesToStorage();
            }

            return success;
        }


        //load library from server
        private async Task<bool> loadCategoriesFromServer()
        {
            bool success = false;
            string result;
            categoryServerConnect csc = new categoryServerConnect();

            var task = csc.loadCategoriesFromServer(dataPath.getUrl(0)+ settings_SO.language.id);

            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoWebAPIGeneric<List<categoryClass>>> libraryAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<categoryClass>>>(result);
                if (libraryAndStatus.Success)
                {
                    categories.fillData(libraryAndStatus.GenericObject.data);
                    success = true;
                }
                else
                {
                    createMessage("error");
                }

            }
            catch (HologoInternetException ex)
            {
                createMessage(ex.Message);
            }

            return success;
        }

        // updating the library . true is returned if there were any updates
        public async Task<bool> updateCategories()
        {
            /*1. first get library from server
             *2. compare updated date for each exp in library from server and the one on local storage
             *3. if exp item date from server is later we override the one in storage.
             */
            bool success = false;
            string result;
            List<categoryClass> categoryData = new List<categoryClass>();

            categoryServerConnect csc = new categoryServerConnect();

            var task = csc.loadCategoriesFromServer(dataPath.getUrl(0) + settings_SO.language.id);

            try
            {
                result = await task;

                GenericObjectAndStatus<HologoWebAPIGeneric<List<categoryClass>>> categoryAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<categoryClass>>>(result);
                if (categoryAndStatus.Success)
                {
                    categoryData = categoryAndStatus.GenericObject.data;
                    categories.fillData(categoryAndStatus.GenericObject.data);
                    saveCategoriesToStorage();
                    success = true;
                }
                else
                {
                    createMessage("error");
                }
            }
            catch (HologoInternetException ex)
            {

                createMessage(ex.Message);
            }

            //updateCategories uc = new updateCategories();
            //List<categoryClass> updatedCategories = new List<categoryClass>();


            //comparing both lists and getting latest updates
            //updatedCategories = uc.updatesLibrary(categories.getCategoryData(), categoryData);
            //if (updatedCategories.Count > 0)
            //{
            //    uc.copyOverUpdates(categories.getCategoryData(), updatedCategories);
            //    //save library to storage
            //    saveCategoriesToStorage();
            //    success = true;
            //}
            return success;
        }

        // helper functions
        bool checkIfCategoriesExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }


        public List<categoryClass> getCategories()
        {
            return categories.getCategoryData();
        }

        string readCategoriesFromStorage()
        {
            categoryServerConnect csc = new categoryServerConnect();
            return csc.loadCategoriesFromLocalStorage(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //convert stored data to data object
        bool convertLibraryData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, categories))
            {
                success = true;
            }
            return success;
        }

        public void clearCategoryAsset()
        {
            categories.clearAsset();
        }

        //save library data to storage
        public bool saveCategoriesToStorage()
        {
            categoryServerConnect csc = new categoryServerConnect();
            return csc.writeCategoryToStorage(categories, false, dataPath.getFolder(0), dataPath.getFileName(0));
        }

    }
}
