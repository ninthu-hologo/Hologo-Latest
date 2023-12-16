using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hologo2.library;
using System.Threading;
using UnityEngine;
using System;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 16 july 2019
    /// Modified by: 
    /// class that loads the assetbundle of experience and labels file and default audio narration
    /// </summary>
    public class experienceAssetManager : messageLogging
    {

        [Header("settings")]
        [SerializeField]
        private settings_SO mainSettings;
        [Header("data")]
        [SerializeField]
        private dataPaths_SO dataPath;
        [SerializeField]
        private userData_SO userData;

        //used for cancelling async method
        CancellationTokenSource tokenSource;
        [SerializeField]
        AssetBundle currentExpAssetBundle;
        [SerializeField]
        AssetBundle currentLocExpAssetBundle;
        [SerializeField]
        private experienceData_SO currentExperience;
        [SerializeField]
        private experienceLocalizationData_SO experienceLocalizedData;
        private assetBundleManager aBManager;




        public async Task<bool> loadExperience( assetBundleManager abm)
        {
            aBManager = abm;
            bool success = false;

            // hamid 2.01 hologo 12th november adding fix for download cut
            if (currentExpAssetBundle != null)
            {
                Debug.Log("Experience Asset Bundle already loaded");
                currentExpAssetBundle.Unload(false); 
                Debug.Log("Experience Asset Bundle unloaded");
            }

            if (currentLocExpAssetBundle != null)
            {
                Debug.Log("Local Experience Asset Bundle already loaded");
                currentLocExpAssetBundle.Unload(false); 
                Debug.Log("Local Experience Asset Bundle unloaded");
            }


            if (notPortal())
            {
                try
                {
                    success = await getExperienceLocalization();
                }
                catch (Exception ex)
                {
                    createMessage("Couldnt download or load experience! Please try again later");
                }
            }

            try
            {
                success = await getExperience();
            }
            catch (Exception ex)
            {
                createMessage("Couldnt download or load experience! Please try again later");
            }


            if (!success)
                return false;

            if (notPortal())
            {
                loadExperienceLocalizationAsset();
            }

            loadExperienceAsset();

            if (currentExperience == null)
            {
                Debug.Log("failed to load main experience");
                return false;
            }

            if (notPortal())
            {
                if (experienceLocalizedData == null)
                {
                    Debug.Log("failed to load localized experience");
                    return false;
                }

                if (currentExperience.isDatafilled() && experienceLocalizedData.isDatafilled())
                {
                    success = true;
                }
            }
            else
            {
                if (currentExperience.isDatafilled())
                {
                    success = true;
                }
            }
                

            return success;
        }

        bool notPortal()
        {
            return mainSettings.currentExperience.local_asset_bundles.Count > 0;
        }

        public experienceData_SO getCurrentExperience()
        {
            return currentExperience;
        }

        public experienceLocalizationData_SO getCurrentExperienceLocalizedData()
        {
            return experienceLocalizedData;
        }

        public quiz_SO getCurrentQuiz()
        {
            return currentExperience.quiz;
        }

        // loading experience
        public async Task<bool> getExperience()
        {
            bool success = false;
            experienceConnect ec = new experienceConnect();
            try
            {

                Debug.Log("loading exp from webrequest");
                // getting experience attachable details
                attachablesClass ac = mainSettings.currentExperience.getMainExperiencePlatformAttachable(mainSettings.getPlatform());
                // setting experience download path
                string path = dataPath.getUrl(0) + mainSettings.currentExperience.experience_id + dataPath.getUrl(1)+ ac.platform +"&token="+ userData.requestMyToken();
                Debug.Log(path);

               //currentExpAssetBundle = await ec.getAssetBundleExperience(path,"", ac.version, ac.crc);
               currentExpAssetBundle = await aBManager.getMainExperience(ac, path);

                if(currentExpAssetBundle == null)
                {
                    createMessage("Couldn't load experience! Please try again later");
                }
                else
                {
                    success = true;
                }
            }
            catch (HologoInternetException ex)
            {
                Debug.Log("something is wrong with the internet! please try again later");
                createMessage(ex.Message);
                //createMessage("something is wrong with the internet! please try again later");
            }
            catch (Exception ex)
            {
                Debug.Log("couldnt load experience! please try again later");
            }

            return success;
        }


        // loading experience
        public async Task<bool> getExperienceLocalization()
        {
            bool success = false;
            experienceConnect ec = new experienceConnect();
            try
            {
                //int crc;
                //   Int32.TryParse(mainSettings.experience.crc, out crc);

                attachablesClass ac = mainSettings.currentExperience.getlocalizedExperiencePlatformAttachable(mainSettings.getPlatform());

                string path = dataPath.getUrl(2) + mainSettings.currentExperience.id + dataPath.getUrl(1) + ac.platform + "&token=" + userData.requestMyToken();
                Debug.Log(path);
               //currentLocExpAssetBundle = await ec.getAssetBundleExperience(path,"", ac.version, ac.crc);

               currentLocExpAssetBundle = await aBManager.getLocalizedExperience(ac, path);
                if (currentLocExpAssetBundle == null)
                {
                    createMessage("Couldn't load experience! Please try again later");
                }
                else
                {
                    success = true;
                }
            }
            catch (HologoInternetException ex)
            {
                Debug.Log("something is wrong with the internet! please try again later");
                createMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.Log("couldnt load experience! please try again later");
            }

            return success;
        }

        // unloading experience
        public void unloadExperience()
        {
            experienceConnect ec = new experienceConnect();
            ec.unloadAssetBundle(currentExpAssetBundle);
            ec.unloadAssetBundle(currentLocExpAssetBundle);
        }

        // load experience model data object
        public void loadExperienceAsset()
        {
            Debug.Log("loading experience");
            experienceConnect ec = new experienceConnect();
            attachablesClass ac = mainSettings.currentExperience.getMainExperiencePlatformAttachable(mainSettings.getPlatform());
            currentExperience = ec.loadExperienceAsset(ac.title,currentExpAssetBundle);
        }

        public void loadExperienceLocalizationAsset()
        {
            Debug.Log("loading experience localization");
            attachablesClass ac = mainSettings.currentExperience.getlocalizedExperiencePlatformAttachable(mainSettings.getPlatform());
            experienceConnect ec = new experienceConnect();
            experienceLocalizedData = ec.loadExperienceLocalizationAsset(ac.title, currentLocExpAssetBundle);
        }

    }
}
