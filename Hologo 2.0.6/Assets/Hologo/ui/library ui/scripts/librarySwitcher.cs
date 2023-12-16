using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
public class librarySwitcher : MonoBehaviour
{
  //Shariz 15th September 2019 v2
  
[Header("DATA")]
[SerializeField]
private settings_SO mainSettings;


[Header("UI CONNECTOR")]
        [SerializeField]
        private libraryUIConnect lConnectMobile;

        [SerializeField]
        private libraryUIConnect lConnectTablet;

    
    
    public libraryUIConnect getUIConnect()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            lConnectTablet.setLibraryUIState(true);
            lConnectMobile.setLibraryUIState(false);
            return lConnectTablet;
        }
            lConnectTablet.setLibraryUIState(false);
            lConnectMobile.setLibraryUIState(true);
        return lConnectMobile;
    }
}
