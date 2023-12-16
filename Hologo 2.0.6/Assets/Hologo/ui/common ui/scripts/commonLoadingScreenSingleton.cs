using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this for showing loading screen when user have to wait
// it will block further interaction with app untill loading screen is disabled
public class commonLoadingScreenSingleton : MonoBehaviour
{
    #region Singleton Pattern

    // this makes sure that this script will remain in memory throught out the life of the app
    public static commonLoadingScreenSingleton Singleton
    {
        get { return instance; }
    }

    private static commonLoadingScreenSingleton instance;

    void Awake()
    {
        if (Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public GameObject loadingDisplayCanvas;


    public void enableLoadingScreen()
    {
        loadingDisplayCanvas.SetActive(true);
    }

    public void disableLoadingScreen()
    {
        loadingDisplayCanvas.SetActive(false);
    }


   
}
