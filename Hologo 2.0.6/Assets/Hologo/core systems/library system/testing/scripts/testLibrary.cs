using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2.library
{
    public class testLibrary : MonoBehaviour
    {

        public libraryManager libManager;

        // Start is called before the first frame update
        void Start()
        {
            getLibrary();
        }


        private async void getLibrary()
        {
            bool success = false;
            var task = libManager.loadLibrary();

            success = await task;

            if(!success)
            {
                Debug.Log(" connect and try again");
            }else
            {
                Debug.Log(" success");
            }
        }
    }
}
