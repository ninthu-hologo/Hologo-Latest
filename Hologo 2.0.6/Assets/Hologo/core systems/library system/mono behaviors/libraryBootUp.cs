using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 july 2019
    /// Modified by: 
    /// boots up library
    /// </summary>
    public class libraryBootUp : messageLogging, IBootUp
    {

        [SerializeField]
        libraryManager lManager;
        [SerializeField]
        categoryManager cManager;

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
            bool success = await cManager.loadCategories();
            bootUpSuccess = await lManager.loadLibrary();
            lManager.cacheLibrary(cManager.getCategories());
            if(bootUpSuccess && success)
            {

            }
            else
            {
                bootUpSuccess = false;
            }
        }


        public void clearLibrary()
        {
            lManager.clearLibraryAsset();
            cManager.clearCategoryAsset();
        }
    }
}
