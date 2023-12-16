using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: 
    /// callback for sending back gameobject info
    /// </summary>
    public delegate void CallbackGameObject(GameObject GO);

    // call back for sending an id back
    public delegate void CallbackForId(int id);
}