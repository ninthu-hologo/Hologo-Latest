using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// class that localizes the ui of experience slides and handles the functions
    /// </summary>
    public class appBootUpUICanvas : iUILayoutBase
    {

        public GameObject FirstTimeUI;
        public GameObject ConsequtiveUI;
        public GameObject NoInternetFirstTime;
        public GameObject AppForcedUpdate;


        public void setFirstBootUIState(bool state)
        {
            FirstTimeUI.SetActive(state);
            ConsequtiveUI.SetActive(!state);
        }

        public void NoInternetWarningFirstTime()
        {
            NoInternetFirstTime.SetActive(true);
        }

        public void ForcedUpdate()
        {
            AppForcedUpdate.SetActive(true);
        }
    }
}
