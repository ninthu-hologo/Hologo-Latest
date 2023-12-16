using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 17 july 2019
    /// Modified by: 
    /// scriptable object that keeps labels of the current experience loaded
    /// </summary>
    [CreateAssetMenu(fileName = "experienceLocalize.asset", menuName = "Hologo V2/Experience Asset/new experienceLocalize")]
    public class experienceLocalizationData_SO : ScriptableObject
    {
        [Header("EXPERIENCE LOCALIZATION")]
        public List<experienceLabelNarrationDataClass> experienceSlides;
        [Header("QUIZ LOCALIZATION")]
        public List<localzationQuizSlide> quizSlides;
        public int selectLabelListToCopy = 0;

        // will reverse the list if its from right to left
        public List<experienceLabelNarrationDataClass> giveSlides()
        {
            return experienceSlides;
        }


        public bool isDatafilled()
        {
            return experienceSlides.Count > 0;

        }

        public void copyList()
        {
            try
            {
                quizSlides[0].quizlabels = experienceSlides[selectLabelListToCopy].labelList;
            }
            catch (System.Exception)
            {
                Debug.Log("couldnt copy");
            }
           
        }
    }
}
