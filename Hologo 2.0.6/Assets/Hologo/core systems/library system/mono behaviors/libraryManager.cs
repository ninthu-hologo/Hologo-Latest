using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;



namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: 
    /// this class manages everthing to do with the library
    /// </summary>
    public partial class libraryManager : messageLogging
    {
        // library scriptable object
        public libraryData_SO library;
        private libraryServerConnect libLoader;
        // paths and names
        public dataPaths_SO dataPath;
        public settings_SO settings;

        private void Awake()
        {
            libLoader = new libraryServerConnect();
        }

        //main functions;

        public async Task<bool> loadLibrary()
        {
            /* 1. first check to see if library exits in the storage.
               2. if so load from storage - return true
               3. if not then try to connect to the server
               4. if cannot app cannot run and inform user to connect to the internet and launch app again - return false
               5. if can connect to server download library and save - return true */
            bool success = false;
            bool readFromStorage = false;

              // return true if the library is already filled
              if (library.isLibaryfilled())
                  return true;

              if (checkIfLibraryExists())
              {
                  string libDataString = readLibraryFromStorage();
                  if (string.IsNullOrEmpty(libDataString))
                  {
                      // failed loadind data;
                      // try to get library from internet
                      success = await loadLibraryFromServer();
                  }
                  else
                  {
                      //success
                      //try convert data read from file to object data
                      success = readFromStorage = convertLibraryData(libDataString);
                      //failed try to get library from internet
                      if (!success)
                      {
                          success = await loadLibraryFromServer();
                      }

                  }
              }
              else
              {
                  //failed try to get library from internet
                  success = await loadLibraryFromServer();
              }

              if (success && !readFromStorage)
              {
                  SavelibraryToStorage();
              }
          
            return success;
        }


        public void cacheLibrary(List<categoryClass> categories)
        {
            library.cacheFilteredLibrary(categories);
        }


        // updating the library . true is returned if there were any updates
        public async Task<bool> updateLibrary(List<categoryClass> categories)
        {
            /*1. first get library from server
             *2. compare updated date for each exp in library from server and the one on local storage
             *3. if exp item date from server is later we override the one in storage.
             */
            bool success = false;
            string result;
            List<experienceDataClass> libraryData = new List<experienceDataClass> ();
            string path = dataPath.getUrl(0) + settings.curriculum.id + dataPath.getUrl(3);
            var task = libLoader.updateExperiencesFromServer(path);

            try
            {
                result = await task;

                GenericObjectAndStatus<HologoWebAPIGeneric<List<experienceDataClass>>> libraryAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<experienceDataClass>>>(result);
                if (libraryAndStatus.Success)
                {
                    libraryData = libraryAndStatus.GenericObject.data;
                    
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

            updateLibrary ul = new updateLibrary();
            List<experienceDataClass> updatedlibrary = new List<experienceDataClass>();

            //comparing both lists and getting latest updates
            updatedlibrary = ul.updatesLibrary(library.getLibraryData(), libraryData);
            if (updatedlibrary.Count > 0)
            {
                ul.copyOverUpdates(library.getLibraryData(), updatedlibrary);
                cacheLibrary(categories);
                //save library to storage

                SavelibraryToStorage();
                success = true;
            }
            return success;
        }


        void cacheNewExperiences(List<experienceDataClass> expData)
        {
            if (expData.Count <= 0)
                return;


        }

        // helper functions
        bool checkIfLibraryExists()
        {
            return readWriteData.CheckIfFileExists(dataPath.getFolder(0), dataPath.getFileName(0));
        }


        string readLibraryFromStorage()
        {
            return libLoader.loadExperiencesFromLocalStorage(dataPath.getFolder(0), dataPath.getFileName(0));
        }

        //convert stored data to data object
        bool convertLibraryData(string data)
        {
            bool success = false;
            if (jsonHelper.DeserializeJsonToScriptableObject(data, library))
            {
                Debug.Log("did convert");
                success = true;
            }
            else
            {
                Debug.Log("did not convert");
            }
            return success;
        }

        //load library from server
        private async Task<bool> loadLibraryFromServer()
        {
            bool success = false;
            string result;

            string path = dataPath.getUrl(0) + settings.curriculum.id + dataPath.getUrl(3);
            Debug.Log(path);
            var task = libLoader.loadExperiencesFromServer(path);

            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoWebAPIGeneric<List<experienceDataClass>>> libraryAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<experienceDataClass>>>(result);
                if (libraryAndStatus.Success)
                {
                    library.fillData(libraryAndStatus.GenericObject.data);
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

        public void clearLibraryAsset()
        {
            library.clearAsset();
          
        }

        //save library data to storage
        public bool SavelibraryToStorage()
        {
            return libLoader.writeLibraryToStorage(library, false, dataPath.getFolder(0),dataPath.getFileName(0));
        }

    }
}

