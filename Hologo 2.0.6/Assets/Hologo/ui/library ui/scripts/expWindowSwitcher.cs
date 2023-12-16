using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
using Hologo.iOSUI;

public class expWindowSwitcher : MonoBehaviour
{
   //Shariz 15th September 2019 v2

   
[Header("DATA")]
[SerializeField]
private settings_SO mainSettings;


[Header("UI CONNECTOR")]
        [SerializeField]
        private experienceDetailsUIWindow experienceDetailsUIWindowMobile;

        [SerializeField]
        private experienceDetailsUIWindow experienceDetailsUIWindowTablet;

        [SerializeField]
        private iOS_ModalWindowE ios_ModalWindowEMobile;
        
        [SerializeField]
        private iOS_ModalWindowE ios_ModalWindowETablet;
        [SerializeField]
        public iOS_ModalWindowF ios_ModalWindowFMobile;
        
        [SerializeField]
        public iOS_ModalWindowF ios_ModalWindowFTablet;

    //[SerializeField]
    //private iOS_UIModalWindowMain tabletModalWindows;
    //[SerializeField]
    //private iOS_UIModalWindowMain mobileModalWindows;





    public experienceDetailsUIWindow getExpDetailWindow()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return experienceDetailsUIWindowTablet;
        }
        return experienceDetailsUIWindowMobile;
    }
    public iOS_ModalWindowE getiOS_ModalWindowE()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return ios_ModalWindowETablet;
        }
        return ios_ModalWindowEMobile;
    }
    public iOS_ModalWindowF getiOS_ModalWindowF()
    {
        if (mainSettings.device == deviceType.TABLET)
        {
            return ios_ModalWindowFTablet;
        }
        return ios_ModalWindowFMobile;
    }


    //public iOS_UIModalWindowMain getWindowMain()
    //{
    //    if (mainSettings.device == deviceType.TABLET)
    //    {
    //        return tabletModalWindows;
    //    }
    //    return mobileModalWindows;
    //}
}
