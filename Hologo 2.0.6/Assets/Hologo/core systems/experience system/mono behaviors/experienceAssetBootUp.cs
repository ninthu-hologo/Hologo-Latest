using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 22 july 2019
    /// Modified by: 
    /// boots up experience asset manager
    /// </summary>
    public class experienceAssetBootUp : messageLogging, IBootUp
    {

        [SerializeField]
        experienceAssetManager eAManager;
        [SerializeField]
        assetBundleManager aBManager;

        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;

        }

        public string getMessages()
        {
            return eAManager.readMessage();
        }

        public void runBootSequence()
        {
            //  initLibrary();
        }

        public async Task runBootUp()
        {
            bootUpSuccess = await eAManager.loadExperience(aBManager);
            
        }
    }
}
