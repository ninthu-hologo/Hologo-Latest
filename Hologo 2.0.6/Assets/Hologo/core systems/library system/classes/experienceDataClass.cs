using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace Hologo2.library
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 27 may 2019
    /// Modified by: Shariz - 21st oct 2019
    /// this class is a data object for servers incoming experience
    /// and also used in app functions
    /// </summary>
    [Serializable]
    public class experienceDataClass : IdataObject, IEquatable<experienceDataClass>, IDateComparison<experienceDataClass>
    {
        // id in database
        public string title;
        public int id;
        public int experience_id;
        public bool featured;
        // name in database this should be localized
        public bool international;
        public string body;
        // subject id this experience belongs to
        public int subject_id;
        public bool free;

        //updated date from server
        public string updated_at;

        public mainExperienceDetails experience;

        public List<attachablesClass> local_asset_bundles;

        
        // bool to determine whether data has changed for this experince entry
        public bool IsDataChanged;
        // temporary bool for newer experiences when library is updating.
        public bool isNew;

        public bool dateCompare(experienceDataClass other)
        {
            return dateChecking.CompareDateTime(experience.updated_at, other.experience.updated_at);
        }

        // for updating the experience
        public void setExperienceItemData(experienceDataClass otherExp)
        {
            title = otherExp.title;
            body = otherExp.body;
            subject_id = otherExp.subject_id;
            updated_at = otherExp.updated_at;
            featured = otherExp.featured;
            free = otherExp.free;

            IsDataChanged = true;

        }

        // return attachable detail for specidied platform
        public attachablesClass getMainExperiencePlatformAttachable(string platform)
        {
            for (int i = 0; i < experience.asset_bundles.Count; i++)
            {
                if(string.Equals(experience.asset_bundles[i].platform, platform, StringComparison.Ordinal))
                {
                    return experience.asset_bundles[i];
                }
            }

            return null;
        }

        public attachablesClass getlocalizedExperiencePlatformAttachable(string platform)
        {
            for (int i = 0; i < local_asset_bundles.Count; i++)
            {
                if (string.Equals(local_asset_bundles[i].platform, platform, StringComparison.Ordinal))
                {
                    return local_asset_bundles[i];
                }
            }

            return null;
        }


        //comparing if one is equal to the other
        public bool Equals(experienceDataClass other)
        {

            if (this.id == other.id)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        public int calculateStarsAchieved(float timeTaken, int attemps) // shariz changed less thans to greater thans for it to work correctly oct 21st 
        {
            int star = 0;
           if(experience.game != null)
            {
               
                if(experience.game.tire_one_attempt >= attemps && experience.game.tire_one_time >= timeTaken)
                {
                    star = 3;
                    return star;
                }

                if(experience.game.tire_two_attempt >= attemps && experience.game.tire_two_time >= timeTaken)
                {
                    star = 2;
                    return star;
                }

                star = 1;
                return star;
            }
           else
            {
                return 3;
            }
        }


        


    }

   


    [Serializable]
    public class mainExperienceDetails
    {
        public int subject_id;
        public bool featured;
        public bool free;
        public bool status;

        //updated date from server
        public string updated_at;
        // image of exprience
        public attachablesClass image;

       
        // asset details of the experience
        public List<attachablesClass> asset_bundles;

        public quizStarCriteria game;

    }


    [Serializable]
    public class LibraryCategory
    {
        public string name;
        public List<experienceDataClass> featuredList;
        public List<SubcategoryAndExperiences> subCategoryAndExps;

    }

    [Serializable]
    public class SubcategoryAndExperiences
    {
        public string name;
        public subCategoryClass subcategory;
        public List<experienceDataClass> subExperiences;
    }

}
