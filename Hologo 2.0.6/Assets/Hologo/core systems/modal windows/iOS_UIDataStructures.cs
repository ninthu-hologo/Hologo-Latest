using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace Hologo.iOSUI
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) 22-03-2018
    /// Modified by:
    /// iOS ui data classes.
    /// </summary>
    [System.Serializable]
    public class iOSUIRect
    {
        public RectTransform iOSRect;
        public float Destination;
        public float StartOffsetX;
        public float StartOffsetY;
    }
}