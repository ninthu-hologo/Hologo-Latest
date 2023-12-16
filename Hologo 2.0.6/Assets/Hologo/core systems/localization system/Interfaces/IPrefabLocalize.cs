using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 June 2019
    /// Modified by: 
    /// this interface is localise the prfab assigned to the localize ui scriptable object
    /// </summary>
    public interface IPrefabLocalize
    {

        void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting);
    }
}
