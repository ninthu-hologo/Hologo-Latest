using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 06 july 2019
    /// Modified by: 
    /// this class data object is for category
    /// </summary>
    [Serializable]
    public class categoryClass : IEquatable<categoryClass>, IdataObject
    {
        public string title;
        public int id;
        public string updated_at;
        public bool isNew;
        public List<subCategoryClass> children;
        public int orderInList;
        public bool status;

        public categoryClass(string subjectName, int id)
        {
            title = subjectName;
            this.id = id;
        }

        public bool dateCompare(categoryClass other)
        {
            return dateChecking.CompareDateTime(this.updated_at, other.updated_at);
        }

        public void setCategoryData(categoryClass other)
        {
            title = other.title;
            updated_at = other.updated_at;
        }

        public bool Equals(categoryClass other)
        {
            if (other == null)
                return false;
            if (this.title.Equals(other.title, StringComparison.Ordinal) &&
               this.id == other.id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    [Serializable]
    public class subCategoryClass : IdataObject
    {
        public string title;
        public int id;
        public string updated_at;
        public bool isNew;
        public bool status;

    }
}