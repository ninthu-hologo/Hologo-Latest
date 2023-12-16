using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 June 2019
    /// Modified by: 
    /// this interface is to assign the language file
    /// </summary>
    public interface ILocalize
    {

        void setLanguage(languageData_SO language);

        void localizePrefab();

    }
}
