using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this category data scriptable object
    /// </summary
    [CreateAssetMenu(fileName ="category.asset",menuName = "Hologo V2/new category Data")]
    public class categoryData_SO : ScriptableObject
    {
        [SerializeField]
        private List<categoryClass> categoryData;

        public void fillData(List<categoryClass> category)
        {
            categoryData = category;
            makeCategoryOrderInList();
        }

        public bool isCategoryfilled()
        {
            return categoryData.Count > 0;

        }

        public List<categoryClass> getCategoryData()
        {
            makeCategoryOrderInList();
            return categoryData;
        }


        public void clearAsset()
        {
            categoryData.Clear();
        }

        public void makeCategoryOrderInList()
        {
            for (int i = 0; i < categoryData.Count; i++)
            {
                categoryData[i].orderInList = i;
            }
        }

        public void flushData()
        {
            categoryData.Clear();
        }
    }
}
