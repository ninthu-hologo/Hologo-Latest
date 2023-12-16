using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class gets categories from server or local
    /// </summary>
    public class updateMaintenanceServerConnect
    {

        public async Task<string> getUpdateAndMaintenanceStatusFromServer(string url)
        {
            return await InternetfetchData.fetchDataFromServer(url);
        }

        


    }
}
