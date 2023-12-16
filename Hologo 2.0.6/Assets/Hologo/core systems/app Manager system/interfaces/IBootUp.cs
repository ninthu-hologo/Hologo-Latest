using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 08 july 2019
    /// Modified by: 
    /// interface to handle booting
    /// </summary>
    public interface IBootUp
    {
        void runBootSequence();

        bool didBoot();
    }
}
