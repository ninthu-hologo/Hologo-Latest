using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 july 2019
    /// Modified by: 
    /// boots up library
    /// </summary>
    public class assetBundleBootUp : messageLogging, IBootUp
    {
        // Start is called before the first frame update
        [SerializeField]
        assetBundleManager aBManager;
        

        private bool bootUpSuccess;

        public bool didBoot()
        {
            return bootUpSuccess;

        }

        public void runBootSequence()
        {
            //  initLibrary();
        }



        public async Task runBootUp()
        {
            bootUpSuccess = aBManager.loadAssetBundleDataFromStorage();
        }

    }
}