using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2.library;



namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 10 September 2019
    /// Modified by: 
    /// holds all the data of downloaded asset bundles
    /// </summary>
    [CreateAssetMenu(fileName = "assetBundleData.asset", menuName = "Hologo V2/new assetBundle data")]
    public class assetBundelData_SO : ScriptableObject
    {
        [SerializeField]
        private List<attachablesClass> assetBundles;



        public bool isDataFilled()
        {
            return assetBundles.Count > 0;
        }


        public attachablesClass getAssetBundleData(attachablesClass ac)
        {
            for (int i = 0; i < assetBundles.Count; i++)
            {
                if (assetBundles[i].Equals(ac))
                {
                    return assetBundles[i];
                }
            }

            return null;
        }

        public void addAssetBundleToList(attachablesClass data)
        {
            if (assetBundles == null)
                assetBundles = new List<attachablesClass>();

            bool exists = false;
            for (int i = 0; i < assetBundles.Count; i++)
            {
                if (assetBundles[i].id==data.id)
                {
                    exists = true;
                    if (assetBundles[i].versionCheck(data))
                    {
                        assetBundles[i] = data;
                    }
                }
            }

            if (!exists)
                assetBundles.Add(data);
        }

        public void ClearList()
        {
            assetBundles.Clear();
        }

    }
}